using MTAA_Backend.Domain.Resources.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTAA_Backend.Domain.Resources.Posts.Embeddings
{
    public struct VectorCollections
    {
        public const string PostTextEmbeddings = "post-text-embeddings";
        public const int PostTextEmbeddingsSize = 1536;

        public const string PostImageEmbeddings = "post-image-embeddings";
        public const int PostImageEmbeddingsSize = 1000;


        public const string UsersPostTextVectors = "users-post-text-vectors";
        public const int UsersPostTextVectorsSize = 1536;
        
        public const string UsersPostImageVectors = "users-post-image-vectors";
        public const int UsersPostImageVectorsSize = 1000;

        public static Dictionary<string, int> Sizes { get; set; } = new Dictionary<string, int>()
        {
            {
                PostTextEmbeddings,
                PostTextEmbeddingsSize
            },
            {
                PostImageEmbeddings,
                PostImageEmbeddingsSize
            },
            {
                UsersPostTextVectors,
                UsersPostTextVectorsSize
            },
            {
                UsersPostImageVectors,
                UsersPostImageVectorsSize
            },
        };
        public static List<string> GetAllNames()
        {
            return new List<string>()
            {
                PostTextEmbeddings,
                PostImageEmbeddings,
                UsersPostImageVectors,
                UsersPostImageVectors
            };
        }
        public static List<int> GetAllSizes()
        {
            return new List<int>()
            {
                PostTextEmbeddingsSize,
                PostImageEmbeddingsSize,
                UsersPostImageVectorsSize,
                UsersPostImageVectorsSize
            };
        }
    }
}
