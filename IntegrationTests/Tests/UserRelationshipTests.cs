using IntegrationTests.Fixtures;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Identity;
using MTAA_Backend.Application.CQRS.Users.Relationships.Commands;
using MTAA_Backend.Domain.DTOs.Users.Identity.Requests;
using MTAA_Backend.Domain.DTOs.Users.Identity.Responses;
using MTAA_Backend.Domain.Entities.Users;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Tests
{
    [Collection("Fixture collection")]
    public class UserRelationshipTests
    {
        private readonly HttpClient _client;

        public UserRelationshipTests(ApiFixture fixture)
        {
            using (var scope = fixture.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MTAA_BackendDbContext>();
                var userManager = scopedServices.GetRequiredService<UserManager<User>>();
                var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                Utilities.InitializeTestUser(db, userManager, roleManager).Wait();
                Utilities.CreateAdditionalTestUser(db, userManager).Wait();
            }

            _client = fixture.CreateClient();
        }

        private async Task<string> GetValidTokenForTestUser()
        {
            var loginRequest = new LogInRequest
            {
                Email = UserSettings.Email,
                Password = UserSettings.Password
            };

            var response = await _client.PostAsJsonAsync("/api/v1/Identity/log-in", loginRequest);
            response.EnsureSuccessStatusCode();
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenDTO>();
            return tokenResponse.Token;
        }

        #region follow
        [Fact]
        public async Task Follow_ValidTargetUser()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new Follow { TargetUserId = UserSettings.SecondUserId };
            var response = await _client.PostAsJsonAsync("/api/v1/Users/follow", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Follow_Self()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new Follow { TargetUserId = UserSettings.UserId };
            var response = await _client.PostAsJsonAsync("/api/v1/Users/follow", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Follow_NonExistentUser()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new Follow { TargetUserId = "nonexistent-user-id" };
            var response = await _client.PostAsJsonAsync("/api/v1/Users/follow", request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Follow_WithoutToken()
        {
            var request = new Follow { TargetUserId = UserSettings.SecondUserId };
            var response = await _client.PostAsJsonAsync("/api/v1/Users/follow", request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Follow_AlreadyFollowing()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new Follow { TargetUserId = UserSettings.SecondUserId };
            var response1 = await _client.PostAsJsonAsync("/api/v1/Users/follow", request);
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            var response2 = await _client.PostAsJsonAsync("/api/v1/Users/follow", request);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        }
        #endregion
    }
}
