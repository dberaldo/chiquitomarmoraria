using System;
using Android.Database.Sqlite;
using Android.Content;
using Android.Database;
using ChiquitoMarmoraria.Resources.model;

namespace ChiquitoMarmoraria
{
	public class DBAdapter
	{
		private Context c;
		private SQLiteDatabase db;
		private DBHelper helper;

		public DBAdapter(Context c)
		{
			this.c = c;
			helper = new DBHelper(this.c);
		}

		//Conectar ao banco
		public DBAdapter openDB()
		{
			try
			{
				db = helper.WritableDatabase;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}

			return this;
		}

		//Desconectar do banco
		public void closeDB()
		{
			try
			{
				helper.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}
		}

		//Inserção de dados de materias
		public bool inserirDados(string nome_material, string categoria, string descricao, double preco)
		{
			try
			{
				ContentValues dados_insercao = new ContentValues();
				dados_insercao.Put(ConstantesDB.NOME_MATERIAL, nome_material);
				dados_insercao.Put(ConstantesDB.CATEGORIA, categoria);
				dados_insercao.Put(ConstantesDB.DESCRICAO, descricao);
				dados_insercao.Put(ConstantesDB.PRECO, preco);
				db.Insert(ConstantesDB.NOME_TABELA_MATERIAIS, ConstantesDB.ID_MATERIAL, dados_insercao);
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}

			return false;
		}

		//Consulta de dados de materiais
		public ICursor recuperarDados()
		{
			string[] columns = { ConstantesDB.ID_MATERIAL, ConstantesDB.NOME_MATERIAL, ConstantesDB.CATEGORIA, ConstantesDB.DESCRICAO, ConstantesDB.PRECO };

			return db.Query(ConstantesDB.NOME_TABELA_MATERIAIS, columns, null, null, null, null, null);
		}

		//Edição de dados materiais
		public bool excluirDados(int id)
		{
			string idString = id.ToString();
			try
			{
				db.Delete(ConstantesDB.NOME_TABELA_MATERIAIS, ConstantesDB.ID_MATERIAL + "= ?", new String[] { idString });
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}
			return false;
		}

        //Insercao de dados de pessoas
        public bool inserirPessoa(string nome_pessoa, string email, string senha)
        {
            try
            {
                ContentValues dados_insercao = new ContentValues();
                dados_insercao.Put(ConstantesDB.NOME_PESSOA, nome_pessoa);
                dados_insercao.Put(ConstantesDB.EMAIL, email);
                dados_insercao.Put(ConstantesDB.SENHA, senha);
                db.Insert(ConstantesDB.NOME_TABELA_PESSOA, ConstantesDB.ID_PESSOA, dados_insercao);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return false;
        }

        public ICursor recuperarDadosPessoas()
        {
            string[] columns = { ConstantesDB.ID_PESSOA, ConstantesDB.NOME_PESSOA, ConstantesDB.EMAIL, ConstantesDB.SENHA};

            return db.Query(ConstantesDB.NOME_TABELA_PESSOA, columns, null, null, null, null, null);
        }

    }
}
