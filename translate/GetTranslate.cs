namespace translate.Translate
{
    public class GetTranslate
    {
        protected async Task<string> GetTranslateAsync(string texto, string IdiomaOrigem, string IdiomaDestino)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://libretranslate.de/translate"),
                Headers =
    {
        { "user-agent", "vscode-restclient" },
        { "accept", "application/json" },
    },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "q", texto },
        { "source", IdiomaOrigem },
        { "target", IdiomaDestino },
        { "format", "text" }
    }),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }

        protected async Task<string> DetectedLanguageAsync(string texto)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://libretranslate.de/detect"),
                Headers =
    {
        { "user-agent", "vscode-restclient" },
        { "accept", "application/json" },
    },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "q", texto }
    }),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                return body;
            }
        }

        protected async Task<string> GetAllAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://libretranslate.de/languages"),
                Headers =
    {
        { "user-agent", "vscode-restclient" },
        { "accept", "application/json" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                return body;
            }
        }

    }
}
