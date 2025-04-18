﻿using FakeItEasy;
using IntegrationTests.Fixtures;
using MTAA_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using IntegrationTests.Helpers;
using MTAA_Backend.Domain.DTOs.Users.Identity.Responses;
using MTAA_Backend.Domain.DTOs.Users.Identity.Requests;
using Microsoft.AspNetCore.Identity;
using MTAA_Backend.Infrastructure;
using MTAA_Backend.Domain.Entities.Users;
using System.Drawing.Imaging;
using System.Drawing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using MTAA_Backend.Domain.Entities.Images;
using MTAA_Backend.Domain.Resources.Images;
using MTAA_Backend.Domain.DTOs.Users.Account.Requests;
using Xunit.Extensions.AssemblyFixture;

namespace IntegrationTests.Tests
{
    [Collection("Fixture collection")]
    public class AccountUpdateTests
    {
        private readonly HttpClient _client;
        private readonly IAzureBlobService _blobService;

        public AccountUpdateTests(ApiFixture fixture)
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

        #region update account avatar
        [Fact]
        public async Task Update_With_Valid_Image()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            IFormFile imageFile = CreateValidImageFile();
            var stream = imageFile.OpenReadStream();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

            var form = new MultipartFormDataContent
            {
                { fileContent, "Avatar", imageFile.FileName }
            };

            var response = await _client.PutAsync("/api/v1/Account/custom-update-account-avatar", form);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        public static IFormFile CreateValidImageFile(string fileName = "valid-image.jpg")
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

        [Fact]
        public async Task Update_With_Large_Image()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(new byte[11 * 1024 * 1024]); // 11MB image
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            formData.Add(fileContent, "Avatar", "large_image.jpg");

            var response = await _client.PutAsync("/api/v1/Account/custom-update-account-avatar", formData);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_With_Null_Image()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(string.Empty), "Avatar");

            var response = await _client.PutAsync("/api/v1/Account/custom-update-account-avatar", formData);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_With_NonImage_File()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(new byte[1024]); // 1KB text file
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");
            formData.Add(fileContent, "Avatar", "document.txt");

            var response = await _client.PutAsync("/api/v1/Account/custom-update-account-avatar", formData);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_With_Unregistered_User()
        {
            var unauthorizedClient = _client;
            var formData = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(new byte[1024 * 1024]); // 1MB image
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            formData.Add(fileContent, "Avatar", "image.jpg");

            var response = await unauthorizedClient.PutAsync("/api/v1/Account/custom-update-account-avatar", formData);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion

        #region update account preset avatar
        [Fact]
        public async Task Update_With_Valid_Preset_Image()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new PresetUpdateAccountAvatarRequest { ImageGroupId = PresetAvatarImages.Image1Id };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/preset-update-account-avatar", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Update_Twice_With_Valid_Preset_Image()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new PresetUpdateAccountAvatarRequest { ImageGroupId = PresetAvatarImages.Image1Id };
            var response1 = await _client.PutAsJsonAsync("/api/v1/Account/preset-update-account-avatar", request);
            var response2 = await _client.PutAsJsonAsync("/api/v1/Account/preset-update-account-avatar", request);

            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        }

        [Fact]
        public async Task Update_With_Invalid_Preset_Image_Id()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new PresetUpdateAccountAvatarRequest { ImageGroupId = Guid.NewGuid() };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/preset-update-account-avatar", request);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Update_Preset_Image_With_Unregistered_User()
        {
            var request = new PresetUpdateAccountAvatarRequest { ImageGroupId = PresetAvatarImages.Image1Id };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/preset-update-account-avatar", request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion

        #region update account birth date
        [Fact]
        public async Task Update_BirthDate_With_Valid_Date()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountBirthDateRequest { BirthDate = new DateTime(1990, 5, 20) };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-birth-date", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Update_BirthDate_With_Date_Before_1900()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountBirthDateRequest { BirthDate = new DateTime(1899, 12, 31) };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-birth-date", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_BirthDate_With_Future_Date()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountBirthDateRequest { BirthDate = DateTime.UtcNow.AddYears(1) };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-birth-date", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_BirthDate_With_Unregistered_User()
        {
            var request = new UpdateAccountBirthDateRequest { BirthDate = new DateTime(1995, 8, 15) };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-birth-date", request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion

        #region update account display name
        [Fact]
        public async Task Update_With_Valid_DisplayName()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountDisplayNameRequest { DisplayName = "ValidName123" };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-display-name", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Update_With_Short_DisplayName()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountDisplayNameRequest { DisplayName = "AB" };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-display-name", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_With_Long_DisplayName()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountDisplayNameRequest { DisplayName = new string('A', 101) };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-display-name", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_With_Invalid_Symbols()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountDisplayNameRequest { DisplayName = "Invalid@#$%" };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-display-name", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_DisplayName_Unregistered_User()
        {
            var request = new UpdateAccountDisplayNameRequest { DisplayName = "ValidName" };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-display-name", request);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        #endregion

        #region update account username
        [Fact]
        public async Task Update_With_Valid_Username()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountUsernameRequest { Username = "ValidName123" };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-username", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Update_With_Short_Username()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountUsernameRequest { Username = "AB" };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-username", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_With_Long_Username()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountUsernameRequest { Username = new string('A', 51) };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-username", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_Username_With_Invalid_Symbols()
        {
            var token = await GetValidTokenForTestUser();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new UpdateAccountUsernameRequest { Username = "Invalid@#$%" };
            var response = await _client.PutAsJsonAsync("/api/v1/Account/update-account-username", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        #endregion
    }
}
