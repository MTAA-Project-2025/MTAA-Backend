using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MTAA_Backend.Application.CQRS.Users.Identity.Events;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Interfaces.Locations;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem.RecommendationFeedService;
using MTAA_Backend.Domain.Resources.Customers;
using MTAA_Backend.Domain.Resources.Localization;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MTAA_Backend.Api.Extensions
{
    public static class WebApplicationExtension
    {
        public static void ConfigureLocalization(this WebApplication app)
        {
            var langCodes = Languages.GetAll();
            var supportedCultures = new List<CultureInfo>(langCodes.Count);

            foreach (var langCode in langCodes)
            {
                supportedCultures.Add(new CultureInfo(langCode));
            }

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(Languages.EN),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
        public static async Task ConfigureQdrant(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var client = scope.ServiceProvider.GetRequiredService<QdrantClient>();

            var existedCollections = await client.ListCollectionsAsync();

/*            foreach(var name in existedCollections)
            {
                await client.DeleteCollectionAsync(name);
            }
            existedCollections = await client.ListCollectionsAsync();*/

            if (!existedCollections.Contains(VectorCollections.PostTextEmbeddings))
            {
                await client.CreateCollectionAsync(VectorCollections.PostTextEmbeddings,
                    vectorsConfig: new VectorParams()
                    {
                        Size = (ulong)VectorCollections.PostTextEmbeddingsSize,
                        Distance = Distance.Euclid,
                    });

                await client.CreatePayloadIndexAsync(VectorCollections.PostTextEmbeddings,
                    fieldName: "watched");
            }
            if (!existedCollections.Contains(VectorCollections.PostImageEmbeddings))
            {
                await client.CreateCollectionAsync(VectorCollections.PostImageEmbeddings,
                    vectorsConfig: new VectorParams()
                    {
                        Size = (ulong)VectorCollections.PostImageEmbeddingsSize,
                        Distance = Distance.Euclid,
                    });

                await client.CreatePayloadIndexAsync(VectorCollections.PostImageEmbeddings,
                    fieldName: "watched");
            }
            if (!existedCollections.Contains(VectorCollections.UsersPostTextVectors))
            {
                await client.CreateCollectionAsync(VectorCollections.UsersPostTextVectors,
                    vectorsConfig: new VectorParams()
                    {
                        Size = (ulong)VectorCollections.UsersPostTextVectorsSize,
                        Distance = Distance.Euclid,
                    });

                var userVector = new float[VectorCollections.UsersPostTextVectorsSize];
                await client.UpsertAsync(VectorCollections.UsersPostTextVectors,
                    points: new List<PointStruct>
                    {
                            new PointStruct()
                            {
                                Id = Guid.NewGuid(),
                                Vectors = userVector,
                            }
                    });
            }
            if (!existedCollections.Contains(VectorCollections.UsersPostImageVectors))
            {
                await client.CreateCollectionAsync(VectorCollections.UsersPostImageVectors,
                    vectorsConfig: new VectorParams()
                    {
                        Size = (ulong)VectorCollections.UsersPostImageVectorsSize,
                        Distance = Distance.Euclid,
                    });

                var userVector = new float[VectorCollections.UsersPostImageVectorsSize];
                await client.UpsertAsync(VectorCollections.UsersPostImageVectors,
                    points: new List<PointStruct>
                    {
                            new PointStruct()
                            {
                                Id = Guid.NewGuid(),
                                Vectors = userVector,
                            }
                    });
            }
        }

        public static void ConfigureHangfireJobs(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var globalPopularityService = scope.ServiceProvider.GetRequiredService<IPostsFromGlobalPopularityRecommendationFeedService>();
            var preferencesPopularityService = scope.ServiceProvider.GetRequiredService<IPostsFromPreferencesRecommendationFeedService>();
            var locationsService = scope.ServiceProvider.GetRequiredService<ILocationService>();

            var recurringJobs = app.Services.GetService<IRecurringJobManager>();
            recurringJobs.AddOrUpdate(
                "update-global-posts-Recommendation",
                () => globalPopularityService.RecomendPostsBackgroundJob(CancellationToken.None),
                Cron.Hourly()
            );

            recurringJobs.AddOrUpdate(
                "update-posts-from-preferences-Recommendation",
            () => preferencesPopularityService.RecomendPostsBackgroundJob(CancellationToken.None),
                Cron.Hourly()
            );


            recurringJobs.AddOrUpdate(
                "correct-locations",
            () => preferencesPopularityService.RecomendPostsBackgroundJob(CancellationToken.None),
                Cron.Hourly()
            );
        }

        public static async Task ConfigureAdminUser(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            UserManager<User> _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var userSeederOptions = app.Configuration.GetSection("UserSeeder");

            var email = userSeederOptions["Email"];
            var password = userSeederOptions["Password"];
            var username = userSeederOptions["Username"];

            if (email == null || password == null || username == null) return;

            if (await _userManager.FindByEmailAsync(email) != null) return;

            var newUser = new User
            {
                UserName = username,
                Email = email,
                DisplayName = username,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(newUser, password);

            RoleManager<IdentityRole> _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Moderator))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Moderator));

            if (await _roleManager.RoleExistsAsync(UserRoles.Moderator))
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Moderator);
            }

            IMediator _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await _mediator.Publish(new CreateAccountEvent()
            {
                UserId = newUser.Id
            });
        }
    }
}
