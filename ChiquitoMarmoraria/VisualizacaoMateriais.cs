
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

				Console.WriteLine("e.Position = " + e.Position);

				Console.WriteLine("Mateirais no evento: ");
				for (int i = 0; i < materiais.Count; i++)
				{
					Console.WriteLine("id: " + materiais[i].Id);
					Console.WriteLine("Nome: " + materiais[i].Nome);
				}

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
			if(materiaisDisplay.Size() > 0)
			{
				materiaisDisplay.Clear();
				materiais.Clear();
			}

			Console.WriteLine("Mateirais antes de dar retrieve: ");
			for (int i = 0; i < materiais.Count; i++)
			{
				Console.WriteLine("i = " + i);
				Console.WriteLine("id: " + materiais[i].Id);
				Console.WriteLine("Nome: " + materiais[i].Nome);
			}


			MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso!");

                    MySqlCommand cmd = new MySqlCommand("select id, nome, categoria, descricao, preco from material", con);

                    //Console.WriteLine("Passou comando!");

                    //materiais = InicializarArray<Material>(materiaisDisplay.Size());

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Console.WriteLine("Passou execute reader!");
                        int id = reader.GetOrdinal("id");

                        while (reader.Read())
                        {
                            materiaisDisplay.Add(reader["nome"].ToString());
                            Console.WriteLine("Adicionando. MateriaisDisplay: " + materiaisDisplay);

							materiais.Add(new Material() { Id = reader.GetInt32(id), Nome = reader["nome"].ToString(), Categoria = reader["categoria"].ToString(), Descricao = reader["descricao"].ToString(), Preco = reader.GetDouble(4)} );

							Console.WriteLine("Loop com k: " + materiais.Count);
							for (int k = 0; k < materiais.Count; k++)
							{
								Console.WriteLine("id: " + materiais[k].Id);
								Console.WriteLine("Nome: " + materiais[k].Nome);
							}

							/*Console.WriteLine("Adicionando... count-1: " + (materiais.Count - 1));
							Console.WriteLine("id: " + materiais[materiais.Count-1].Id);
							Console.WriteLine("Nome: " + materiais[materiais.Count-1].Nome);*/

                        }

						Console.WriteLine("Materiais depois de dar retrieve: Count: " + materiais.Count);
						for (int i = 0; i < materiais.Count; i++)
						{
							Console.WriteLine("i = " + i);
							Console.WriteLine("id: " + materiais[i].Id);
							Console.WriteLine("Nome: " + materiais[i].Nome);
						}

                    }

                    if (materiaisDisplay.Size() > 0)
                    {
                        lista.Adapter = adapter;
                    }
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
