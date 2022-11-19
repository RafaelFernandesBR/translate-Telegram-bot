using Translate.Google;

namespace translate.Balancer
{
    public class Balancer
    {

        public async Task<string> TranslateTextAsync(string texto, string IdiomaOrigem, string IdiomaDestino)
        {
            string Tradusido = null;

            var translate = new TranslateGoogle();
            Tradusido = await translate.TranslateTextAsync(texto, IdiomaOrigem, IdiomaDestino);

            return Tradusido;
        }

    }
}
