using Newtonsoft.Json.Linq;

namespace translate.Translate
{
    public class Translate : GetTranslate
    {

        public async Task<string> TranslateTextAsync(string texto, string IdiomaOrigem, string IdiomaDestino)
        {
            string tradusido = null;
            string tradusir = await GetTranslateAsync(texto, IdiomaOrigem, IdiomaDestino);
            if (tradusir == "erro")
            {
                tradusido = "Erro, tente novamente";
            }
            else
            {
                var obj = JObject.Parse(tradusir);

                tradusido = Convert.ToString(obj["translatedText"]);
            }
            return tradusido;
        }

        public async Task<string[]> DetectedTextAsync(string texto)
        {
            string[] Idioma = new string[2];
            string detectar = await DetectedLanguageAsync(texto);
            var obj = JArray.Parse(detectar);
            Idioma[0] = Convert.ToString(obj[0]["confidence"]);
            Idioma[1] = Convert.ToString(obj[0]["language"]);

            return Idioma;
        }

        public async Task<string> AllLanguagesAsync()
        {
            string todos = null;
            string detectar = await GetAllAsync();

            if (detectar == "erro")
            {
                todos = "Erro, tente novamente";
            }
            else
            {
                var obj = JArray.Parse(detectar);

                foreach (var item in obj)
                {
                    todos += Convert.ToString(item["name"]) + " " + Convert.ToString(item["code"]) + "\n";
                }
            }

            return todos;
        }

        public async Task<string[]> RandomAsync()
        {
            string[] todos = new string[28];
            string detectar = await GetAllAsync();

            if (detectar == "erro")
            {
                todos[0] = "Erro, tente novamente";
            }
            else
            {
                var obj = JArray.Parse(detectar);

                for (var i = 0; i <= 27; i++)
                {
                    todos[i] = Convert.ToString(obj[i]["code"]);
                }
            }

            return todos;
        }

    }
}
