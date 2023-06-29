namespace FitCookieAI.RestComunication.GPT
{
    public class GPT_RequestBuilder
    {
        private GPT_RequestRouter _router = new GPT_RequestRouter();

        public string PostGPTINputRequestBuilder(string uri, string input)
        {
            return uri + _router.GPT_RquestRoute + $"input={input}";
        }
    }
}
