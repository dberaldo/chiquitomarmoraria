
using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "Menu")]
	public class MenuAdministrador : Activity
	{
		Button btnCadastroMaterial;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.MenuAdministrador);
			// Create your application here

			btnCadastroMaterial = FindViewById<Button>(Resource.Id.btnCadastrarMaterial);

			btnCadastroMaterial.Click += (sender, e) => 
			{
				var intent = new Intent(this, typeof(VisualizacaoMateriais));
				StartActivity(intent);
			};
		}
	}
}
