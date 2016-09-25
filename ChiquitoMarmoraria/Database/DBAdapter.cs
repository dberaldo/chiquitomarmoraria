using System;
using Android.Database.Sqlite;
using Android.Content;
using Android.Database;

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

		//Inserção de dados
		public bool inserirDados(string nome_material, string categoria, string descricao, double preco)
		{
			try
			{
				ContentValues dados_insercao = new ContentValues();
				dados_insercao.Put(ConstantesDB.NOME_MATERIAL, nome_material);
				dados_insercao.Put(ConstantesDB.CATEGORIA, categoria);
				dados_insercao.Put(ConstantesDB.DESCRICAO, descricao);
				dados_insercao.Put(ConstantesDB.PRECO, preco);
				db.Insert(ConstantesDB.NOME_TABELA, ConstantesDB.ID_MATERIAL, dados_insercao);
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}

			return false;
		}

		//Consulta de dados
		public ICursor recuperarDados()
		{
			string[] columns = { ConstantesDB.ID_MATERIAL, ConstantesDB.NOME_MATERIAL, ConstantesDB.CATEGORIA, ConstantesDB.DESCRICAO, ConstantesDB.PRECO };

			return db.Query(ConstantesDB.NOME_TABELA, columns, null, null, null, null, null);
		}
	}
}
