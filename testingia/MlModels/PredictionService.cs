using Microsoft.ML;
using testingia.MlModels;

namespace testingia.Services
{
    public class PredictionService
    {
        private readonly string _modelPath = "MLModels/productModel.zip";
        private readonly MLContext _mlContext;
        private ITransformer _model;
        private PredictionEngine<ProductData, ProductPrediction> _predictionEngine;

        public PredictionService()
        {
            _mlContext = new MLContext();
            LoadModel();
        }

        private void LoadModel()
        {
            DataViewSchema schema;
            _model = _mlContext.Model.Load(_modelPath, out schema);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<ProductData, ProductPrediction>(_model);
        }

        public string Predict(string productName)
        {
            var input = new ProductData { ProductName = productName };
            var prediction = _predictionEngine.Predict(input);
            return prediction.PredictedLabel;
        }
    }
}
