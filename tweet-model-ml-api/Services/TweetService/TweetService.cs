using NLP.SentimentAnalysis;

namespace tweet_model_ml_api.Services.TweetService
{
    public class TweetService : ITweetService
    {
        private readonly ILogger<TweetService> _logger;

        public TweetService(ILogger<TweetService> logger)
        {
            _logger = logger;
        }

        public Task<string> MessageAsync(string message)
        {
            var response = string.Empty;
            var featureVectors = new List<LogisticRegression.VocabularyFeatureVector>();
            var messageSplitByLines = message.Split("\\n");
            //This Theta is just for now, this will be set as part of training data.
            LogisticRegression.Theta = new double[3] { 6.03518871e-08, 5.38184972e-04, -5.58300168e-04 };
            for (int i = 0; i < messageSplitByLines.Length; i++)
            {
                var inputTextWords = messageSplitByLines[i].GetStringWords();
                var featureVector = LogisticRegression.GetVocabularyFeatureVector(inputTextWords, LogisticRegression.VocabularyFrequencies);
                double[,] featureVectorMatrix = LogisticRegression.FeatureVectorMatrix(featureVector);
                double dotProduct = LogisticRegression.MatrixDotProduct(featureVectorMatrix);
                var y_Hat = LogisticRegression.Sigmoid(dotProduct);
                int y_Pred = LogisticRegression.PredictOutputValue(y_Hat);
                
                response = $"{response}\n{messageSplitByLines[i]} is {(y_Pred>0? "a good tweet!":"Not a good tweet")}";

            }
            return Task.Run(() => { return response; });
        }
    }
}
