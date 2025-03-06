using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTAA_Backend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Extensions
{
    internal static class Extensions
    {
        // Taken from the tutorial to new verstion of Aspire and Aspire.Testing
        public static async Task EnsureDbCreated(this IHost app)
        {
            using var serviceScope = app.Services.CreateScope();
            var db = serviceScope.ServiceProvider.GetRequiredService<MTAA_BackendDbContext>();
            await db.Database.EnsureCreatedAsync();
        }
    }
}
