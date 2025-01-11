using FakeItEasy;
using IntegrationTests.Fixtures;
using MTAA_Backend.Application.Identity.Commands;
using MTAA_Backend.Domain.DTOs.Users.Requests;
using MTAA_Backend.Domain.DTOs.Users.Responses;
using MTAA_Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.DependencyInjection;
using Azure;
using IntegrationTests.Helpers;
using System.Net;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;
using MTAA_Backend.Infrastructure;
using MTAA_Backend.Domain.Entities.Users;

namespace IntegrationTests.Tests
{
    public class IdentityEndPointsTests : IClassFixture<ApiFixture>
    {
        private readonly HttpClient _client;

        public IdentityEndPointsTests(ApiFixture fixture, ITestOutputHelper output)
        {
            using (var scope = fixture.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scope.ServiceProvider.GetRequiredService<MTAA_BackendDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                Utilities.InitializeTestUser(db, userManager, roleManager).Wait();
            }

            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var codeGeneratorService = A.Fake<ICodeGeneratorService>();
                    A.CallTo(() => codeGeneratorService.Generate6DigitCode()).Returns("123456");
                    services.AddSingleton(codeGeneratorService);

                    var emailService = A.Fake<IEmailService>();
                    A.CallTo(() => emailService.SendSighUpVerificationEmail("test@test.com","123456"));
                    services.AddSingleton(emailService);

                });
            }).CreateClient();
        }

        #region sign-up
        /**
         * Tests the Full email Sign Up.
         * Ensures that all Sign Up steps work correctly.
        */
        [Fact]
        public async Task Full_Email_Sign_Up_Success()
        {
            // Arrange
            StartSignUpEmailVerificationRequest startSignUpRequest = new StartSignUpEmailVerificationRequest()
            {
                Email = "test@test.com"
            };

            var startSignUpResponse = await _client.PostAsJsonAsync("/api/v1/Identity/sign-up-start-email-verification", startSignUpRequest);

            // Assert
            startSignUpResponse.EnsureSuccessStatusCode();

            // Arrange
            SignUpVerifyEmailRequest verifyEmailRequest = new SignUpVerifyEmailRequest()
            {
                Email = "test@test.com",
                Code="123456"
            };

            var verifyEmailResponse = await _client.PostAsJsonAsync("/api/v1/Identity/sign-up-verify-email", verifyEmailRequest);

            // Assert
            verifyEmailResponse.EnsureSuccessStatusCode();
            var verifyEmailresult = await verifyEmailResponse.Content.ReadFromJsonAsync<bool>();
            Assert.True(verifyEmailresult);

            // Arrange
            SignUpByEmailRequest signUpRequest = new SignUpByEmailRequest()
            {
                Email = "test@test.com",
                UserName = "test",
                Password = "123456%$234Tw45"
            };

            var SignUpByEmailResponse = await _client.PostAsJsonAsync("/api/v1/Identity/sign-up-by-email", signUpRequest);

            // Assert
            SignUpByEmailResponse.EnsureSuccessStatusCode();
            var signUpResponse = await SignUpByEmailResponse.Content.ReadFromJsonAsync<TokenDTO>();
            Assert.NotNull(signUpResponse);
            Assert.NotEmpty(signUpResponse.Token);
        }

        /**
         * Tests Start Email Sign up
         * Ensures that check for existing email works correctly.
        */
        [Fact]
        public async Task Start_Email_Sign_Up_Failure_Email_Exist()
        {
            // Arrange
            StartSignUpEmailVerificationRequest startSignUpRequest = new StartSignUpEmailVerificationRequest()
            {
                Email = UserSettings.Email
            };

            var startSignUpResponse = await _client.PostAsJsonAsync("/api/v1/Identity/sign-up-start-email-verification", startSignUpRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, startSignUpResponse.StatusCode);
        }

        /**
         * Tests the Full email Sign Up.
         * Ensures that code verification works correct.
        */
        [Fact]
        public async Task Full_Email_Sign_Up_Failure_Incorrect_Code()
        {
            // Arrange
            StartSignUpEmailVerificationRequest startSignUpRequest = new StartSignUpEmailVerificationRequest()
            {
                Email = "test3@test.com"
            };

            var startSignUpResponse = await _client.PostAsJsonAsync("/api/v1/Identity/sign-up-start-email-verification", startSignUpRequest);

            // Assert
            startSignUpResponse.EnsureSuccessStatusCode();

            // Arrange
            SignUpVerifyEmailRequest verifyEmailRequest = new SignUpVerifyEmailRequest()
            {
                Email = "test3@test.com",
                Code = "613451"
            };

            var verifyEmailResponse = await _client.PostAsJsonAsync("/api/v1/Identity/sign-up-verify-email", verifyEmailRequest);

            // Assert
            verifyEmailResponse.EnsureSuccessStatusCode();
            var verifyEmailresult = await verifyEmailResponse.Content.ReadFromJsonAsync<bool>();
            Assert.False(verifyEmailresult);
        }
        #endregion

        #region log-in
        [Fact]
        public async Task Log_In_Correct_Email_Correct_Password()
        {
            // Arrange
            LogInRequest logInRequest = new LogInRequest()
            {
                Email = UserSettings.Email,
                Password = UserSettings.Password
            };

            var logInResponse = await _client.PostAsJsonAsync("/api/v1/Identity/log-in", logInRequest);

            // Assert
            logInResponse.EnsureSuccessStatusCode();
            var tokenResponse = await logInResponse.Content.ReadFromJsonAsync<TokenDTO>();
            Assert.NotNull(tokenResponse);
            Assert.NotEmpty(tokenResponse.Token);
        }
        [Fact]
        public async Task Log_In_Correct_Email_Incorrect_Password()
        {
            // Arrange
            LogInRequest logInRequest = new LogInRequest()
            {
                Email = UserSettings.Email,
                Password = "125214123"
            };

            var logInResponse = await _client.PostAsJsonAsync("/api/v1/Identity/log-in", logInRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, logInResponse.StatusCode);
        }
        [Fact]
        public async Task Log_In_Incorrect_Email_Incorrect_Password()
        {
            // Arrange
            LogInRequest logInRequest = new LogInRequest()
            {
                Email = "test@test.com",
                Password = "125214123"
            };

            var logInResponse = await _client.PostAsJsonAsync("/api/v1/Identity/log-in", logInRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, logInResponse.StatusCode);
        }
        [Fact]
        public async Task Log_In_Correct_PhoneNumber_Correct_Password()
        {
            // Arrange
            LogInRequest logInRequest = new LogInRequest()
            {
                PhoneNumber = UserSettings.PhoneNumber,
                Password = UserSettings.Password
            };

            var logInResponse = await _client.PostAsJsonAsync("/api/v1/Identity/log-in", logInRequest);

            // Assert
            logInResponse.EnsureSuccessStatusCode();
            var tokenResponse = await logInResponse.Content.ReadFromJsonAsync<TokenDTO>();
            Assert.NotNull(tokenResponse);
            Assert.NotEmpty(tokenResponse.Token);
        }
        [Fact]
        public async Task Log_In_Correct_PhoneNumber_Incorrect_Password()
        {
            // Arrange
            LogInRequest logInRequest = new LogInRequest()
            {
                PhoneNumber = UserSettings.PhoneNumber,
                Password = "125214123"
            };

            var logInResponse = await _client.PostAsJsonAsync("/api/v1/Identity/log-in", logInRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, logInResponse.StatusCode);
        }
        [Fact]
        public async Task Log_In_Incorrect_PhoneNumber_Incorrect_Password()
        {
            // Arrange
            LogInRequest logInRequest = new LogInRequest()
            {
                PhoneNumber = "+0987654321",
                Password = "125214123"
            };

            var logInResponse = await _client.PostAsJsonAsync("/api/v1/Identity/log-in", logInRequest);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, logInResponse.StatusCode);
        }
        #endregion
    }
}
