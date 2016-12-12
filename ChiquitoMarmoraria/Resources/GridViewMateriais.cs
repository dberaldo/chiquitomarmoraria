
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "GridViewMateriais")]
	public class GridViewMateriais : Activity
	{
		List<Material> materiais = new List<Material>();
		List<Bitmap> lista_fotos = new List<Bitmap>();
		List<string> lista_nomes = new List<string>();
		Button btnVoltar;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.MateriaisUsuario);
			btnVoltar = FindViewById<Button>(Resource.Id.btnVoltar3);
			GridView gridview = FindViewById<GridView>(Resource.Id.myGridView1);
			consulta_materiais();
			gridview.Adapter = new ImageAdapter(this, lista_fotos, lista_nomes);

			//Seta a altura do gridview em tempo de execução
			setGridViewHeightBasedOnChildren(gridview, 2);

			btnVoltar.Click += (sender, e) => 
			{
				Finish();
			};
		}

		protected void consulta_materiais()
		{
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
							/* Lendo o nome */
							lista_nomes.Add(reader["nome"].ToString());

							/* Lendo a foto */
							int bufferSize = 65535;
							byte[] ImageData = new byte[bufferSize];

							reader.GetBytes(reader.GetOrdinal("foto"), 0, ImageData, 0, bufferSize);

							materiais.Add(new Material() { Id = reader.GetInt32(id), Nome = reader["nome"].ToString(), Categoria = reader["categoria"].ToString(), Descricao = reader["descricao"].ToString(), Preco = reader.GetDouble(4), Foto = ImageData });

							/* Convertendo os array de bytes em Bitmaps */
							Bitmap bmp = null;

							bmp = BitmapFactory.DecodeByteArray(ImageData, 0, ImageData.Length);

							if(bmp == null)
							{
								bmp = BitmapFactory.DecodeResource(Resources, Resource.Drawable.sem_foto);
							}

							lista_fotos.Add(bmp);
						}
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

		public void setGridViewHeightBasedOnChildren(GridView gridView, int columns)
		{
			IListAdapter listAdapter = gridView.Adapter;
			if (listAdapter == null)
			{
				// pre-condition
				return;
			}

			int totalHeight = 0;
			int items = listAdapter.Count;
			int rows = 0;

			View listItem = listAdapter.GetView(0, null, gridView);
			listItem.Measure(0, 0);
			totalHeight = listItem.MeasuredHeight;

			float x = 1;
			if (items > columns)
			{
				x = items / columns;
				rows = (int)(x + 1);
				totalHeight *= rows;
			}

			ViewGroup.LayoutParams parameters = gridView.LayoutParameters;
			parameters.Height = totalHeight;
			gridView.LayoutParameters = parameters;

		}
	}
}
