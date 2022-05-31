using Newtonsoft.Json;

namespace Conect.data
{

    public class Getdata
    {

        public Conect.data.ConectData GetdataAll()
        {
            StreamReader r = new StreamReader("telegram.json");
            string readFile = r.ReadToEnd();
            //converter json em método csharp
            var json = JsonConvert.DeserializeObject<ConectData>(readFile);
            return json;
        }

    }
}
