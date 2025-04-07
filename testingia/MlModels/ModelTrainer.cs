using System;
using System.Data;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using testingia.MlModels;

namespace testingia.Services
{
    public class ModelTrainer
    {
        private readonly string _modelPath = "MLModels/productModel.zip";
        private readonly string _excelPath = "Data/products.xlsx";

        public void TrainModel()
        {
            try
            {
                var mlContext = new MLContext();
                var fullExcelPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _excelPath);
                Console.WriteLine($"Loading Excel file from: {fullExcelPath}");

                DataTable dt = ExcelLoader.LoadSheet(fullExcelPath);
                Console.WriteLine($"Excel file loaded. Total rows (including header): {dt.Rows.Count}");

                var productList = dt.AsEnumerable()
                                    .Skip(1) // Skip header row
                                    .Select(row => new ProductData
                                    {
                                        SegmentCode = row[0]?.ToString(),
                                        SegmentName = row[1]?.ToString(),
                                        FamilyCode = row[2]?.ToString(),
                                        FamilyName = row[3]?.ToString(),
                                        ClassCode = row[4]?.ToString(),
                                        ClassName = row[5]?.ToString(),
                                        ProductCode = row[6]?.ToString(),
                                        ProductName = row[7]?.ToString()
                                    })
                                    .ToList();
                Console.WriteLine($"Loaded {productList.Count} product entries.");


                // Print the first few entries for inspection
                for (int i = 0; i < Math.Min(5, productList.Count); i++)
                {
                    var p = productList[i];
                    Console.WriteLine($"[{i}] Segment: {p.SegmentName}, Family: {p.FamilyName}, Class: {p.ClassName}, Product: {p.ProductName}");
                }

                var data = mlContext.Data.LoadFromEnumerable(productList);

                // Preview the first few rows from the IDataView to inspect what was loaded
                var preview = data.Preview(maxRows: 5);
                foreach (var row in preview.RowView)
                {
                    Console.WriteLine("---- Data Row ----");
                    foreach (var col in row.Values)
                    {
                        Console.WriteLine($"{col.Key}: {col.Value}");
                    }
                }

                var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(ProductData.ProductName))
                    .Append(mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(ProductData.ClassName)))
                    .Append(mlContext.MulticlassClassification.Trainers.NaiveBayes("Label", "Features"))
                    .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

                Console.WriteLine("Starting model training...");
                var model = pipeline.Fit(data);
                Console.WriteLine("Model training done.");
                var fullModelPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), _modelPath);
                Console.WriteLine($"Saving model to: {fullModelPath}");
                mlContext.Model.Save(model, data.Schema, fullModelPath);
                Console.WriteLine("Model training completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during model training: " + ex.Message);
            }
        }

    }
}
