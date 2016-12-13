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

using System.Data;
using ChiquitoMarmoraria.Resources;

namespace ChiquitoMarmoraria.Resources
{
    [Activity(Label = "Menu")]
    public class MenuUsuario : Activity
    {
        TextView lblBemvindo;
        Button btnOrcamento;
        Button btnAgendamento;
        Button btnMeusAgedamentos;
        Button btnMateriais;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MenuUsuario);

            lblBemvindo = FindViewById<TextView>(Resource.Id.lbl_bemvindo);
            string id = Intent.GetStringExtra("id") ?? "Data not available";

            btnOrcamento = FindViewById<Button>(Resource.Id.btn_orcamento);
            btnAgendamento = FindViewById<Button>(Resource.Id.btn_agendamento);
            btnMeusAgedamentos = FindViewById<Button>(Resource.Id.btn_meusagendamentos);
            btnMateriais = FindViewById<Button>(Resource.Id.btnMateriaisDisponiveis);


            MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

            string email="";
            string nome="";
            string senha="";

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso!");

                }

                MySqlCommand cmd = new MySqlCommand("Select email, id, nome, senha from pessoa where id=@id;", con);
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Passou execute reader!");
                    if (reader.Read())
                    {
                        Console.WriteLine("Passou REad reader!");
                        email = reader["email"].ToString();
                        nome = reader["nome"].ToString();
                        senha = reader["senha"].ToString();
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

            lblBemvindo.Text = "Bem-vindo(a), " + nome + "!";

			MySqlConnection conn = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

			try
			{
				if (conn.State == ConnectionState.Closed)
				{
					conn.Open();
					Console.WriteLine("Conectado com sucesso2!");

					MySqlCommand cmd = new MySqlCommand("select id, needNotifyClient from agendamento where id_usuario = @id_usuario AND needNotifyClient = 1", conn);
					cmd.Parameters.AddWithValue("@id_usuario", id);

					Console.WriteLine("Passou comando2!");

					using (MySqlDataReader reader = cmd.ExecuteReader())
					{
						Console.WriteLine("OH GOSH BUGOU!");

						while (reader.Read())
						{
							//   materiaisDisplay.Add(reader["nome"].ToString());
							// Console.WriteLine("Adicionando. MateriaisDisplay: " + materiaisDisplay);


							Console.WriteLine("Passou execute reader2!");
							//int id2 = reader.GetOrdinal("id");
							//int idservico = reader.GetOrdinal("id_servico");
							//int idusuario = reader.GetOrdinal("id_usuario");
							//int conf = reader.GetOrdinal("confirmado");
							//String conf = reader.GetString("confirmado");
							int needNot = reader.GetOrdinal("needNotifyClient");
							int idAg = reader.GetOrdinal("id");
							Console.WriteLine(needNot);

							if (needNot == 1)
							{
								Intent intent = new Intent(this, typeof(MeusAgendamentos));
								intent.PutExtra("id", id);

								const int pendingIntentId = 0;
								PendingIntent pendingIntent =
									PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);

								Notification.Builder builder = new Notification.Builder(this)
									.SetContentIntent(pendingIntent)
									.SetAutoCancel(true)
									.SetContentTitle("Resposta de Agendamento")
									.SetContentText("Seu agendamento recebeu uma resposta")
									.SetSmallIcon(Resource.Drawable.Icon);

								Notification not = builder.Build();

								NotificationManager notManager = GetSystemService(Context.NotificationService) as NotificationManager;

								const int notid = 0;

								notManager.Notify(notid, not);

								Console.WriteLine("id mudado ==" + idAg);
							}


						}

					}

					MySqlCommand query = new MySqlCommand("UPDATE agendamento SET needNotifyClient=0 WHERE id_usuario = @idUs AND needNotifyClient=1", conn);
					Console.WriteLine("USER = " + id + " - NEEDNOT = ");
					query.Parameters.AddWithValue("@idUs", id);
					query.ExecuteNonQuery();

				}
			}
			catch (MySqlException ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				conn.Close();
			}

            btnOrcamento.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(MeuOrcamento));
                intent.PutExtra("id", id);
                StartActivity(intent);
                //Finish();
            };

            btnAgendamento.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(AgendamentoUsuario));
                intent.PutExtra("id", id);
                StartActivity(intent);

            };

            btnMeusAgedamentos.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(MeusAgendamentosCalendario));
                intent.PutExtra("id", id);
                Console.WriteLine("MenuUduario ID = "+ id);
                StartActivity(intent);
            };

            btnMateriais.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(GridViewMateriais));
                StartActivity(intent);
            };

        }
    }
}