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
    /// <summary>
    /// Provides services for generating embeddings for text and images.
    /// </summary>
    public class EmbeddingsService : IEmbeddingsService
    {
        private readonly IOpenAIService _openAIService;
        private readonly IMLNetService _mlNetService;

        /// <summary>
        /// Initializes a new instance of the EmbeddingsService class.
        /// </summary>
        /// <param name="openAIService">The service for generating text embeddings.</param>
        /// <param name="mlNetService">The service for generating image embeddings.</param>
        public EmbeddingsService(IOpenAIService openAIService,
            IMLNetService mlNetService)
        {
            _openAIService = openAIService;
            _mlNetService = mlNetService;
        }

        /// <summary>
        /// Generates embeddings for a given text using OpenAI's embedding model.
        /// </summary>
        /// <param name="text">The text to generate embeddings for.</param>
        /// <returns>A task representing the asynchronous operation, returning the text embeddings as an array of doubles.</returns>
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
        /// <summary>
        /// Generates embeddings for a given image using a machine learning model.
        /// </summary>
        /// <param name="image">The image to generate embeddings for.</param>
        /// <returns>A task representing the asynchronous operation, returning the image embeddings as an array of floats.</returns>
        public async Task<float[]> GetImageEmbeddings(MyImage image)
        {
            float[] imageData = await PreprocessImage(image.FullPath);

            var input = new ImageClassificationInput { Data = imageData };
            return _mlNetService.Predict(input);
        }

        /// <summary>
        /// Preprocesses an image for embedding generation by resizing and normalizing pixel values.
        /// </summary>
        /// <param name="imageUrl">The URL of the image to preprocess.</param>
        /// <returns>A task representing the asynchronous operation, returning the preprocessed image data as an array of floats.</returns>
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
