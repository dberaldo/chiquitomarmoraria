
using System;
using Android.App;
using Android.OS;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;
using Android.Content;
using ChiquitoMarmoraria.Resources.model;
using Android.Views;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "Cadastro")]
	public class CadastroLogin : Activity
	{
        Pessoa p;

        //Cadastro Básico
		EditText txtNome;
		EditText txtEmail;
		EditText txtSenha;
		EditText txtSenhaRepete;
        Button btnProximo;

        //Cadastro Endereço
        EditText txtCep;
        EditText txtEstado;
        EditText txtCidade;
        EditText txtEndereco;
        EditText txtNumero;
        EditText txtComplemento;
        EditText txtTelefone;
        EditText txtBairro;
        Button btnConfirmar;
        Button btnVoltar;

        LinearLayout llBasico;
        LinearLayout llEndereco;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            
			SetContentView(Resource.Layout.CadastroLogin);

            p = new ChiquitoMarmoraria.Resources.model.Pessoa();

            llBasico = FindViewById<LinearLayout>(Resource.Id.ll_infoBasic);
            llEndereco = FindViewById<LinearLayout>(Resource.Id.ll_infoContato);

            //Cadastro Básico
			txtNome = FindViewById<EditText>(Resource.Id.txt_nome);
			txtEmail = FindViewById<EditText>(Resource.Id.txt_email);
			txtSenha = FindViewById<EditText>(Resource.Id.txt_senha);
			txtSenhaRepete = FindViewById<EditText>(Resource.Id.txt_senha_repete);
            btnProximo = FindViewById<Button>(Resource.Id.btn_proximo);
            txtBairro = FindViewById<EditText>(Resource.Id.txt_bairro);
            //Cadastro Endereço
            txtCep = FindViewById<EditText>(Resource.Id.txt_cep);
            txtEstado = FindViewById<EditText>(Resource.Id.txt_estado);
            txtCidade = FindViewById<EditText>(Resource.Id.txt_cidade);
            txtEndereco = FindViewById<EditText>(Resource.Id.txt_endereco);
            txtNumero = FindViewById<EditText>(Resource.Id.txt_numero);
            txtComplemento = FindViewById<EditText>(Resource.Id.txt_complemento);
            txtTelefone = FindViewById<EditText>(Resource.Id.txt_telefone);
			btnConfirmar = FindViewById<Button>(Resource.Id.btn_confirmar);
            btnVoltar = FindViewById<Button>(Resource.Id.btn_voltar);

            btnProximo.Click += (object sender, EventArgs e) =>
            {
                Console.WriteLine("clicou próximo");
                p.Nome = txtNome.Text;
                p.Email = txtEmail.Text;
                p.Senha = txtSenha.Text;
                if (txtSenha.Text != txtSenhaRepete.Text)
                {
                    txtSenha.Text = "";
                    txtSenhaRepete.Text = "";
                    Toast.MakeText(this, "Senhas não conferem. Por favor, tente novamente!", ToastLength.Short).Show();
                }
                else
                {
                    llBasico.Visibility = ViewStates.Gone;
                    llEndereco.Visibility = ViewStates.Visible;
                }
            };

			btnConfirmar.Click += (object sender, EventArgs e) =>
			{
                p.Cep = Convert.ToInt32(txtCep.Text);
                p.Estado = txtEstado.Text;
                p.Cidade = txtCidade.Text;
                p.Endereco = txtEndereco.Text;
                p.Numero = Convert.ToInt32(txtNumero.Text);
                p.Bairro = txtBairro.Text;
                if (string.IsNullOrEmpty(txtComplemento.Text))
                    p.Complemento = " ";
                else
                    p.Complemento = txtComplemento.Text;
                p.Telefone = txtTelefone.Text;
                Console.WriteLine("vai entrar cadastro");
                cadastroPessoa(p);
            };

            btnVoltar.Click += (object sender, EventArgs e) =>
            {
                llBasico.Visibility = ViewStates.Visible;
                llEndereco.Visibility = ViewStates.Gone;

                //Redireciona para a página de Login
                //var intent = new Intent(this, typeof(MainActivity));
                //StartActivity(intent);
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
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO pessoa (nome, email, senha, cep, estado, cidade, endereco, numero, complemento, telefone, bairro) VALUES (@nome, @email, @senha, @cep, @estado, @cidade, @endereco, @numero, @complemento, @telefone, @bairro)", con);
                    cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@senha", txtSenha.Text);
                    cmd.Parameters.AddWithValue("@cep", p.Cep);
                    cmd.Parameters.AddWithValue("@estado", p.Estado);
                    cmd.Parameters.AddWithValue("@cidade", p.Cidade);
                    cmd.Parameters.AddWithValue("@endereco", p.Endereco);
                    cmd.Parameters.AddWithValue("@numero", p.Numero);
                    cmd.Parameters.AddWithValue("@complemento", p.Complemento);
                    cmd.Parameters.AddWithValue("@telefone", p.Telefone);
                    cmd.Parameters.AddWithValue("@bairro", p.Bairro);
                    cmd.ExecuteNonQuery();
                    Toast.MakeText(this, "Cadastrado realizado com sucesso.", ToastLength.Short).Show();
                    //Redireciona para a página de Login
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                Toast.MakeText(this, "Usuário ou senha incorreta.", ToastLength.Short).Show();

            }
            finally
            {
                con.Close();
            }
        }
	}
}
