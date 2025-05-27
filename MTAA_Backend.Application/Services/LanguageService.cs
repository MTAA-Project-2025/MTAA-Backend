using Microsoft.AspNetCore.Http;
using MTAA_Backend.Domain.Interfaces;
using MTAA_Backend.Domain.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Services
{
    /// <summary>
    /// Provides services for managing language preferences based on HTTP context.
    /// </summary>
    public class LanguageService : ILanguageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the LanguageService class.
        /// </summary>
        /// <param name="httpContextAccessor">The accessor for HTTP context information.</param>
        public LanguageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves the current language code from the HTTP request's Accept-Language header.
        /// </summary>
        /// <returns>The language code, or the default English code if the header is invalid or missing.</returns>
        public string GetCurrentLanguageCode()
        {
            try
            {
                var acceptLanguageHeader = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"];

                var code = acceptLanguageHeader.ToString();

                if (code == null || code.Length > 10) return Languages.EN;

                return code;
            }
            catch
            {
                return Languages.EN;
            }
        }
    }
}
