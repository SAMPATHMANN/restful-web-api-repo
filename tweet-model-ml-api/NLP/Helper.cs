
namespace NLP
{
    public static class Helper
    {
        public readonly static string[] StopWords = new string[181] { "i", "i'm", "me", "my", "myself", "we", "our", "ours", "ourselves", "you", "you're", "you've", "you'll", "you'd", "your", "yours", "yourself", "yourselves", "he", "him", "his", "himself", "she", "she's", "her", "hers", "herself", "it", "it's", "its", "itself", "they", "them", "their", "theirs", "themselves", "what", "which", "who", "whom", "this", "that", "that'll", "these", "those", "am", " is ", "are", "was", "were", "be", "been", "being", "have", "has", "had", "having", "do", "does", "did", "doing", "a", "an", "the", "and", "but", "if", "or", "because", " as", "until", "while", "of", "at", "by", "for", "with", "about", "against", "between", "into", "through", "during", "before", "after", "above", "below", "to", "from", "up", "down", "in", "out", "on", "off", "over", "under", "again", "further", "then", "once", "here", "there", "when", "where", "why", "how", "all", "any", "both", "each", "few", "more", "most", "other", "some", "such", "no", "nor", "not", "only", "own", "same", "so", "than", "too", "very", "s", "t", "can", "will", "just", "don", "don't", "should", "should've", "now", "d", "ll", "m", "o", "re", "ve", "y", "ain", "aren", "aren't", "couldn", "couldn't", "didn", "didn't", "doesn", "doesn't", "hadn", "hadn't", "hasn", "hasn't", "haven", "haven't", "isn", "isn't", "ma", "mightn", "mightn't", "mustn", "mustn't", "needn", "needn't", "shan", "shan't", "shouldn", "shouldn't", "wasn", "wasn't", "weren", "weren't", "won", "won't", "wouldn", "wouldn't", "is" };

        public readonly static string[] PuntuationWords = new string[35] { "!", "\"", "\'", "\r", "\n", "#", "$", "%", "&", "'", "(", ")", "*", "+", ",", "-", ".", "/", ":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "_", "`", "{", "|", "}", "~" }; 
        
    
        public static string[]? Process_Tokenize(this string inputString)
        {
            //Preprocessing of strings.

            //removing puntuation words.
            if (!string.IsNullOrEmpty(inputString)
            && PuntuationWords != null
            && PuntuationWords.Length > 0)
            {
                for (int i = 0; i < PuntuationWords.Length; i++)
                {
                    inputString = inputString.Replace(PuntuationWords[i], string.Empty);
                }
            }
            var retVal = inputString.Replace("\r", " ").Replace("\n", " ").Split(" ")
                .Select(x =>
                {
                    x = x.ToLower();
                    if (!string.IsNullOrEmpty(x)
                    && !string.IsNullOrWhiteSpace(x)
                    && !x.StartsWith("@") && !x.StartsWith("@http") //url handlers.
                    && !StopWords.Contains(x) //removing stop words.
                    && !PuntuationWords.Contains(x) //removing puntuation words.
                    )
                    {
                        if (x.EndsWith("ed"))
                        {
                            x = x.Substring(0, x.Length - 2);
                        }
                        if (x.EndsWith("ing"))
                        {
                            x = x.Substring(0, x.Length - 3);
                        }
                    }
                    else
                    {
                        return null;
                    }
                    return x.Trim();
                }).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            return retVal;
        }

    }
}
