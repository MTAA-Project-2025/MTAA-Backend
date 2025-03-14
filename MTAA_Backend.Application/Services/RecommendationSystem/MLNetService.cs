using Microsoft.ML.Transforms.Onnx;
using Microsoft.ML;
using MTAA_Backend.Application.Utils;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;

namespace MTAA_Backend.Application.Services.RecommendationSystem
{
    public class MLNetService : IMLNetService
    {
        public const string ModelPath = "resnet50-v2-7.onnx";
        private readonly PredictionEngine<ImageClassificationInput, ImageClassificationOutput> predictionEngine;
        public MLNetService()
        {
            var mlContext = new MLContext();
            var pipeline = mlContext.Transforms.ApplyOnnxModel(
                modelFile: ModelPath,
                inputColumnNames: new[] { "data" },
                outputColumnNames: new[] { "resnetv24_dense0_fwd" });

            OnnxTransformer model = pipeline.Fit(mlContext.Data.LoadFromEnumerable(new List<ImageClassificationInput>()));
            predictionEngine = mlContext.Model.CreatePredictionEngine<ImageClassificationInput, ImageClassificationOutput>(model);
        }

        public float[] Predict(ImageClassificationInput input)
        {
            return predictionEngine.Predict(input).Scores;
        }
    }
}
