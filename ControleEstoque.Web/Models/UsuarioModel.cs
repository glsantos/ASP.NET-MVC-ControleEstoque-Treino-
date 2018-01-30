using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ControleEstoque.Web.Models {
    public class UsuarioModel {

        public static bool ValidarUsuario(string login, string senha) {

            var retorno = false;
            using (var conexao =  new SqlConnection()) {

                var Banco = "Data Source=LENOVO-PC;Initial Catalog=ESTUDOControleEstoque;User Id=sa;Password=123456";

                conexao.ConnectionString = Banco;
                conexao.Open();

                using (var comando = new SqlCommand()) {

                    comando.Connection = conexao;
                    comando.CommandText = string.Format(
                                    "select count(*) from usuario where login='{0}' and senha = '{1}'", login, senha);

                    retorno = ((int)comando.ExecuteScalar() > 0);
                }
            }

            return retorno;
        }
    }
}