using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces
{
    public interface IEmailService
    {
        public Task SendSighUpVerificationEmail(string email, string code);
        public Task SendEmail(string subject, string body, string toUsername, string toEmail);
    }
}
