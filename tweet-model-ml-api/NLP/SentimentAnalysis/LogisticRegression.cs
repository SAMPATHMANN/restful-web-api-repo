using Newtonsoft.Json;
namespace NLP.SentimentAnalysis
{
    public static class LogisticRegression
    {
        public class VocabularyFeatureVector
        {
            public double Bias { get; set; }
            public double PositiveFrequenciesSum { get; set; }
            public double NegativeFrequenciesSum { get; set; }
        }
        public class VocabularyFrequency
        {
            public string Word { get; set; }
            public bool PositiveOrNegative { get; set; }
            public int Count { get; set; }
        }

        //train Theta
        public static double[] Theta;

        public static double Alpha = 1e-9;


        public static List<VocabularyFrequency> VocabularyFrequencies { get; set; } = new List<VocabularyFrequency>();

        public static List<VocabularyFrequency> BuildFrequency(string? positiveStrings, string? negativeStrings)
        {
            var totalPositiveWords = positiveStrings?.Process_Tokenize();
            var totalNegativeWords = negativeStrings?.Process_Tokenize();

            var positiveVocabulary = GetVocabularyDict(positiveStrings)?
                .Select(x =>
                {
                    return new VocabularyFrequency
                    {
                        Word = x.Key,
                        PositiveOrNegative = true,
                        Count = (totalPositiveWords != null ? totalPositiveWords.Where(c => !string.IsNullOrEmpty(c) && c == x.Key).Count() : 0)
                    };
                });
            var negativeVocabulary = GetVocabularyDict(negativeStrings)?
                .Select(x =>
                {
                    return new VocabularyFrequency
                    {
                        Word = x.Key,
                        PositiveOrNegative = false,
                        Count = (totalNegativeWords != null ? totalNegativeWords.Where(c => !string.IsNullOrEmpty(c) && c == x.Key).Count() : 0)
                    };
                });

            //Concate 
            var vocabularyFreq = new List<VocabularyFrequency>();
            vocabularyFreq.AddRange(positiveVocabulary);
            vocabularyFreq.AddRange(negativeVocabulary);

            Console.WriteLine(vocabularyFreq);
            return vocabularyFreq;
        }

        public static string[]? GetStringWords(this string? inputString)
        {
            var retVal = inputString?.Process_Tokenize()
            .Distinct()?
            .Select(x => x?.ToLower());
            return retVal?.ToArray();
        }
        
        public static Dictionary<string, int> GetVocabularyDict(string? inputString)
        {
            //var inputStringArr = inputString?.Split("\r\n");

            ////convert all to lower for comparing actual words.
            //var concatinatedInputStrings = string.Join(" ", inputStringArr)?.ToLower();

            var vocabularyCountDic = new Dictionary<string, int>();
            var allWordsInInputString = inputString.GetStringWords()?
            .Select(x =>
            {
                if (!string.IsNullOrEmpty(x))
                {
                    if (vocabularyCountDic.ContainsKey(x))
                    {
                        vocabularyCountDic[x] = vocabularyCountDic[x] + 1;
                    }
                    else
                    {
                        vocabularyCountDic.Add(x, 1);
                    }
                }
                return x;
            }).ToArray();
            return vocabularyCountDic;
        }
        
        public static List<VocabularyFrequency> GetVocabularyFrequencies(string positiveTweetsFilePath, string negativeTweetsFilePath)
        {
            var positiveStringsFileRead = File.ReadAllText(positiveTweetsFilePath);
            var negativeStringsFileRead = File.ReadAllText(negativeTweetsFilePath);

            var totalPositiveStringArr = JsonConvert.DeserializeObject<string[]>(positiveStringsFileRead);
            var totalNegativeStringArr = JsonConvert.DeserializeObject<string[]>(negativeStringsFileRead);

            var positiveStrings = string.Join("\n", totalPositiveStringArr);
            var negativeStrings = string.Join("\n", totalNegativeStringArr);

            var vocabularyFreq = BuildFrequency(positiveStrings, negativeStrings);

            return vocabularyFreq;
        }
                
        public static VocabularyFeatureVector GetVocabularyFeatureVector(string[]? inputTextWords, List<VocabularyFrequency> vocabularyFrequencyList)
        {
            var positiveFeatures =
            (from vocFrqLst in vocabularyFrequencyList.Where(x => x.PositiveOrNegative == true)
             join inTxtWrd in inputTextWords on vocFrqLst.Word equals inTxtWrd
             select new
             {
                 inTxtWrd,
                 vocFrqLst.Count,
             }).ToList();
            var negativeFeatures =
            (from vocFrqLst in vocabularyFrequencyList.Where(x => x.PositiveOrNegative == false)
             join inTxtWrd in inputTextWords on vocFrqLst.Word equals inTxtWrd
             select new
             {
                 inTxtWrd,
                 vocFrqLst.Count,
             }).ToList();

            var featureVectorExtraction = new VocabularyFeatureVector
            {
                Bias = 1,
                PositiveFrequenciesSum = positiveFeatures.Sum(x => x.Count),
                NegativeFrequenciesSum = negativeFeatures.Sum(x => x.Count)
            };
            return featureVectorExtraction;
        }
               
        public static float Sigmoid(double value)
        {
            return 1.0f / (1.0f + (float)Math.Exp(-value));
        }

        public static double[,] FeatureVectorMatrix(VocabularyFeatureVector featureVector)
        {
            var featureVectorProperties = featureVector.GetType().GetProperties();
            var featureVectorMatrix = new double[1, featureVectorProperties.Count()];
            for (int j = 0; j < featureVectorProperties.Count(); j++)
            {
                featureVectorMatrix[0, j] = ((double)featureVectorProperties[j].GetValue(featureVector, null));
            }

            return featureVectorMatrix;
        }
        
        public static double MatrixDotProduct(double[,] train_data_feature_vector_Matrices)
        {
            double dotProduct = new double();
            var featureVector = train_data_feature_vector_Matrices;
            for (int j = 0; j < Theta.Length; j++)
            {
                dotProduct = dotProduct + (featureVector[0, j] * LogisticRegression.Theta[j]);
            }

            return dotProduct;
        }

        public static int PredictOutputValue(double y)
        {
            var isGreatedThanPoint5 = y >= 0.5;
            var y_Pred = isGreatedThanPoint5 ? 1 : 0;
            return y_Pred;
        }

        //    public static object GradientDescent(List<float[]> train_data_feature_vectors
        //        , List<float[]> thetha
        //        , double alpha
        //        , int num_Iters)
        //    {
        //        var m = train_data_feature_vectors.Count;

        //        for (var i = 0; i < num_Iters; i++) {

        //            // get z, the dot product of x and theta
        //            var z = Vector.Dot(train_data_feature_vectors, thetha);
        //        }



        //    # get the sigmoid of z
        //    h = sigmoid(z)

        //    # calculate the cost function
        //    J = (-1 / m) * (np.dot(y.T, np.log(h)) + np.dot((1 - y).T, np.log(1 - h)))

        //    # update the weights theta
        //    theta = theta - (alpha / m) * np.dot(x.T, h - y)


        //J = float(J)
        //return J, theta
        //    }
    }
}
