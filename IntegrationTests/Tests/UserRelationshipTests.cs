using IntegrationTests.Fixtures;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Identity;
using MTAA_Backend.Application.CQRS.Users.Relationships.Commands;
using MTAA_Backend.Domain.DTOs.Users.Account.Responses;
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

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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

        #region unfollow
        [Fact]
        public async Task Unfollow_ValidTargetUser()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var followRequest = new Follow { TargetUserId = UserSettings.SecondUserId };
            await _client.PostAsJsonAsync("/api/v1/Users/follow", followRequest);

            var unfollowRequest = new Unfollow { TargetUserId = UserSettings.SecondUserId };
            var response = await _client.PostAsJsonAsync("/api/v1/Users/unfollow", unfollowRequest);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Unfollow_NotFollowingUser()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var unfollowRequest = new Unfollow { TargetUserId = UserSettings.SecondUserId };
            var response = await _client.PostAsJsonAsync("/api/v1/Users/unfollow", unfollowRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Unfollow_NonExistentUser()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var unfollowRequest = new Unfollow { TargetUserId = "nonexistent-user-id" };
            var response = await _client.PostAsJsonAsync("/api/v1/Users/unfollow", unfollowRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Unfollow_WithoutToken()
        {
            var unfollowRequest = new Unfollow { TargetUserId = UserSettings.SecondUserId };
            var response = await _client.PostAsJsonAsync("/api/v1/Users/unfollow", unfollowRequest);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Unfollow_TwiceInARow()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var followRequest = new Follow { TargetUserId = UserSettings.SecondUserId };
            await _client.PostAsJsonAsync("/api/v1/Users/follow", followRequest);

            var unfollowRequest = new Unfollow { TargetUserId = UserSettings.SecondUserId };
            var firstUnfollow = await _client.PostAsJsonAsync("/api/v1/Users/unfollow", unfollowRequest);
            Assert.Equal(HttpStatusCode.OK, firstUnfollow.StatusCode);

            var secondUnfollow = await _client.PostAsJsonAsync("/api/v1/Users/unfollow", unfollowRequest);
            Assert.Equal(HttpStatusCode.BadRequest, secondUnfollow.StatusCode);
        }
        #endregion

        #region public full account

        [Fact]
        public async Task PublicGetFullAccount_WithoutToken()
        {
            var response = await _client.GetAsync($"/api/v1/Users/public-full-account/{UserSettings.SecondUserId}");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task PublicGetFullAccount_ValidUser()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"/api/v1/Users/public-full-account/{UserSettings.SecondUserId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dto = await response.Content.ReadFromJsonAsync<PublicFullAccountResponse>();
            Assert.NotNull(dto);
            Assert.Equal(UserSettings.SecondUserId, dto.Id);
            Assert.True(dto.DataCreationTime <= DateTime.UtcNow);
        }

        [Fact]
        public async Task PublicGetFullAccount_AfterFollow()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var followReq = new Follow { TargetUserId = UserSettings.SecondUserId };
            var followRes = await _client.PostAsJsonAsync("/api/v1/Users/follow", followReq);
            Assert.Equal(HttpStatusCode.OK, followRes.StatusCode);

            var response = await _client.GetAsync($"/api/v1/Users/public-full-account/{UserSettings.SecondUserId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dto = await response.Content.ReadFromJsonAsync<PublicFullAccountResponse>();
            Assert.True(dto.IsFollowing, "After following, IsFollowing should be true");
            Assert.Equal(0, dto.FriendsCount);
            Assert.Equal(1, dto.FollowersCount);
        }

        [Fact]
        public async Task PublicGetFullAccount_NonExistentUser()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var fakeId = "nonexistent-user-id";
            var response = await _client.GetAsync($"/api/v1/Users/public-full-account/{fakeId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion
    }
}
