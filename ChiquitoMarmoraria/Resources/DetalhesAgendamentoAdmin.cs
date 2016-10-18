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
using ChiquitoMarmoraria.Resources.Model;
using MySql.Data.MySqlClient;
using System.Data;

namespace ChiquitoMarmoraria.Resources
{
	[Activity(Label = "DetalhesAgendamentoAdmin")]
	public class DetalhesAgendamentoAdmin : Activity
	{

		TextView txtTipo;
		TextView txtData;
		TextView txtStatus;
		Agendamento a;
		Button btnVoltar;

		Button btnAceitar;
		Button btnCancelar;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.DetalhesAgendamentoAdmin);

			// Create your application here

			txtTipo = FindViewById<TextView>(Resource.Id.txt_tipo);
			txtStatus = FindViewById<TextView>(Resource.Id.txt_status);
			txtData = FindViewById<TextView>(Resource.Id.txt_data);
			btnVoltar = FindViewById<Button>(Resource.Id.btn_voltar);
			btnAceitar = FindViewById<Button>(Resource.Id.btnAceitar);
			btnCancelar = FindViewById<Button>(Resource.Id.btnCancelar);

			int dia = Intent.GetIntExtra("day", 0);
			int mes = Intent.GetIntExtra("month", 0);
			int ano = Intent.GetIntExtra("year", 0);


			a = new Agendamento();
			a.Id = Intent.GetIntExtra("id", 0);
			a.IdServico = Intent.GetIntExtra("idservico", 0);
			a.IdUsuario = Intent.GetIntExtra("idusuario", 0);
			a.Confirmado = Intent.GetIntExtra("status", 0);

			if (a.IdServico == 1)
				txtTipo.Text = "Medicao";
			else if (a.IdServico == 2)
				txtTipo.Text = "Entrega";
			else if (a.IdServico == 3)
				txtTipo.Text = "Instalacao";

			if (a.Confirmado == -1)
				txtStatus.Text = "Pendente";
			else if (a.Confirmado == 0)
				txtStatus.Text = "Cancelado";
			else if (a.Confirmado == 1)
				txtStatus.Text = "Confirmado";

			txtData.Text = dia + "/" + mes + "/" + ano;

			btnVoltar.Click += (sender, e) =>
			{
				Finish();
			};

			btnAceitar.Click += (sender, e) => { 
				MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");
				try
				{
					if (con.State == ConnectionState.Closed)
					{
						con.Open();
						Console.WriteLine("Conectado com sucesso Agendamento Usuario!");
						MySqlCommand cmd = new MySqlCommand("UPDATE agendamento SET confirmado=1 WHERE id=@id", con);
						cmd.Parameters.AddWithValue("@id", a.Id);
						cmd.ExecuteNonQuery();
						//quando o usuario envia o agendamento a coluna confirmado deve ter valor false
						//somente terá valor true quando o adm confirmar a solicitacao de agendamento

						// apos cadastrar o agendamento no banco exibir uma mensaeg para o usaurio
						// informando que a solicitaçao está pendente 
						Toast.MakeText(this, "Solicitação aceita com sucesso!", ToastLength.Short).Show();
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

				//apos exibir mensagem chamar de solicitações
				var intent = new Intent(this, typeof(Solicitacoes));
				StartActivity(intent);
				Finish();

			};

			btnCancelar.Click += (sender, e) =>
			{
				new AlertDialog.Builder(this)
					.SetPositiveButton("Sim", (sender2, args) =>
					{
						MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");
						try
						{
							if (con.State == ConnectionState.Closed)
							{
								con.Open();
								Console.WriteLine("Conectado com sucesso Agendamento Usuario!");
								MySqlCommand cmd = new MySqlCommand("UPDATE agendamento SET confirmado=0 WHERE id=@id", con);
								cmd.Parameters.AddWithValue("@id", a.Id);
								cmd.ExecuteNonQuery();
								//quando o usuario envia o agendamento a coluna confirmado deve ter valor false
								//somente terá valor true quando o adm confirmar a solicitacao de agendamento

								// apos cadastrar o agendamento no banco exibir uma mensaeg para o usaurio
								// informando que a solicitaçao está pendente 
								Toast.MakeText(this, "Solicitação aceita com sucesso!", ToastLength.Short).Show();
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

						//apos exibir mensagem chamar de solicitações
						var intent = new Intent(this, typeof(Solicitacoes));
						StartActivity(intent);
						Finish();
					})
					.SetNegativeButton("Nao", (sender2, args) =>
					{
						// User pressed no 
					})
					.SetMessage("Tem certeza que deseja recusar a solicitacao?")
					.SetTitle("Alerta")
					.Show();
				
			};

		}
	}
}