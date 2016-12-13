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
        TextView txtNome;
        TextView txtEndereco;
        TextView txtCidade;
        TextView txtNumero;
        TextView txtTelefone;
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
            txtNome = FindViewById<TextView>(Resource.Id.txt_nome);
            txtEndereco = FindViewById<TextView>(Resource.Id.txt_endereco);
            txtCidade = FindViewById<TextView>(Resource.Id.txt_cidade);
            txtNumero = FindViewById<TextView>(Resource.Id.txt_numero);
            txtTelefone = FindViewById<TextView>(Resource.Id.txt_telefone);
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



            MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

            string endereco = "";
            string nome = "";
            int numero=0;
            string cidade = "";
            string complemento = "";
            string telefone = "";

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso!");

                }

                MySqlCommand cmd = new MySqlCommand("Select nome, endereco, numero, cidade, telefone, complemento from pessoa where id=@id;", con);
                cmd.Parameters.AddWithValue("@id", a.IdUsuario.ToString());

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Passou execute reader!");
                    if (reader.Read())
                    {

                        numero = reader.GetOrdinal("numero");
                        nome = reader["nome"].ToString();
                        endereco = reader["endereco"].ToString();
                        cidade = reader["cidade"].ToString();
                        complemento = reader["complemento"].ToString();
                        telefone = reader["telefone"].ToString();
                        Console.WriteLine("Passou REad reader!");
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
            
            txtNome.Text = nome;
            txtEndereco.Text = endereco + ", "+ numero.ToString();
            txtCidade.Text = cidade;
            txtTelefone.Text = telefone;
            
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
				MySqlConnection con2 = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");
				try
				{
					if (con2.State == ConnectionState.Closed)
					{
						con2.Open();
						Console.WriteLine("Conectado com sucesso Agendamento Usuario!");
						MySqlCommand cmd = new MySqlCommand("UPDATE agendamento SET confirmado=1, needNotifyClient=1 WHERE id=@id", con2);
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
					con2.Close();
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
						MySqlConnection con3 = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");
						try
						{
							if (con3.State == ConnectionState.Closed)
							{
								con3.Open();
								Console.WriteLine("Conectado com sucesso Agendamento Usuario!");
								MySqlCommand cmd = new MySqlCommand("UPDATE agendamento SET confirmado=0, needNotifyClient=1 WHERE id=@id", con3);
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
							con3.Close();
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