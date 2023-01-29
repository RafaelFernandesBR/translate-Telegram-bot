namespace Translate.Google;
public class TranslateGoogle
{

    public async Task<string?> TranslateTextAsync(string texto, string IdiomaOrigem, string IdiomaDestino)
    {
        var gtranslate = new GetTranslateGoogle(LoggerConfig.CreateLogger());
        var tradusir = await gtranslate.GetTranslateAsync(texto, IdiomaOrigem, IdiomaDestino);

        if (tradusir != null)
        {
            string textFin = null;

            if (IdiomaOrigem != "auto")
            {
                textFin = tradusir.Substring(2, tradusir.Length - 4);
            }
            else
            {
                textFin = tradusir.Substring(3, tradusir.Length - 11);
            }
            //remover os \\n do texto e adicionar nova linha no lugar
            textFin = textFin.Replace("\\n", "\n");

            return textFin;
        }
        else
        {
            return null;
        }
    }

}
