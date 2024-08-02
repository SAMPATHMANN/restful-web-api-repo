namespace tweet_model_ml_api.Services.TweetService
{
    public interface ITweetService
    {
        Task<string> MessageAsync(string message);
    }
}
