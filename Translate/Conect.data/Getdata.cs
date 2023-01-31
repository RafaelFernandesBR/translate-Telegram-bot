namespace Conect.data;
public class Getdata
{
    private static Getdata _instance;
    private ConectData _conectData;

    private Getdata()
    {
        _conectData = new ConectData
        {
            mysql = new Mysql
            {
                Server = Environment.GetEnvironmentVariable("MYSQLSERVER"),
                Database = Environment.GetEnvironmentVariable("MYSQLDATABASE"),
                user = Environment.GetEnvironmentVariable("MYSQLUSER"),
                senha = Environment.GetEnvironmentVariable("MYSQLPASSWORD")
            },
            telegram = new Telegram
            {
                tokem = Environment.GetEnvironmentVariable("tokem")
            }
        };
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

    public ConectData GetConectData()
    {
        return _conectData;
    }
}
