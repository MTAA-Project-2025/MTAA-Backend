using Microsoft.ML.Data;
using Microsoft.ML;
using MTAA_Backend.Application.Utils;
using MTAA_Backend.Domain.Interfaces.RecommendationSystem;

namespace MTAA_Backend.Application.Services.RecommendationSystem
{
    public class IllegalClassificationService : IIllegalClassificationService
    {
        private readonly MLContext mlContext;
        private ITransformer model;
        private PredictionEngine<ImageFeatureData, Prediction> predictionEngine;
        public IllegalClassificationService()
        {
            mlContext = new MLContext(seed: 0);

            LoadModel("adultClassifier.onnx");
        }

        public void LoadModel(string modelPath)
        {
            DataViewSchema schema;
            model = mlContext.Model.Load(modelPath, out schema);
            predictionEngine = mlContext.Model.CreatePredictionEngine<ImageFeatureData, Prediction>(model);
        }

        public float Predict(float[] features)
        {
            var result = predictionEngine.Predict(new ImageFeatureData { Features = features });
            return result.Probability;
        }

        private class Prediction
        {
            [ColumnName("Label")]
            public bool Label { get; set; }

            [ColumnName("PredictedLabel")]
            public bool PredictedLabel { get; set; }

            [ColumnName("Score")]
            public float Score { get; set; }

            [ColumnName("Probability")]
            public float Probability { get; set; }
        }
    }
    public class ImageFeatureData
    {
        [VectorType(1000)]
        [ColumnName("Features")]
        public float[] Features { get; set; }

        [ColumnName("Label")]
        public bool Label { get; set; }
    }
}
