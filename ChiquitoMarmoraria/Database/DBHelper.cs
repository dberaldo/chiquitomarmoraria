using System;
using Android.Database.Sqlite;
using Android.Content;

namespace ChiquitoMarmoraria
{
	public class DBHelper : SQLiteOpenHelper
	{
		public DBHelper(Context context) : base(context, ConstantesDB.NOME_BANCO, null, ConstantesDB.VERSAO_BANCO)
		{
			Console.WriteLine("Banco de dados criando...");
		}

		/* Para fazer o OnCreate rodar de novo, delete os arquivos do banco localizados em /data/data/<nome da aplicação> 
		Assim ele vai ter que criar o banco de dados de novo */
		public override void OnCreate(SQLiteDatabase db)
		{
			try
			{
				db.ExecSQL(ConstantesDB.CREATE_TABLE);
				Console.WriteLine("Tabela criada com sucesso!");
			}
			catch (Exception e)
			{
				Console.WriteLine("Tabela NAO FOI CRIADA.");
				Console.WriteLine("Exception: " + e.Message);
			}
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			try
			{
				db.ExecSQL(ConstantesDB.DROP_TABLE);
				OnCreate(db);
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}
		}
	}
}
