using Conect.data;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;
using Serilog;

namespace Data.Conect;
public class DatabaseConect
{
    private MySqlConnection conm { get; set; }
    private readonly ILogger _logger;

    public DatabaseConect(ILogger logger)
    {
        Getdata Getdata = new Getdata();
        var dadosConect = Getdata.GetdataAll();

        this.conm = new MySqlConnection($"Server={dadosConect.mysql.Server};Database={dadosConect.mysql.Database};Uid={dadosConect.mysql.user};Pwd={dadosConect.mysql.senha};SSL Mode=None");
        _logger = logger;
    }

    private IEnumerable<Users> ObterInfo(string query)
    {
        try
        {
            using (IDbConnection connectiontst = conm)
            {
                var cost = connectiontst.Query<Users>(query);
                return cost;
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Erro: {ex.Message}");
            return Enumerable.Empty<Users>();
        }
    }

    private void DeletInsert(string query)
    {
        try
        {
            using (IDbConnection connectiontst = conm)
            {
                connectiontst.QueryAsync(query);
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Erro: {ex.Message}");
        }
    }

    public async Task<IEnumerable<Users>> Verificar(string chatId)
    {
        string query = "SELECT * FROM usuarios WHERE chat_id = " + chatId;
        try
        {
            using (IDbConnection connectiontst = conm)
            {
                var result = await connectiontst.QueryAsync<Users>(query);
                return result;
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Erro: {ex.Message}");
            return Enumerable.Empty<Users>();
        }
    }

    public void Salvar(string chatId)
    {
        var check = ObterInfo($"SELECT * FROM usuarios WHERE chat_id = '{chatId}';");

        if (check.Count() == 0)
        {
            DeletInsert($"INSERT INTO usuarios(chat_id) VALUES('{chatId}');");
        }

    }

    public IEnumerable<Users> Atualizar(string chatId, string idiomaOrigem, string idiomaDestino)
    {
        DeletInsert($"UPDATE usuarios SET idioma_selecionado_origem = '{idiomaOrigem}', idioma_selecionado_destino ='{idiomaDestino}' WHERE chat_id = '{chatId}';");
        var check = ObterInfo("SELECT * FROM usuarios WHERE chat_id = " + chatId);

        if (check.Count() != 0)
        {
            return check;
        }

        return Enumerable.Empty<Users>();
    }

}
