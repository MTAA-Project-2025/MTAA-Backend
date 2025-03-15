using MTAA_Backend.Domain.Entities.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Interfaces.RecommendationSystem
{
    public interface IEmbeddingsService
    {
        public Task<double[]> GetTextEmbeddings(string text);
        public Task<float[]> GetImageEmbeddings(MyImage image);
    }
}
