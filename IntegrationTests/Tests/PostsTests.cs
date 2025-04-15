using FakeItEasy;
using IntegrationTests.Fixtures;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MTAA_Backend.Domain.DTOs.Users.Identity.Requests;
using MTAA_Backend.Domain.DTOs.Users.Identity.Responses;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Infrastructure;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static Betalgo.Ranul.OpenAI.ObjectModels.StaticValues.AssistantsStatics.MessageStatics;

namespace IntegrationTests.Tests
{
    [Collection("Fixture collection")]
    public class PostsTests
    {
        private readonly HttpClient _client;
        private readonly IAzureBlobService _blobService;

        public PostsTests(ApiFixture fixture)
        {
            using (var scope = fixture.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scope.ServiceProvider.GetRequiredService<MTAA_BackendDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                Utilities.InitializeTestUser(db, userManager, roleManager).Wait();
            }

            _blobService = A.Fake<IAzureBlobService>();
            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_blobService);
                });
            }).CreateClient();
        }

        public static IFormFile CreateValidImageFile1x1(string fileName = "valid-image.jpg")
        {
            var image = new Image<Rgba32>(100, 100);

            image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.Blue));

            var stream = new MemoryStream();
            image.Save(stream, new JpegEncoder());

            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
        }

        private async Task<string> GetValidTokenForTestUser()
        {
            var loginRequest = new LogInRequest()
            {
                Email = UserSettings.Email,
                Password = UserSettings.Password
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Identity/log-in", loginRequest);
            response.EnsureSuccessStatusCode();
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenDTO>();
            return tokenResponse.Token;
        }

        [Fact]
        public async Task Add_Without_Location_Valid()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var form = new MultipartFormDataContent();

            for (int i = 0; i < 3; i++)
            {
                IFormFile imageFile = CreateValidImageFile1x1();
                var stream = imageFile.OpenReadStream();
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

                form.Add(fileContent, $"images[{i}].image", imageFile.FileName);
                form.Add(fileContent, $"images[{i}].position", imageFile.FileName);
            }
            form.Add(new StringContent("Test 123"), "Description");

            var response = await _client.PutAsync("/api/v1/Posts/add", form);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
