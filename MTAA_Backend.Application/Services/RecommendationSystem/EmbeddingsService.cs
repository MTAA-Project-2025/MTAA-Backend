using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels;
using MTAA_Backend.Domain.Entities.Images;
using SixLabors.ImageSharp;
using MTAA_Backend.Domain.Resources.Posts.Embeddings;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Microsoft.ML;
using MTAA_Backend.Application.Utils;
using Microsoft.ML.Transforms.Onnx;
using Org.BouncyCastle.Security;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;

namespace MTAA_Backend.Application.Services.RecommendationSystem
{
    public class EmbeddingsService : IEmbeddingsService
    {
        private readonly IOpenAIService _openAIService;
        private readonly IMLNetService _mlNetService;
        public EmbeddingsService(IOpenAIService openAIService,
            IMLNetService mlNetService)
        {
            _openAIService = openAIService;
            _mlNetService = mlNetService;
        }

        public async Task<double[]> GetTextEmbeddings(string text)
        {
            var res = await _openAIService.Embeddings.CreateEmbedding(new EmbeddingCreateRequest()
            {
                EncodingFormat = "float",
                Model = Models.TextEmbeddingV3Small,
                Input = text
            });
            return res.Data[0].Embedding.ToArray();
        }

        //Taken from official documentation and modified
        public async Task<float[]> GetImageEmbeddings(MyImage image)
        {
            float[] imageData = await PreprocessImage(image.FullPath);

            var input = new ImageClassificationInput { Data = imageData };
            return _mlNetService.Predict(input);
        }

        private async Task<float[]> PreprocessImage(string imageUrl)
        {
            using var httpClient = new HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

            using var image = Image.Load<Rgb24>(imageBytes);
            image.Mutate(x => x.Resize(ImageEmbeddingModelSizes.WIDTH, ImageEmbeddingModelSizes.HEIGHT));

            float[] imageData = new float[3 * ImageEmbeddingModelSizes.WIDTH * ImageEmbeddingModelSizes.HEIGHT];
            int rIndex = 0;
            int gIndex = ImageEmbeddingModelSizes.WIDTH * ImageEmbeddingModelSizes.HEIGHT;
            int bIndex = 2 * ImageEmbeddingModelSizes.WIDTH * ImageEmbeddingModelSizes.HEIGHT;

            for (int y = 0; y < ImageEmbeddingModelSizes.HEIGHT; y++)
            {
                for (int x = 0; x < ImageEmbeddingModelSizes.WIDTH; x++)
                {
                    var pixel = image[x, y];

                    imageData[rIndex++] = (pixel.R / 255.0f - 0.485f) / 0.229f;
                    imageData[gIndex++] = (pixel.G / 255.0f - 0.456f) / 0.224f;
                    imageData[bIndex++] = (pixel.B / 255.0f - 0.406f) / 0.225f;
                }
            }

            return imageData;
        }
    }
}
