
using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;
using Android.Content;
using ChiquitoMarmoraria.Resources.model;
using Android.Views;
using System.Net.Mail;
using System.Net.Mime;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "EsqueciSenha")]
	public class EsqueciSenha : Activity
	{
        Pessoa p;

        //Cadastro Básico
		EditText txtEmail;
        Button btnProximo;

        // Button btnVoltar;

        LinearLayout llBasico;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            
			SetContentView(Resource.Layout.EsqueciSenha);

            p = new ChiquitoMarmoraria.Resources.model.Pessoa();

            llBasico = FindViewById<LinearLayout>(Resource.Id.ll_infoBasic);

            //Cadastro Básico
			txtEmail = FindViewById<EditText>(Resource.Id.txt_email);
            btnProximo = FindViewById<Button>(Resource.Id.btn_proximo);

            // btnVoltar = FindViewById<Button>(Resource.Id.btn_voltar);

            btnProximo.Click += (object sender, EventArgs e) =>
            {
                p.Email = txtEmail.Text;
                cadastroPessoa(p);
            };

        }

        public void cadastroPessoa(Pessoa p)
        {
            MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso!");
                    MySqlCommand cmd = new MySqlCommand("SELECT senha, nome FROM pessoa where email = @email", con);

                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string senha = reader.GetString("senha");
                            string nome = reader.GetString("nome");

                            MailMessage mailMsg = new MailMessage();

                            // To
                            mailMsg.To.Add(new MailAddress(txtEmail.Text, nome));

                            // From
                            mailMsg.From = new MailAddress("donotreply@chiquitomarmoraria.com", "Chiquito Marmoraria");

                            // Subject and multipart/alternative Body
                            mailMsg.Subject = "Senha Chiquito Marmoraria";
                            string text = "Olá, " + nome + ". Sua senha da Chiquito Marmoraria é: " + senha + "\n\nObrigado por usar nosso aplicativo!";
                            string html = @"<p>Olá, " + nome + ". Sua senha da Chiquito Marmoraria é: " + senha + "<br /><br />Obrigado por usar nosso aplicativo!</p>";
                            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                            // Init SmtpClient and send
                            SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("ufscarpds", "ufscar1993");
                            smtpClient.Credentials = credentials;

                            smtpClient.Send(mailMsg);

                            Toast.MakeText(this, "Senha enviada para o email " + txtEmail.Text, ToastLength.Short).Show();

                            //Redireciona para a página de Login
                            var intent = new Intent(this, typeof(MainActivity));
                            StartActivity(intent);
                        }
                        else
                        {
                            Toast.MakeText(this, "Email não cadastrado", ToastLength.Short).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Toast.MakeText(this, "Houve um problema de conexão.", ToastLength.Short).Show();
            }
            finally
            {
                con.Close();
            }
        }
	}
}
