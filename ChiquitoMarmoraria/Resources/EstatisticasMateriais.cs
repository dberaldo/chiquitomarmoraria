
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "EstatisticasMateriais")]
	public class EstatisticasMateriais : Activity
	{
		List<int> lista_quantidades = new List<int>();
		List<string> lista_materiais = new List<string>();
		Button btnVoltar;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Console.WriteLine("passou do onCreate savedInstance.");

			SetContentView(Resource.Layout.EstatisticasMateriais);
			ListView lista = FindViewById<ListView>(Resource.Id.listView1);
			btnVoltar = FindViewById<Button>(Resource.Id.btnVoltarEstatisticas);
			Console.WriteLine("passou do Button");
			consulta_estatisticas();
			Console.WriteLine("passou do consultar");
			lista.Adapter = new ListViewAdapter(this, lista_materiais, lista_quantidades);
			btnVoltar.Click += (sender, e) => 
			{
				Finish();
			};
		}

		protected void consulta_estatisticas()
		{
			MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

			try
			{
				if (con.State == ConnectionState.Closed)
				{
					con.Open();
					Console.WriteLine("Conectado com sucesso!");

					MySqlCommand cmd = new MySqlCommand("SELECT material, count(material) AS qtde FROM orcamento GROUP BY material ORDER BY count(material) DESC", con);

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							String nome_material = reader["material"].ToString();
							int qtde = reader.GetInt32("qtde");
							lista_materiais.Add(nome_material);
							lista_quantidades.Add(qtde);
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
	}
}
