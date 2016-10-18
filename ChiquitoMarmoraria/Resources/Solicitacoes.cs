
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ChiquitoMarmoraria.Resources;
using ChiquitoMarmoraria.Resources.Model;
using MySql.Data.MySqlClient;
using System.Data;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "Solicitacoes")]
	public class Solicitacoes : Activity
	{
		List<Agendamento> agendamentos = new List<Agendamento>();
		JavaList<string> agendamentosDisplay = new JavaList<string>();
		ListView solicitacoes;
		AgendamentoAdapter adapter;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Solicitacoes);

			solicitacoes = FindViewById<ListView>(Resource.Id.resultSocilitacoes);
			adapter = new AgendamentoAdapter(this, agendamentos);

			retrieve();

			solicitacoes.ItemClick += (sender, e) =>
			{
				var intent = new Intent(this, typeof(DetalhesAgendamentoAdmin));
				intent.PutExtra("id", agendamentos[e.Position].Id);
				intent.PutExtra("idservico", agendamentos[e.Position].IdServico);
				intent.PutExtra("idusuario", agendamentos[e.Position].IdUsuario);
				intent.PutExtra("day", agendamentos[e.Position].Data.Day);
				intent.PutExtra("month", agendamentos[e.Position].Data.Month);
				intent.PutExtra("year", agendamentos[e.Position].Data.Year);
				intent.PutExtra("status", agendamentos[e.Position].Confirmado);

				StartActivity(intent);


			};
		}

		public void retrieve()
		{

			MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

			try
			{
				if (con.State == ConnectionState.Closed)
				{
					con.Open();
					Console.WriteLine("Conectado com sucesso2!");

					MySqlCommand cmd = new MySqlCommand("select id, data, id_servico, id_usuario, confirmado from agendamento where confirmado=-1", con);

					Console.WriteLine("Passou comando2!");



					using (MySqlDataReader reader = cmd.ExecuteReader())
					{

						while (reader.Read())
						{
							//  materiaisDisplay.Add(reader["nome"].ToString());
							// Console.WriteLine("Adicionando. MateriaisDisplay: " + materiaisDisplay);


							Console.WriteLine("Passou execute reader2!");
							int id2 = reader.GetOrdinal("id");
							int idservico = reader.GetOrdinal("id_servico");
							int idusuario = reader.GetOrdinal("id_usuario");

							Agendamento a = new Agendamento();
							a.Id = reader.GetInt32(id2);
							a.Data = reader.GetDateTime("data");
							a.IdServico = reader.GetInt32(idservico);
							a.IdUsuario = reader.GetInt32(idusuario);
							a.Confirmado = reader.GetInt32("confirmado");

							Console.WriteLine("Id= " + a.Id + " Data= " + a.Data + " idservico= " + a.IdServico);
							Console.WriteLine("idusuario= " + a.IdUsuario + "confirmado= " + a.Confirmado);

							agendamentos.Add(a);

							if (a.IdServico == 1)
								agendamentosDisplay.Add(a.Data + " - Medicao");
							else if (a.IdServico == 2)
								agendamentosDisplay.Add(a.Data + " - Entrega");
							else if (a.IdServico == 3)
								agendamentosDisplay.Add(a.Data + " - Instalacao");

						}


					}

					if (agendamentosDisplay.Size() > 0)
					{
						solicitacoes.Adapter = adapter;
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
