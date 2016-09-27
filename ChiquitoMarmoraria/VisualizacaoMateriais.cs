
using System;
using Android.Database;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "Visualização de Materiais")]
	public class VisualizacaoMateriais : Activity
	{
		Button btnAdd;
		Button btnAtualizar;
		ListView lista;
		JavaList<string> materiaisDisplay = new JavaList<string>();
		Material[] materiais;
		ArrayAdapter adapter;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.VisualizacaoMateriais);
			// Create your application here

			btnAdd = FindViewById<Button>(Resource.Id.btnAdicionar);
			btnAtualizar = FindViewById<Button>(Resource.Id.btnAtualizar);
			lista = FindViewById<ListView>(Resource.Id.listaMateriais);
			adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, materiaisDisplay);

			retrieve();

			lista.ItemClick += (sender, e) => {
				Material m = (Material)materiais[e.Position];
				var intent = new Intent(this, typeof(DetalhesMaterial));
				intent.PutExtra("id", m.Id);
				intent.PutExtra("nome", m.Nome);
				intent.PutExtra("categoria", m.Categoria);
				intent.PutExtra("descricao", m.Descricao);
				intent.PutExtra("preco", m.Preco);

				StartActivity(intent);
			};

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

			materiaisDisplay.Clear();

			while (c.MoveToNext())
			{
				string nomeMaterial = c.GetString(1);
				materiaisDisplay.Add(nomeMaterial);
				Console.WriteLine("Adicionando. MateriaisDisplay: " + materiaisDisplay);
			}

			materiais = InicializarArray<Material>(materiaisDisplay.Size());
			c.MoveToFirst();

			int i = 0;
			do
			{
				materiais[i].Id = c.GetInt(0);
				materiais[i].Nome = c.GetString(1);
				materiais[i].Categoria = c.GetString(2);
				materiais[i].Descricao = c.GetString(3);
				materiais[i].Preco = c.GetDouble(4);
				i++;
			} while (c.MoveToNext());

			if (materiaisDisplay.Size() > 0)
			{
				lista.Adapter = adapter;
			}

			db.closeDB();
		}

		T[] InicializarArray<T>(int length) where T : new()
		{
			T[] array = new T[length];
			for (int i = 0; i < length; ++i)
			{
				array[i] = new T();
			}

			return array;
		}
	}

}
