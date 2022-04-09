using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Data.Conect
{
    public class DatabaseConect
    {
        private MySqlConnection conm { get; set; }

        public string idioma_selecionado_origem { get; set; }
        public string idioma_selecionado_destino { get; set; }

        public DatabaseConect()
        {
            //get a file json
            StreamReader r = new StreamReader("telegram.json");
            string readFile = r.ReadToEnd();
            conectSql conectData = JsonConvert.DeserializeObject<conectSql>(readFile);

            this.conm = new MySqlConnection("Server=" + conectData.server + ";Database=" + conectData.Database + ";Uid=" + conectData.user + ";Pwd=" + conectData.senha + ";SSL Mode=None");
        }

        private List<DatabaseConect> ObterInfo(string query)
        {
            List<DatabaseConect> data = new List<DatabaseConect>();

            this.conm.Open();
            MySqlCommand cmd = new MySqlCommand(query, this.conm);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                data.Add(new DatabaseConect
                {
                    //get a data em mysql
                    idioma_selecionado_origem = Convert.ToString(reader["idioma_selecionado_origem"]),
                    idioma_selecionado_destino = Convert.ToString(reader["idioma_selecionado_destino"])
                });
            }
            reader.Close();
            this.conm.Close();
            return data;
        }

        private void DeletInsert(string query)
        {
            this.conm.Open();
            MySqlCommand cmd = new MySqlCommand(query, this.conm);
            cmd.ExecuteNonQuery();
            this.conm.Close();
        }

        public string[] Verificar(string chatId)
        {
            var check = ObterInfo("SELECT * FROM usuarios WHERE chat_id = " + chatId);
            //salvar idioma origem e destino em array
            string[] OrigemDestino = null;

            if (check.Count > 0)
            {
                OrigemDestino = new string[2];

                OrigemDestino[0] = check[0].idioma_selecionado_origem;
                OrigemDestino[1] = check[0].idioma_selecionado_destino;
            }

            return OrigemDestino;
        }

        public void Salvar(string chatId)
        {
            var check = ObterInfo($"SELECT * FROM usuarios WHERE chat_id = '{chatId}';");

            if (check.Count <= 0)
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

            if (check.Count > 0)
            {
                NovoOrigemDestino = new string[2];

                NovoOrigemDestino[0] = check[0].idioma_selecionado_origem;
                NovoOrigemDestino[1] = check[0].idioma_selecionado_destino;
            }

            return NovoOrigemDestino;
        }

    }
}
