using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces
{
    public interface ICodeGeneratorService
    {
        public string Generate6DigitCode();
    }
}
