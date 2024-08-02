
using Newtonsoft.Json;
using NLP.SentimentAnalysis;
using tweet_model_ml_api.Services.TweetService;

var builder = WebApplication.CreateBuilder(args);

var envCurrentDirPath = Environment.CurrentDirectory;
//Prepare NLP for Tweets
var positiveTweetsFilePath = Path.Combine(envCurrentDirPath, "Data/PositiveTweets.json");
var negativeTweetsFilePath = Path.Combine(envCurrentDirPath, "Data/NegativeTweets.json");

SentimentAnalysis_LogisticRegression_Model_Training_Context(positiveTweetsFilePath, negativeTweetsFilePath);

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped<ITweetService, TweetService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.Run();

static void SentimentAnalysis_LogisticRegression_Model_Training_Context(string positiveTweetsFilePath, string negativeTweetsFilePath)
{
    var positiveStringsFileRead = File.ReadAllText(positiveTweetsFilePath);
    var negativeStringsFileRead = File.ReadAllText(negativeTweetsFilePath);

    var totalPositiveStringArr = JsonConvert.DeserializeObject<string[]>(positiveStringsFileRead);
    var totalNegativeStringArr = JsonConvert.DeserializeObject<string[]>(negativeStringsFileRead);

    //Build Frequency for Input strings
    LogisticRegression.VocabularyFrequencies = LogisticRegression.BuildFrequency(string.Join("\n", totalPositiveStringArr), string.Join("\n", totalNegativeStringArr));

    //Split Training and Testing Data -> here dividing by half
    var train_data = totalPositiveStringArr.Skip(0).Take((totalPositiveStringArr.Length / 2) - 1)
        .Concat(totalNegativeStringArr.Skip(0).Take((totalNegativeStringArr.Length / 2) - 1))
        .ToArray();
    var test_data = totalPositiveStringArr.Skip(totalPositiveStringArr.Length / 2).Take((totalPositiveStringArr.Length) - 1)
        .Concat(totalNegativeStringArr.Skip(totalNegativeStringArr.Length / 2).Take((totalNegativeStringArr.Length) - 1))
        .ToArray();

    //Get Training vectors.
    var train_data_feature_vector_Matrices = new List<double[,]>();
    for (int i = 0; i < train_data.Length; i++)
    {
        var inputTextWords = train_data[i].GetStringWords();
        var featureVector = LogisticRegression.GetVocabularyFeatureVector(inputTextWords, LogisticRegression.VocabularyFrequencies);
        double[,] featureVectorMatrix = LogisticRegression.FeatureVectorMatrix(featureVector);
        train_data_feature_vector_Matrices.Add(featureVectorMatrix);
    }

    var sampleFeatureVector = LogisticRegression.GetVocabularyFeatureVector([], LogisticRegression.VocabularyFrequencies);
    var sampleFeatureVectorPropertiesCount = sampleFeatureVector.GetType().GetProperties().Count();
    //thetha is sampleFeatureVectorProperties.Count()X1 matrix


    //for now just setting as zeros for training.
    LogisticRegression.Theta = new double[3] { 0, 0, 0 };
    
    for(int i=0; i< train_data_feature_vector_Matrices.Count; i++)
    {
        double dotProduct = LogisticRegression.MatrixDotProduct(train_data_feature_vector_Matrices[i]);
        var h = LogisticRegression.Sigmoid(dotProduct);
        //LogisticRegression.Theta = LogisticRegression.Theta - (LogisticRegression.Alpha / m) * np.dot(x.T, h - y)
    }

    

    var y_Hat_List = new List<double>();
    for (int i = 0; i < train_data_feature_vector_Matrices.Count; i++)
    {
        double dotProduct = LogisticRegression.MatrixDotProduct(train_data_feature_vector_Matrices[i]);
        var y_Hat = LogisticRegression.Sigmoid(dotProduct);

        y_Hat_List.Add(y_Hat);
    }



    var y_Pred_List = new List<double>();
    for (int i = 0; i < y_Hat_List.Count; i++)
    {
        var y = y_Hat_List[i];
        int y_Pred = LogisticRegression.PredictOutputValue(y);
        y_Pred_List.Add(y_Pred);
    }

    Console.WriteLine(y_Pred_List);
}