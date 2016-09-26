using System;
namespace ChiquitoMarmoraria
{
	public class ConstantesDB
	{
		//Colunas da tabela Material
		public static string ID_MATERIAL = "id";
		public static string NOME_MATERIAL = "nome_material";
		public static string CATEGORIA = "categoria";
		public static string DESCRICAO = "descricao";
		public static string PRECO = "preco";

        //Colunas da tabela Pessoa
        public static string ID_PESSOA = "id";
        public static string NOME_PESSOA = "nome_pessoa";
        public static string EMAIL = "email";
        public static string SENHA = "senha";

		//Nome do banco
		public static string NOME_BANCO = "ChiquitoMarmoraria";
		public static int VERSAO_BANCO = 1;
		public static string NOME_TABELA_MATERIAIS = "materiais";
        public static string NOME_TABELA_PESSOA = "pessoas";

		//Queries do de criação e de exclusão da tabela materiais
		public static string CREATE_TABLE = "CREATE TABLE " + NOME_TABELA_MATERIAIS + " (" +
			"id INTEGER PRIMARY KEY AUTOINCREMENT, nome_material VARCHAR(60) NOT NULL, " +
			"categoria VARCHAR(12) NOT NULL, " +
			"descricao text, " +
			"preco DOUBLE)";

		public static string DROP_TABLE = "DROP TABLE IF EXISTS " + NOME_TABELA_MATERIAIS;

        //Queries de criação e de exclusão da tabela pessoa
        public static string CREATE_TABLE_PESSOA = "CREATE TABLE " + NOME_TABELA_PESSOA + " (" +
            "id INTEGER PRIMARY KEY AUTOINCREMENT, nome_pessoa VARCHAR(60) NOT NULL, " +
            "email VARCHAR(30) NOT NULL, " +
            "senha VARCHAR(20) NOT NULL)";

        public static string DROP_TABLE_PESSOA = "DROP TABLE IF EXISTS " + NOME_TABELA_PESSOA;


    }
}
