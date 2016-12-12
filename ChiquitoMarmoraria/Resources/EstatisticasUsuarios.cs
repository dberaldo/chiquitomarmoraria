
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
	[Activity(Label = "EstatisticasUsuarios")]
	public class EstatisticasUsuarios : Activity
	{
		ListView lista;
		List<int> lista_qtde = new List<int>();
		List<string> lista_regioes = new List<string>();
		Button btnVoltar;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.EstatisticasUsuarios);
			lista = FindViewById<ListView>(Resource.Id.lista_regioes);
			consulta_estatisticas();
			lista.Adapter = new ListViewAdapter(this, lista_regioes, lista_qtde);
			btnVoltar = FindViewById<Button>(Resource.Id.btnVoltar2);

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

					MySqlCommand cmd = new MySqlCommand("SELECT count(*) AS qtde, pessoa.bairro FROM orcamento, pessoa WHERE orcamento.id_usuario = pessoa.id GROUP BY pessoa.bairro ORDER BY count(*) DESC", con);

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							String nome_bairro = reader["bairro"].ToString();
							int qtde = reader.GetInt32("qtde");
							lista_regioes.Add(nome_bairro);
							lista_qtde.Add(qtde);
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
