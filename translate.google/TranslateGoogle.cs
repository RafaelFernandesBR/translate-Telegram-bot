using Newtonsoft.Json;

namespace Translate.Google
{
    public class TranslateGoogle : GetTranslateGoogle
    {

        public async Task<string> TranslateTextAsync(string texto, string IdiomaOrigem, string IdiomaDestino)
        {
            var tradusir = await GetTranslateAsync(texto, IdiomaOrigem, IdiomaDestino);/*[["texto tradusido"]]*/
            //remover os primeiros 4 e os últimos 7 caractéres
            string textFin = tradusir.Substring(2, tradusir.Length - 4);
            //remover os \\n do texto e adicionar nova linha no lugar
            textFin = textFin.Replace("\\n", "\n");

            return textFin;
        }

    }
}
