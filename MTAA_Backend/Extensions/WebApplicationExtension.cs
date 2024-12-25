using Microsoft.AspNetCore.Localization;
using MTAA_Backend.Domain.Resources.Localization;
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
    }
}
