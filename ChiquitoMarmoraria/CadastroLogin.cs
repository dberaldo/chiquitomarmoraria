
using System;
using Android.App;
using Android.OS;
using Android.Widget;

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

			//Este comando liga o layout à sua respectiva activity
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
                    //erro
                }
                else
                {
                    cadastroPessoa(txtNome.Text, txtEmail.Text, txtSenha.Text);
                }

            };
		}

        public void cadastroPessoa(string nome, string email, string senha)
        {
            DBAdapter database = new DBAdapter(this);
            database.openDB();

            try
            {
                if (database.inserirPessoa(nome, email, senha))
                {
                    txtNome.Text = "";
                    txtEmail.Text = "";
                    txtSenha.Text = "";
                    txtSenhaRepete.Text = "";
                    Toast.MakeText(this, "Cadastrado realizado com sucesso.", ToastLength.Short).Show();
                }
            }
            catch (FormatException f_e)
            {
                Console.WriteLine(f_e.Message);
                Toast.MakeText(this, "Não foi possível realizar o cadastro!", ToastLength.Short).Show();
            }
            database.closeDB();
        }

		
	}
}
