using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Json;
using ChiquitoMarmoraria.Resources;
using Android.Content.Res;
using Xamarin.Auth;
using Newtonsoft.Json;

namespace ChiquitoMarmoraria
{
    [Activity(Label = "ChiquitoMarmoraria", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        TextView lblCadastre;
        TextView lblEsqueci;
        Button btnEntrar;
        Button btnFacebook;
        EditText txtLogin;
        EditText txtSenha;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            lblCadastre = FindViewById<TextView>(Resource.Id.lbl_cadastre);
            lblEsqueci = FindViewById<TextView>(Resource.Id.lbl_esqueci);
            btnEntrar = FindViewById<Button>(Resource.Id.btn_entrar);
            btnFacebook = FindViewById<Button>(Resource.Id.btn_facebook);

            txtLogin = FindViewById<EditText>(Resource.Id.txt_login);
            txtSenha = FindViewById<EditText>(Resource.Id.txt_senha);

          

            //metodo botao cadastre-se
            lblCadastre.Click += (sender, e) =>
			{
				//Redireciona para a página Cadastro Login
				var intent = new Intent(this, typeof(CadastroLogin));
                StartActivity(intent);
			};

            //metodo esqueceu senha
            lblEsqueci.Click += (sender, e) =>
            {
                //Redireciona para a página Esqueci Senha
                var intent = new Intent(this, typeof(EsqueciSenha));
                StartActivity(intent);
            };

            //metodo botao facebook
            btnFacebook.Click += (sender, e) =>
            {
                Console.WriteLine("1");

                var auth = new OAuth2Authenticator("383371458662674", "email", new Uri("https://m.facebook.com/dialog/oauth/"), new Uri("http://www.facebook.com/connect/login_success.html"));

                Console.WriteLine("2");

                auth.Completed += (authSender, eventArgs) =>
                {
                    Console.WriteLine("complete");
                    if (eventArgs.IsAuthenticated)
                    {
                        // autenticou
                        Account fbAccount = eventArgs.Account;
                        Console.WriteLine("Autenticado! /o\\");

                        var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=email,id,name"), null, eventArgs.Account);
                        request.GetResponseAsync().ContinueWith(t => {
                            if (t.IsFaulted)
                                Console.WriteLine("Error: " + t.Exception.InnerException.Message);
                            else
                            {
                                string json = t.Result.GetResponseText();
                                // Console.WriteLine(json);

                                var info = JsonValue.Parse(json);

                                string facebookEmail = info["email"];
                                string facebookID = info["id"];
                                string facebookName = info["name"];

                                Console.WriteLine(facebookEmail);
                                Console.WriteLine(facebookName);
                                Console.WriteLine(facebookID);

                                MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

                                try
                                {
                                    if (con.State == ConnectionState.Closed)
                                    {
                                        con.Open();
                                        Console.WriteLine("Conectado com sucesso!");
                                        MySqlCommand cmd = new MySqlCommand("Select email, id, nome, senha from pessoa where facebook=@facebookID;", con);
                                        cmd.Parameters.AddWithValue("@facebookID", facebookID);
                                        //cmd.ExecuteNonQuery();

                                        using (MySqlDataReader reader = cmd.ExecuteReader())
                                        {
                                            Console.WriteLine("Passou execute reader!");
                                            if (reader.Read())
                                            {
                                                Console.WriteLine("Passou Read reader!");
                                                var email = reader["email"].ToString();
                                                Console.WriteLine("Email= " + email);

                                                Console.WriteLine(reader.GetString(0));
                                                Console.WriteLine(reader.GetString(1));
                                                Console.WriteLine(reader.GetString(2));
                                                Console.WriteLine(reader.GetString(3));
                                                var senha = reader.GetString(3);

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
                                                // Primeiro acesso, redirecionar para o cadastro com dados importados já preenchidos
                                                var intentCadastro = new Intent(this, typeof(CadastroLogin));
                                                intentCadastro.PutExtra("facebookEmail", facebookEmail);
                                                intentCadastro.PutExtra("facebookName", facebookName);
                                                intentCadastro.PutExtra("facebookID", facebookID);
                                                StartActivity(intentCadastro);
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
                        });
                    }
                    else
                    {
                        // cancelou
                    }
                };

                var intent = auth.GetUI(this);
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
                    MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");
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
            Console.WriteLine("MainActivity ID = " + id);
            StartActivity(intent);
			Finish();
		}
    }
}

