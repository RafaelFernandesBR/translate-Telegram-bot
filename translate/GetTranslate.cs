namespace translate.Translate
{
    public class GetTranslate
    {
        public string url { get; set; }

        public GetTranslate()
        {
            this.url = "https://libretranslate.de";
        }

        protected async Task<string> GetTranslateAsync(string texto, string IdiomaOrigem, string IdiomaDestino)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{url}/translate"),
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
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    return body;
                }
                else
                {
                    return "erro";
                }
            }
        }

        protected async Task<string> DetectedLanguageAsync(string texto)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{url}/detect"),
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
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    return body;
                }
                else
                {
                    return "erro";
                }
            }
        }

        protected async Task<string> GetAllAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{url}/languages"),
                Headers =
    {
        { "user-agent", "vscode-restclient" },
        { "accept", "application/json" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    return body;
                }
                else
                {
                    return "erro";
                }
            }
        }

    }
}
