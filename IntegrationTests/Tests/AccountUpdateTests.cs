using FakeItEasy;
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

namespace IntegrationTests.Tests
{
    public class AccountUpdateTests : IClassFixture<ApiFixture>
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
                Password = UserSettings.Password  // Use the password set in UserSettings
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
            // Get the token after logging in
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
            // Get the token after logging in
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
            // Get the token after logging in
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
            // Get the token after logging in
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
    }
}
