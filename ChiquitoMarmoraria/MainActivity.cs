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
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.CadastroMateriais);
			Console.WriteLine("Chamei o MenuAdministrador.");
		}
        
    }
}

