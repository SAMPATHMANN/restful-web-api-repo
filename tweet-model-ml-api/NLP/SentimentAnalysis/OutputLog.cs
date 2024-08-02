namespace NLP.SentimentAnalysis
{
    public static class OutputLog
    {
        public static void LogVocabularyFrequencies(List<LogisticRegression.VocabularyFrequency> vocabularySentimentFrequencyList)
        {
            var emptyString = "          ";
            var retVal = string.Empty;
            //Console.WriteLine(vocabularySentimentFrequencyList);

            retVal = retVal + "\n" + $"Vocabulary Frequency\r\n";
            retVal = retVal + "\n" + $"Word {emptyString}|Positive Count {emptyString}|Negative Count {emptyString}\r\n";
            foreach (var vocabularySentimentFrequency in vocabularySentimentFrequencyList)
            {
                //retVal = retVal + "\n" + $"{vocabularySentimentFrequency.Word}{emptyString}|{vocabularySentimentFrequency.PositiveFrequency}{emptyString}|{vocabularySentimentFrequency.NegativeFrequency}{emptyString}\r\n";
            }
            Console.WriteLine(retVal);
        }

        public static string GetFeatureVectorExtractionLog(LogisticRegression.VocabularyFeatureVector featureVector)
        {
            var retVal = $"[{featureVector.Bias},{featureVector.PositiveFrequenciesSum},{featureVector.NegativeFrequenciesSum}].\r\n";
            retVal = $"{retVal}{$"{featureVector.Bias} corresponds to the bias,{featureVector.PositiveFrequenciesSum} the positive feature, and {featureVector.NegativeFrequenciesSum} the negative feature.\r\n"}";
            Console.WriteLine(retVal);
            return retVal;
        }
    }
}
