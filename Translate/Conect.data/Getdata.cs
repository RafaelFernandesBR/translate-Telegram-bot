using Newtonsoft.Json;

namespace Conect.data;
public class Getdata
{
    private static Getdata _instance;
    private ConectData _conectData;

    private Getdata()
    {
        StreamReader r = new StreamReader("telegram.json");
        string readFile = r.ReadToEnd();
        _conectData = JsonConvert.DeserializeObject<ConectData>(readFile);
    }

    public static Getdata Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Getdata();
            }
            return _instance;
        }
    }

    public ConectData ConectData
    {
        get { return _conectData; }
    }
}