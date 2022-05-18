using translate.Translate;
using Translate.Google;

namespace translate.Balancer
{
    public class Balancer
    {

        public async Task<string> TranslateTextAsync(string texto, string IdiomaOrigem, string IdiomaDestino)
        {
            string Tradusido = null;

            if (texto.Length >= 250)
            {
                var google = new TranslateGoogle();

                Tradusido = await google.TranslateTextAsync(texto, IdiomaOrigem, IdiomaDestino);
            }
            else
            {
                var Translate = new TranslateLibretranslate();
                Tradusido = await Translate.TranslateTextAsync(texto, IdiomaOrigem, IdiomaDestino);
            }

            return Tradusido;
        }


    }
}
