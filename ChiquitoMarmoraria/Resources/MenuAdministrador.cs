
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "Menu")]
	public class MenuAdministrador : Activity
	{
		Button btnCadastroMaterial;
		Button btnSolicitacoes;
		Button btnAgendamentos;
        Button btnEstatisticas;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.MenuAdministrador);
			// Create your application here

			btnCadastroMaterial = FindViewById<Button>(Resource.Id.btnCadastrarMaterial);

			MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");
			try
			{
				if (con.State == ConnectionState.Closed)
				{
					con.Open();
					Console.WriteLine("Menu Usuário!");
					MySqlCommand cmd = new MySqlCommand("select id, needNotifyAdmin from agendamento WHERE needNotifyAdmin=1", con);


					using (MySqlDataReader reader = cmd.ExecuteReader())
					{

						while (reader.Read())
						{
							//   materiaisDisplay.Add(reader["nome"].ToString());
							// Console.WriteLine("Adicionando. MateriaisDisplay: " + materiaisDisplay);


							Console.WriteLine("ta no reader");

							int needNot = reader.GetOrdinal("needNotifyAdmin");
							int id = reader.GetOrdinal("id");

							Console.WriteLine("needNot == " + needNot);
							Console.WriteLine("----------------------- NEW OBJECT --------------------------");
							Console.WriteLine("-------------------------------------------------------------");

							if (needNot == 1)
							{
								Intent intent = new Intent(this, typeof(Solicitacoes));

								const int pendingIntentId = 0;
								PendingIntent pendingIntent =
									PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);

								Notification.Builder builder = new Notification.Builder(this)
									.SetContentIntent(pendingIntent)
									.SetAutoCancel(true)
									.SetContentTitle("Nova mudança na agenda!")
									.SetContentText("Há um novo pedido ou cancelamento de agendamento de serviço em espera.")
									.SetSmallIcon(Resource.Drawable.Icon);

								Notification not = builder.Build();

								NotificationManager notManager = GetSystemService(Context.NotificationService) as NotificationManager;

								const int notid = 0;

								notManager.Notify(notid, not);

								Console.WriteLine("id == " + id);
							}
						}

					}

					MySqlCommand query = new MySqlCommand("UPDATE agendamento SET needNotifyAdmin=0 WHERE needNotifyAdmin=1", con);

					query.ExecuteNonQuery();

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


			btnCadastroMaterial.Click += (sender, e) => 
			{
				var intent = new Intent(this, typeof(VisualizacaoMateriais));
				StartActivity(intent);
			};

			btnAgendamentos = FindViewById<Button>(Resource.Id.btn_agendamentos);

			btnAgendamentos.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(AgendamentoAdministrador));
				StartActivity(intent);
			};

			btnSolicitacoes = FindViewById<Button>(Resource.Id.btn_solicitacoes);

			btnSolicitacoes.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(Solicitacoes));
				StartActivity(intent);
			};

            btnEstatisticas = FindViewById<Button>(Resource.Id.btnEstatisticas);

            btnEstatisticas.Click += (sender, e) =>
            {
                Console.WriteLine("entrou botao estatisticas");
                var intent = new Intent(this, typeof(SubMenuEstatisticas));
                StartActivity(intent);
            };
        }
	}
}
