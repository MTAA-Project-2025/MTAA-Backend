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
    public class LanguageService : ILanguageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LanguageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
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
