using Microsoft.ML.Data;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;

namespace MTAA_Backend.Application.Utils
{
    public class ImageClassificationInput
    {
        [ColumnName("data")]
        [VectorType(3, ImageEmbeddingModelSizes.WIDTH, ImageEmbeddingModelSizes.HEIGHT)]
        public float[] Data { get; set; }
    }
}
