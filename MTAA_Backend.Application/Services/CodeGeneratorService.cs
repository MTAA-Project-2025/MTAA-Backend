using Microsoft.EntityFrameworkCore.Design;
using MTAA_Backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Application.Services
{
    public class CodeGeneratorService : ICodeGeneratorService
    {
        private static Random _random = new Random();
        public string Generate6DigitCode()
        {
            int code = _random.Next(100000, 999999);
            return code.ToString();
        }
    }
}
