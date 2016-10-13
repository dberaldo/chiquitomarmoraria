using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MySql.Data.MySqlClient;
using System.Data;
using ChiquitoMarmoraria.Resources;

namespace ChiquitoMarmoraria
{
    [Activity(Label = "ChiquitoMarmoraria", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        TextView lblCadastre;
        TextView lblEsqueci;
        Button btnEntrar;
        EditText txtLogin;
        EditText txtSenha;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            lblCadastre = FindViewById<TextView>(Resource.Id.lbl_cadastre);
            lblEsqueci = FindViewById<TextView>(Resource.Id.lbl_esqueci);
            btnEntrar = FindViewById<Button>(Resource.Id.btn_entrar);

            txtLogin = FindViewById<EditText>(Resource.Id.txt_login);
            txtSenha = FindViewById<EditText>(Resource.Id.txt_senha);

            //metodo botao cadastre-se
			lblCadastre.Click += (sender, e) =>
			{
				//Redireciona para a página Cadastro Login
				var intent = new Intent(this, typeof(CadastroLogin));
                StartActivity(intent);
			};

            //metodo botao entrar
            btnEntrar.Click += (sender, e) =>
            {
                if (txtLogin.Text.Length <= 0)
                {
                    Toast.MakeText(this, "Campo Login não pode estar em branco!", ToastLength.Short).Show();
                }
                else if (txtSenha.Text.Length <= 0)
                {
                    Toast.MakeText(this, "Campo Senha não pode estar em branco!", ToastLength.Short).Show();
                }
                else
                {
                    MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=ufscarpds;User Id=ufscarpds;Password=19931993;charset=utf8");
                    //MySqlConnection con = new MySqlConnection("Server=localhost;Port=3306;database=pds2016;User Id=root;Password=;charset=utf8");

                    try
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                            Console.WriteLine("Conectado com sucesso!");
                            MySqlCommand cmd = new MySqlCommand("Select email, id, nome, senha from pessoa where email=@email;", con);
                            cmd.Parameters.AddWithValue("@email", txtLogin.Text);
                            //cmd.ExecuteNonQuery();
                            
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                Console.WriteLine("Passou execute reader!");
                                if (reader.Read())
                                {
                                    Console.WriteLine("Passou REad reader!");
                                    var email = reader["email"].ToString();
                                    Console.WriteLine("Email= "+email);
                                    if (email.Equals(txtLogin.Text, StringComparison.Ordinal))
                                    {

                                        Console.WriteLine("Entrou Primeiro IF!");
                                        Console.WriteLine(reader.GetString(0));
                                        Console.WriteLine(reader.GetString(1));
                                        Console.WriteLine(reader.GetString(2));
                                        Console.WriteLine(reader.GetString(3));
                                        var senha = reader.GetString(3);

                                        if (senha.Equals(txtSenha.Text, StringComparison.Ordinal))
                                        {
                                            Console.WriteLine(reader.GetString(0));
                                            Console.WriteLine(reader.GetString(1));
                                            Console.WriteLine(reader.GetString(2));
                                            Console.WriteLine(reader.GetString(3));

                                            if (email.Equals("leonardo.g.chiquito@gmail.com", StringComparison.Ordinal))
                                            {
                                                //chama tela de adm
                                                entrarAdm(reader.GetString(1));

                                            }
                                            else
                                            {
                                                //chama tela de user
                                                entrarUser(reader.GetString(1));
                                            }

                                        }
                                        else
                                        {
                                            Toast.MakeText(this, "Usuário ou senha incorreta.", ToastLength.Short).Show();
                                            txtLogin.Text = "";
                                            txtSenha.Text = "";
                                        }
                                    }
                                    else
                                    {
                                        Toast.MakeText(this, "Usuário ou senha incorreta.", ToastLength.Short).Show();
                                        txtLogin.Text = "";
                                        txtSenha.Text = "";
                                    }

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
            };
        }

        public void entrarAdm(string id)
        {
			//Redireciona para a tela de Menu do administrador
			var intent = new Intent(this, typeof(MenuAdministrador));
            intent.PutExtra("id", id);
			StartActivity(intent);
            Finish();
            
        }

        public void entrarUser(string id)
        {

            //Redireciona para a tela de Menu do usuário
            var intent = new Intent(this, typeof(MenuUsuario));
            intent.PutExtra("id", id);
            StartActivity(intent);
            Finish();
            
        }

    }
}

