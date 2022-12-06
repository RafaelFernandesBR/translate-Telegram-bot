using Conect.data;
using MySql.Data.MySqlClient;
using System.Data;
using Telegram.Bot.Types;
using Dapper;
using System.Linq;

namespace Data.Conect
{
    public class DatabaseConect
    {
        private MySqlConnection conm { get; set; }

        public string ChatId { get; set; }
        public string idioma_selecionado_origem { get; set; }
        public string idioma_selecionado_destino { get; set; }
        public string admin { get; set; }

        public DatabaseConect()
        {
            Getdata Getdata = new Getdata();
            var dadosConect = Getdata.GetdataAll();

            this.conm = new MySqlConnection($"Server={dadosConect.mysql.Server};Database={dadosConect.mysql.Database};Uid={dadosConect.mysql.user};Pwd={dadosConect.mysql.senha};SSL Mode=None");
        }

        private IEnumerable<DatabaseConect> ObterInfo(string query)
        {
            using (IDbConnection connectiontst = conm)
            {
                var cost = connectiontst.Query<DatabaseConect>(query);
                return cost;
            }
        }

        private void DeletInsert(string query)
        {
            using (IDbConnection connectiontst = conm)
            {
                connectiontst.QueryAsync(query);
            }
        }

        public string[] Verificar(string chatId)
        {
            var check = ObterInfo("SELECT * FROM usuarios WHERE chat_id = " + chatId);
            //salvar idioma origem e destino em array
            string[] OrigemDestino = null;

            if (check != null)
            {
                foreach (var d in check)
                {
                    OrigemDestino = new string[3];

                    OrigemDestino[0] = d.idioma_selecionado_origem;
                    OrigemDestino[1] = d.idioma_selecionado_destino;
                    OrigemDestino[2] = d.admin;
                }
            }

            return OrigemDestino;
        }

        public void Salvar(string chatId)
        {
            var check = ObterInfo($"SELECT * FROM usuarios WHERE chat_id = '{chatId}';");

            if (check.Count() == 0)
            {
                DeletInsert($"INSERT INTO usuarios(chat_id) VALUES('{chatId}');");
            }

        }

        public string[] Atualizar(string chatId, string idiomaOrigem, string idiomaDestino)
        {
            ObterInfo($"UPDATE usuarios SET idioma_selecionado_origem = '{idiomaOrigem}', idioma_selecionado_destino ='{idiomaDestino}' WHERE chat_id = '{chatId}';");

            var check = ObterInfo("SELECT * FROM usuarios WHERE chat_id = " + chatId);
            //salvar idioma origem e destino em array
            string[] NovoOrigemDestino = null;

            if (check.Count() != 0)
            {
                foreach (var d in check)
                {
                    NovoOrigemDestino = new string[2];

                    NovoOrigemDestino[0] = d.idioma_selecionado_origem;
                    NovoOrigemDestino[1] = d.idioma_selecionado_destino;
                }
            }

            return NovoOrigemDestino;
        }

        public async Task<string[]> GetUsersAsync()
        {
            var check = ObterInfo("SELECT * FROM usuarios");
            //salvar dados em array
            string[] AllUser = null;

            if (check != null)
            {
                List<string> intermediate_list = new List<string>();

                foreach (var d in check)
                {
                    intermediate_list.Add(d.ChatId);
                }

                AllUser = intermediate_list.ToArray();
            }
            return AllUser;
        }

    }
}
