
using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;
using Android.Content;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "Cadastro")]
	public class CadastroLogin : Activity
	{
		EditText txtNome;
		EditText txtEmail;
		EditText txtSenha;
		EditText txtSenhaRepete;
		Button btnConfirmar;
        Button btnVoltar;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            
			SetContentView(Resource.Layout.CadastroLogin);

			txtNome = FindViewById<EditText>(Resource.Id.txt_nome);
			txtEmail = FindViewById<EditText>(Resource.Id.txt_email);
			txtSenha = FindViewById<EditText>(Resource.Id.txt_senha);
			txtSenhaRepete = FindViewById<EditText>(Resource.Id.txt_senha_repete);
			btnConfirmar = FindViewById<Button>(Resource.Id.btn_confirmar);
            btnVoltar = FindViewById<Button>(Resource.Id.btn_voltar);

			btnConfirmar.Click += (object sender, EventArgs e) =>
			{
                if (txtSenha.Text != txtSenhaRepete.Text)
                {
                    txtSenha.Text = "";
                    txtSenhaRepete.Text = "";
                    Toast.MakeText(this, "Senhas não conferem. Por favor, tente novamente!", ToastLength.Short).Show();
                    
                }
                else
                {
                    cadastroPessoa(txtNome.Text, txtEmail.Text, txtSenha.Text);
                }

            };

            btnVoltar.Click += (object sender, EventArgs e) =>
            {

                //Redireciona para a página de Login
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);

            };

        }

        public void cadastroPessoa(string nome, string email, string senha)
        {
            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=ufscarpds;User Id=ufscarpds;Password=19931993;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso!");
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO pessoa (nome, email, senha) VALUES (@nome, @email, @senha)", con);
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@senha", txtSenha.Text);
                    cmd.ExecuteNonQuery();
                    txtNome.Text = "";
                    txtEmail.Text = "";
                    txtSenha.Text = "";
                    txtSenhaRepete.Text = "";
                    Toast.MakeText(this, "Cadastrado realizado com sucesso.", ToastLength.Short).Show();
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
