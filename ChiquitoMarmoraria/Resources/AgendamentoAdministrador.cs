
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
using MySql.Data.MySqlClient;
using ChiquitoMarmoraria.Resources.Model;
using ChiquitoMarmoraria.Resources;
using System.Data;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "AgendamentoAdministrador")]
	public class AgendamentoAdministrador : Activity
	{
		ListView resultAgenda;
		AgendamentoAdapter adapter;
		List<Agendamento> agendamentos = new List<Agendamento>();
		JavaList<string> agendamentosDisplay = new JavaList<string>();

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.AgendamentoAdministrador);

            DateTime data = DateTime.Parse(Intent.GetStringExtra("data"));

            resultAgenda = FindViewById<ListView>(Resource.Id.resultAgenda);
			adapter = new AgendamentoAdapter(this, agendamentos);

			DateTime today = DateTime.Now.Date;
			Console.WriteLine("DATE = " + today.Year + "-" + today.Month + "-" + today.Day);

			MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");
			try
			{
				if (con.State == ConnectionState.Closed)
				{
					con.Open();
					Console.WriteLine("Conectado com sucesso Agendamento Usuario!");
					MySqlCommand cmd = new MySqlCommand("select id, data, id_servico, id_usuario, confirmado from agendamento WHERE data = @data AND confirmado=1", con);

					cmd.Parameters.AddWithValue("@data", data);

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							Console.WriteLine("ta no reader");
							int id = reader.GetOrdinal("id");

							Agendamento evento = new Agendamento();
							evento.Id = reader.GetInt32(id);
							id = reader.GetOrdinal("id_servico");
							evento.IdServico = reader.GetInt32(id);
							id = reader.GetOrdinal("id_usuario");
							evento.IdUsuario = reader.GetInt32(id);
							id = reader.GetOrdinal("confirmado");
							evento.Confirmado = reader.GetInt32(id);
							evento.Data = reader.GetDateTime("data");

							Console.WriteLine("-------------------------------------------------------------");
							Console.WriteLine("----------------------- NEW OBJECT --------------------------");
							Console.WriteLine("-------------------------------------------------------------");

							Console.WriteLine("Id= " + evento.Id + " servico= " + evento.IdServico + " usuario= " + evento.IdUsuario);
							Console.WriteLine("confirmado= " + evento.Confirmado + "data= " + evento.Data);

							agendamentos.Add(evento);

							if (evento.IdServico == 1)
								agendamentosDisplay.Add(evento.Data + " - Medicao");
							else if (evento.IdServico == 2)
								agendamentosDisplay.Add(evento.Data + " - Entrega");
							else if (evento.IdServico == 3)
								agendamentosDisplay.Add(evento.Data + " - Instalacao");

						}


					}

					if (agendamentosDisplay.Size() > 0)
					{
						resultAgenda.Adapter = adapter;
					}
				}
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
				Toast.MakeText(this, "Erro ao carregar agenda!", ToastLength.Short).Show();

			}
			finally
			{
				con.Close();
			}

			resultAgenda.ItemClick += (sender, e) =>
			{
                Console.WriteLine("Position: " + e.Position);
                Console.WriteLine("idusuario: " + agendamentos[e.Position].IdUsuario);

				var intent = new Intent(this, typeof(Activity1));
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
	}
}
