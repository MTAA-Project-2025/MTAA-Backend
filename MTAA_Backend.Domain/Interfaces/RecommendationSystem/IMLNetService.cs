using MTAA_Backend.Application.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.RecommendationSystem
{
    public interface IMLNetService
    {
        public float[] Predict(ImageClassificationInput input);
    }
}
