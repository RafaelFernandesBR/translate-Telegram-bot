using Serilog;

namespace Translate.Google;
public class GetTranslateGoogle : IGetTranslateGoogle
{
    private string url { get; set; }
    private readonly ILogger _logger;

    public GetTranslateGoogle(ILogger logger)
    {
        this.url = "https://clients5.google.com/";
        _logger = logger;
    }

    public virtual async Task<string?> GetTranslateAsync(string texto, string IdiomaOrigem, string IdiomaDestino)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{url}translate_a/t?client=dict-chrome-ex&sl={IdiomaOrigem}&tl={IdiomaDestino}&q={texto}");
        request.Method = HttpMethod.Get;

        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, como Gecko) Chrome/88.0.4324.104 Safari/537.36");

        try
        {
            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Erro: {ex.Message}");
            return null;
        }
    }
}
