using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "MenuAdministrador")]
	public class MenuAdministrador : Activity
	{
		Button btnMateriais;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.MenuAdministrador);
			btnMateriais = FindViewById<Button>(Resource.Id.btnMateriais);

			btnMateriais.Click += (sender, e) =>
			{
				Console.WriteLine("Chamou evento.");
				//Redireciona para a página CadastroMateriais
				var intent = new Intent(this, typeof(CadastroMateriais));
				StartActivity(intent);
				Console.WriteLine("Redirecionou!");
			};

		}
	}
}
