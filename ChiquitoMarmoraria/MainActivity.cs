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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
        }

        public void entrar(View v)
        {
            
            
        }

        public void cadastro(View v)
        {
            SetContentView(Resource.Layout.cadastro_login);
        }
    }
}

