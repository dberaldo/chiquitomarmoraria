
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

namespace ChiquitoMarmoraria
{
	[Activity(Label = "DetalhesMaterial")]
	public class DetalhesMaterial : Activity
	{
		TextView txtNome;
		TextView txtCategoria;
		TextView txtDescricao;
		TextView txtPreco;
		Button botaoEditarMaterial, botaoExcluirMaterial;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.DetalhesMaterial);
			// Create your application here

			txtNome = FindViewById<TextView>(Resource.Id.txtNome);
			txtCategoria = FindViewById<TextView>(Resource.Id.txtCategoria);
			txtDescricao = FindViewById<TextView>(Resource.Id.txtDescricao);
			txtPreco = FindViewById<TextView>(Resource.Id.txtPreco);
			botaoEditarMaterial = FindViewById<Button>(Resource.Id.btnEditar);
			botaoExcluirMaterial = FindViewById<Button>(Resource.Id.btnRemover);

			int Id = Intent.GetIntExtra("id", 0);
			string nome = Intent.GetStringExtra("nome");
			string categoria = Intent.GetStringExtra("categoria");
			string descricao = Intent.GetStringExtra("descricao");
			double preco = Intent.GetDoubleExtra("preco", 0.00);

			txtNome.Text = nome;
			txtCategoria.Text = categoria;
			txtDescricao.Text = descricao;
			txtPreco.Text = preco.ToString("0.00");

			botaoExcluirMaterial.Click += (sender, e) => 
			{
				DBAdapter db = new DBAdapter(this);

				db.openDB();

				if (db.excluirDados(Id))
				{
					Toast.MakeText(this, "Material excluído com sucesso", ToastLength.Short).Show();
				}

				db.closeDB();
			};
		}


	}
}
