using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.RecommendationSystem
{
    public interface IIllegalClassificationService
    {
        public float Predict(float[] features);
    }
}
