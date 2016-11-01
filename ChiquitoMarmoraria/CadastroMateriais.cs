
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using Java.IO;
using Android.Content.PM;
using Android.Graphics;
using Android.Provider;
using System.Collections.Generic;

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
        Button buttonVoltar;
		public static readonly int PickImageId = 1000;
		public static readonly int RequestCamera = 2000;
		ImageView imageMaterial;
		byte[] imagemArray;

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
            buttonVoltar = FindViewById<Button>(Resource.Id.btnVoltar);

			if (IsThereAnAppToTakePictures())
			{
				CriarDiretorioParaImagens();

				imageMaterial = FindViewById<ImageView>(Resource.Id.imageViewMaterial);

				imageMaterial.Click += (sender, e) =>
				{
					//set alert for executing the task
					String[] items = { "Galeria", "Camera", "Cancelar" };
					AlertDialog.Builder alert = new AlertDialog.Builder(this);
					alert.SetTitle("Selecionar foto:");
					alert.SetItems(items, (d, args) =>
					{
						if (args.Which == 0)
						{
							escolherImagemGaleria();
						}
						else if (args.Which == 1)
						{
							escolherImagemCamera();
						}

					});

					Dialog dialog = alert.Create();
					dialog.Show();
				};
			}


			buttonCadastrar.Click += (object sender, EventArgs e) =>
			{
                MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        System.Console.WriteLine("Conectado com sucesso!");

                        System.Console.WriteLine("antes do comando");
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO material (nome, categoria, descricao, preco, foto) VALUES (@nome, @categoria, @descricao, @preco, @imagem)", con);
                        cmd.Parameters.AddWithValue("@nome", txtNomeMaterial.Text);
                        cmd.Parameters.AddWithValue("@categoria", txtCategoria.Text);
                        cmd.Parameters.AddWithValue("@descricao", txtDescricao.Text);
                        cmd.Parameters.AddWithValue("@preco", txtPreco.Text);
						cmd.Parameters.AddWithValue("@imagem", imagemArray);

                        System.Console.WriteLine("antes do executa");
                        cmd.ExecuteNonQuery();
                        txtNomeMaterial.Text = "";
                        txtCategoria.Text = "";
                        txtDescricao.Text = "";
                        txtPreco.Text = "";
                        Toast.MakeText(this, "Material cadastrado com sucesso.", ToastLength.Short).Show();
                    }
                }
                catch (MySqlException ex)
                {
                    System.Console.WriteLine(ex.Message);
                    Toast.MakeText(this, "Não foi possível cadastrar o material.", ToastLength.Short).Show();
                }
                finally
                {
                    con.Close();
                }
            };

            buttonVoltar.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(VisualizacaoMateriais));
                StartActivity(intent);
                Finish();
            };
        }

		public void escolherImagemGaleria()
		{
			Intent = new Intent();
			Intent.SetType("image/*");
			Intent.SetAction(Intent.ActionGetContent);
			StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
		}

		public void escolherImagemCamera()
		{
			var intent = new Intent(MediaStore.ActionImageCapture);
			App.arquivo = new Java.IO.File(App.diretorio, String.Format("karma_{0}.jpg", Guid.NewGuid()));
			intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App.arquivo));
			this.StartActivityForResult(intent, RequestCamera);
		}

		private void CriarDiretorioParaImagens()
		{
			App.diretorio = new Java.IO.File(
				Android.OS.Environment.GetExternalStoragePublicDirectory(
					Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
			if (!App.diretorio.Exists())
			{
				App.diretorio.Mkdirs();
			}
		}

		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities =
				PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
			{
				Android.Net.Uri uri = data.Data;
				imageMaterial.SetImageURI(uri);

				Bitmap foto_bitmap = null;
				foto_bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, uri);

				MemoryStream stream = new MemoryStream();
				foto_bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
				imagemArray = stream.ToArray();
			}
			else if(requestCode == RequestCamera)
			{
				base.OnActivityResult(requestCode, resultCode, data);

				// Make it available in the gallery

				Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
				Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App.arquivo);
				mediaScanIntent.SetData(contentUri);
				SendBroadcast(mediaScanIntent);

				// Display in ImageView. We will resize the bitmap to fit the display.
				// Loading the full sized image will consume to much memory
				// and cause the application to crash.

				int height = Resources.DisplayMetrics.HeightPixels;
				int width = imageMaterial.Height;
				App.bitmap = App.arquivo.Path.LoadAndResizeBitmap(width, height);

				if (App.bitmap != null)
				{
					imageMaterial.SetImageBitmap(App.bitmap);
					App.bitmap = null;
				}

				// Dispose of the Java side bitmap.
				GC.Collect();
			}
		}  
	}

	public static class App
	{
		public static Java.IO.File arquivo;
		public static Java.IO.File diretorio;
		public static Bitmap bitmap;
	}
}
