using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

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

                }
                else if (txtLogin.Text.Length <= 0)
                {

                }
                else
                {

                    //Redireciona para a página Cadastro Login
                    var intent = new Intent(this, typeof(CadastroLogin));
                    StartActivity(intent);
                }
                
            };

        }

        public void entrar(View v)
        {
            
            
        }

       
}
}

