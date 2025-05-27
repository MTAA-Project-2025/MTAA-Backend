using Microsoft.EntityFrameworkCore.Design;
using MTAA_Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Services
{
    /// <summary>
    /// Provides services for generating random codes.
    /// </summary>
    public class CodeGeneratorService : ICodeGeneratorService
    {
        private static Random _random = new Random();

        /// <summary>
        /// Generates a random 6-digit code.
        /// </summary>
        /// <returns>A string representing a 6-digit code.</returns>
        public string Generate6DigitCode()
        {
            int code = _random.Next(100000, 999999);
            return code.ToString();
        }
    }
}
