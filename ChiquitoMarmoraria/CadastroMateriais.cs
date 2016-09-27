
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "CadastroMateriais")]
	public class CadastroMateriais : Activity
	{
		EditText txtNomeMaterial;
		EditText txtCategoria;
		EditText txtDescricao;
		EditText txtPreco;
		Button buttonCadastrar;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.CadastroMateriais);
			// Create your application here

			txtNomeMaterial = FindViewById<EditText>(Resource.Id.txtNomeMaterial);
			txtCategoria = FindViewById<EditText>(Resource.Id.txtCategoria);
			txtDescricao = FindViewById<EditText>(Resource.Id.txtDescricao);
			txtPreco = FindViewById<EditText>(Resource.Id.txtPreco);
			buttonCadastrar = FindViewById<Button>(Resource.Id.btnCadastrar);

			buttonCadastrar.Click += (object sender, EventArgs e) =>
			{
				cadastroMaterial(txtNomeMaterial.Text, txtCategoria.Text, txtDescricao.Text, txtPreco.Text);
			};
		}

		public void cadastroMaterial(string nome, string categoria, string descricao, string preco)
		{
			DBAdapter database = new DBAdapter(this);
			database.openDB();

			try
			{
				if (database.inserirDados(nome, categoria, descricao, Convert.ToDouble(preco)))
				{
					txtNomeMaterial.Text = "";
					txtCategoria.Text = "";
					txtDescricao.Text = "";
					txtPreco.Text = "";
					Toast.MakeText(this, "Material cadastrado com sucesso.", ToastLength.Short).Show();
				}
			}
			catch (FormatException f_e)
			{
				Console.WriteLine(f_e.Message);
				Toast.MakeText(this, "Não foi possível cadastrar o material.", ToastLength.Short).Show();
			}

			database.closeDB();
		}
	}
}
