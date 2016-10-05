
using System;
using Android.Database;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using Java.Util;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "Visualização de Materiais")]
	public class VisualizacaoMateriais : Activity
	{
		Button btnAdd;
		Button btnAtualizar;
		ListView lista;
		JavaList<string> materiaisDisplay = new JavaList<string>();
		List<Material> materiais = new List<Material>();
		ArrayAdapter adapter;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.VisualizacaoMateriais);
			// Create your application here

			btnAdd = FindViewById<Button>(Resource.Id.btnAdicionar);
			btnAtualizar = FindViewById<Button>(Resource.Id.btnAtualizar);
			lista = FindViewById<ListView>(Resource.Id.listaMateriais);
			adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, materiaisDisplay);

			retrieve();

			lista.ItemClick += (sender, e) => {
				var intent = new Intent(this, typeof(DetalhesMaterial));
				intent.PutExtra("id", materiais[e.Position].Id);
				intent.PutExtra("nome", materiais[e.Position].Nome);
				intent.PutExtra("categoria", materiais[e.Position].Categoria);
				intent.PutExtra("descricao", materiais[e.Position].Descricao);
				intent.PutExtra("preco", materiais[e.Position].Preco);

				StartActivity(intent);
                Finish();
			};

			btnAdd.Click += (sender, e) => 
			{
				var intent = new Intent(this, typeof(CadastroMateriais));
				StartActivity(intent);
                Finish();
			};

			btnAtualizar.Click += (sender, e) => 
			{ 
				retrieve();
			};
		}

		public void retrieve()
		{

            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=ufscarpds;User Id=ufscarpds;Password=19931993;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso!");
                }

                MySqlCommand cmd = new MySqlCommand("Select id, nome, categoria, descricao, preco from material", con);
                
                Material mat = new Material();

                //materiais = InicializarArray<Material>(materiaisDisplay.Size());

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Passou execute reader!");

                    while (reader.Read())
                    {
                        materiaisDisplay.Add(reader["nome"].ToString());
                        Console.WriteLine("Adicionando. MateriaisDisplay: " + materiaisDisplay);

                        mat.Id = (int)reader["id"];
                        mat.Nome = reader["nome"].ToString();
                        mat.Categoria = reader["categoria"].ToString();
                        mat.Descricao = reader["descricao"].ToString();
                        mat.Preco = (float)reader["preco"];

                        Console.WriteLine("Id= "+mat.Id+" Nome= "+ mat.Nome+" Cat= "+ mat.Categoria);
                        Console.WriteLine("descr= " + mat.Descricao + "preco= " + mat.Preco);

                        materiais.Add(mat);
                        
                    }

                   
                }

                if (materiaisDisplay.Size() > 0)
                {
                    lista.Adapter = adapter;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }	
		
		}

		T[] InicializarArray<T>(int length) where T : new()
		{
			T[] array = new T[length];
			for (int i = 0; i < length; ++i)
			{
				array[i] = new T();
			}

			return array;
		}
	}

}
