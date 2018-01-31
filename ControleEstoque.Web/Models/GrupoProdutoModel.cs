using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Configuration;

namespace ControleEstoque.Web.Models {
    public class GrupoProdutoModel {

        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome")]
        public string Nome { get; set; }
        

        public bool Status { get; set; }

        
        public static List<GrupoProdutoModel> RecuperarLista() {

            var retorno = new List<GrupoProdutoModel>();

            using (var conexao = new SqlConnection()) {

                var Banco = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;

                conexao.ConnectionString = Banco;
                conexao.Open();

                using (var comando = new SqlCommand()) {

                    comando.Connection = conexao;
                    comando.CommandText = "select * from grupo_produto order by nome";

                    var reader = comando.ExecuteReader();
                    while (reader.Read()) {

                        retorno.Add(new GrupoProdutoModel {

                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Status = (bool)reader["status"]
                        });
                    }
                }
            }

            return retorno;
        }

        public static GrupoProdutoModel RecuperarPeloId(int id) {

            GrupoProdutoModel retorno = null;

            using (var conexao = new SqlConnection()) {

                var Banco = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;

                conexao.ConnectionString = Banco;
                conexao.Open();

                using (var comando = new SqlCommand()) {

                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from grupo_produto where id = {0}", id);

                    var reader = comando.ExecuteReader();
                    while (reader.Read()) {

                        retorno = new GrupoProdutoModel {

                            Id = (int)reader["id"],
                            Nome = (string)reader["nome"],
                            Status = (bool)reader["status"]
                        };
                    }
                }
            }

            return retorno;
        }

        public static bool ExcluirPeloId(int id) {

            var retorno = false;

            if(RecuperarPeloId(id) != null) {
                
                using (var conexao = new SqlConnection()) {

                    var Banco = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;

                    conexao.ConnectionString = Banco;
                    conexao.Open();

                    using (var comando = new SqlCommand()) {

                        comando.Connection = conexao;
                        comando.CommandText = string.Format("delete from grupo_produto where id = {0}", id);

                        retorno = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return retorno;
        }

        public int Salvar() {

            var retorno = 0;

            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SqlConnection()) {

                var Banco = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;

                conexao.ConnectionString = Banco;
                conexao.Open();

                using (var comando = new SqlCommand()) {

                    comando.Connection = conexao;

                    if (model == null) {
                        
                        comando.CommandText = string.Format("insert into grupo_produto (nome, status) values ('{0}', {1}); select convert(int, scope_identity())", this.Nome, this.Status ? 1 : 0);
                        retorno = (int)comando.ExecuteScalar();
                    }else {

                        comando.CommandText = string.Format(
                                        "update grupo_produto set nome='{1}', status={2} where id={0}", 
                                         this.Id, this.Nome, this.Status ? 1 : 0);

                        if(comando.ExecuteNonQuery() > 0) {

                            retorno = this.Id;
                        }
                    }
                }
            }

            return retorno;
        }
    }
}