
using System;
using Android.Database;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.IO;
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
				intent.PutExtra("foto", materiais[e.Position].Foto);

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

			MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso!");

                    MySqlCommand cmd = new MySqlCommand("select id, nome, categoria, descricao, preco, foto from material", con);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int id = reader.GetOrdinal("id");

                        while (reader.Read())
                        {
                            materiaisDisplay.Add(reader["nome"].ToString());

							/* Lendo a foto */
							int bufferSize = 65535;
							byte[] ImageData = new byte[bufferSize];

							if(reader["foto"] == null)
							{
								Console.WriteLine("Não tem foto!!!");
							}
							else
							{
								Console.WriteLine("Tem foto.");
							}

							reader.GetBytes(reader.GetOrdinal("foto"), 0, ImageData, 0, bufferSize );

							//Console.WriteLine("ImageData: " + Encoding.Default.GetString(ImageData));

							materiais.Add(new Material() { Id = reader.GetInt32(id), Nome = reader["nome"].ToString(), Categoria = reader["categoria"].ToString(), Descricao = reader["descricao"].ToString(), Preco = reader.GetDouble(4), Foto = ImageData} );
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
	}

}
