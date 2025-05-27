using Microsoft.ML.Transforms.Onnx;
using Microsoft.ML;
using MTAA_Backend.Application.Utils;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;

namespace MTAA_Backend.Application.Services.RecommendationSystem
{
    /// <summary>
    /// Provides services for generating predictions using an ONNX machine learning model.
    /// </summary>
    public class MLNetService : IMLNetService
    {
        /// <summary>
        /// The path to the ONNX model file.
        /// </summary>
        public const string ModelPath = "resnet50-v2-7.onnx";
        private readonly PredictionEngine<ImageClassificationInput, ImageClassificationOutput> predictionEngine;

        /// <summary>
        /// Initializes a new instance of the MLNetService class.
        /// </summary>
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

        /// <summary>
        /// Generates predictions for the given input using the ONNX model.
        /// </summary>
        /// <param name="input">The input data for prediction.</param>
        /// <returns>An array of prediction scores.</returns>
        public float[] Predict(ImageClassificationInput input)
        {
            return predictionEngine.Predict(input).Scores;
        }
    }
}
