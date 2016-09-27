using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MySql.Data.MySqlClient;
using System.Data;

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
                else if (txtLogin.Text.Length <= 0)
                {
                    Toast.MakeText(this, "Campo Senha não pode estar em branco!", ToastLength.Short).Show();
                }
                else
                {

					//Redireciona para a página Cadastro Login
					/*var intent = new Intent(this, typeof(CadastroLogin));
                    StartActivity(intent);*/
					entrar();
                }
            };
        }

        public void entrar()
        {
			//Redireciona para a parte de CRUD de Materiais do administrador só para fins de testes.
			var intent = new Intent(this, typeof(MenuAdministrador));
			StartActivity(intent);
        }

       
}
}

