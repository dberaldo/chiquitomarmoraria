
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
		Material m;
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

			m = new Material();
			m.Id = Intent.GetIntExtra("id", 0);
			m.Nome = Intent.GetStringExtra("nome");
			m.Categoria = Intent.GetStringExtra("categoria");
			m.Descricao = Intent.GetStringExtra("descricao");
			m.Preco = Intent.GetDoubleExtra("preco", 0.00);

			txtNome.Text = m.Nome;
			txtCategoria.Text = m.Categoria;
			txtDescricao.Text = m.Descricao;
			txtPreco.Text = m.Preco.ToString("0.00");

			botaoExcluirMaterial.Click += (sender, e) => 
			{
				DBAdapter db = new DBAdapter(this);

				db.openDB();

				if (db.excluirDados(m.Id))
				{
					Toast.MakeText(this, "Material excluído com sucesso", ToastLength.Short).Show();
				}

				db.closeDB();
			};

			botaoEditarMaterial.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(EdicaoMaterial));
				intent.PutExtra("id", m.Id);
				intent.PutExtra("nome", m.Nome);
				intent.PutExtra("categoria", m.Categoria);
				intent.PutExtra("descricao", m.Descricao);
				intent.PutExtra("preco", m.Preco);

				StartActivity(intent);
			};
		}
	}
}
