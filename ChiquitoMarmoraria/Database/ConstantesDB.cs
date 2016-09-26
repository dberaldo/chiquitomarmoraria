using System;
namespace ChiquitoMarmoraria
{
	public class ConstantesDB
	{
		//Colunas da tabela
		public static string ID_MATERIAL = "id";
		public static string NOME_MATERIAL = "nome_material";
		public static string CATEGORIA = "categoria";
		public static string DESCRICAO = "descricao";
		public static string PRECO = "preco";

		//Nome do banco
		public static string NOME_BANCO = "ChiquitoMarmoraria";
		public static int VERSAO_BANCO = 1;
		public static string NOME_TABELA = "materiais";

		//Queries do de criação e de exclusão da tabela
		public static string CREATE_TABLE = "CREATE TABLE " + NOME_TABELA + " (" +
			"id INTEGER PRIMARY KEY AUTOINCREMENT, nome_material VARCHAR(60) NOT NULL, " +
			"categoria VARCHAR(12) NOT NULL, " +
			"descricao text, " +
			"preco DOUBLE)";

		public static string DROP_TABLE = "DROP TABLE IF EXISTS " + NOME_TABELA;
	}
}
