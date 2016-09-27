
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Database;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "Visualização de Materiais")]
	public class VisualizacaoMateriais : Activity
	{
		Button btnAdd;
		Button btnAtualizar;
		ListView lista;
		JavaList<String> materiais = new JavaList<string>();
		ArrayAdapter adapter;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.VisualizacaoMateriais);
			// Create your application here

			btnAdd = FindViewById<Button>(Resource.Id.btnAdicionar);
			btnAtualizar = FindViewById<Button>(Resource.Id.btnAtualizar);
			lista = FindViewById<ListView>(Resource.Id.listaMateriais);
			adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, materiais);

			retrieve();

			btnAdd.Click += (sender, e) => 
			{
				var intent = new Intent(this, typeof(CadastroMateriais));
				StartActivity(intent);
			};

			btnAtualizar.Click += (sender, e) => 
			{ 
				retrieve();
			};
		}

		public void retrieve()
		{
			DBAdapter db = new DBAdapter(this);
			db.openDB();

			ICursor c = db.recuperarDados();

			materiais.Clear();

			while (c.MoveToNext())
			{
				string nomeMaterial = c.GetString(1);
				materiais.Add(nomeMaterial);
			}

			if (materiais.Size() > 0)
			{
				lista.Adapter = adapter;
			}

			db.closeDB();
		}
	}

}
