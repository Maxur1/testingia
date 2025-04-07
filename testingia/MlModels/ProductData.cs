using Microsoft.ML.Data;

namespace testingia.MlModels
{
    public class ProductData
    {
        [LoadColumn(0)] public string SegmentCode { get; set; }
        [LoadColumn(1)] public string SegmentName { get; set; }
        [LoadColumn(2)] public string FamilyCode { get; set; }
        [LoadColumn(3)] public string FamilyName { get; set; }
        [LoadColumn(4)] public string ClassCode { get; set; }
        [LoadColumn(5)] public string ClassName { get; set; }
        [LoadColumn(6)] public string ProductCode { get; set; }
        [LoadColumn(7)] public string ProductName { get; set; }
    }

    public class ProductPrediction
    {
        [ColumnName("PredictedLabel")] public string PredictedLabel { get; set; }
    }
}

