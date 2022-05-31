using MySql.Data.MySqlClient;
using Conect.data;

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
                    idioma_selecionado_destino = Convert.ToString(reader["idioma_selecionado_destino"]),
                    ChatId = Convert.ToString(reader["chat_id"]),
                    admin = Convert.ToString(reader["admin"])
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
                OrigemDestino = new string[3];

                OrigemDestino[0] = check[0].idioma_selecionado_origem;
                OrigemDestino[1] = check[0].idioma_selecionado_destino;
                OrigemDestino[2] = check[0].admin;
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

        public async Task<string[]> GetUsersAsync()
        {
            var check = ObterInfo("SELECT * FROM usuarios");
            //salvar dados em array
            string[] AllUser = null;

            if (check.Count > 0)
            {
                //obter o tamanho do check
                AllUser = new string[check.Count];

                for (int i = 0; i < check.Count; i++)
                {
                    AllUser[i] = check[i].ChatId;
                }
            }

            return AllUser;
        }

    }
}
