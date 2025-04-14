using Microsoft.ML.Data;

namespace MTAA_Backend.Application.Utils
{
    public class ImageClassificationOutput
    {
        [ColumnName("resnetv24_dense0_fwd")]
        public float[] Scores { get; set; }
    }
}
