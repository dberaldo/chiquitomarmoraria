
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
using MySql.Data.MySqlClient;
using System.Data;
using Android.Provider;
using Android.Graphics;
using System.IO;
using Android.Content.PM;

namespace ChiquitoMarmoraria
{
	[Activity(Label = "EdicaoMaterial")]
	public class EdicaoMaterial : Activity
	{
		Material m;
		EditText nome;
		EditText categoria;
		EditText descricao;
		EditText preco;
		Button botaoSalvar;
		Button botaoCancelar;
		ImageView imagemEdicao;
		byte[] imagemArray;
		public static readonly int PickImageId = 1000;
		public static readonly int RequestCamera = 2000;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.EdicaoMaterial);
			// Create your application here

			m = new Material();

			m.Id = Intent.GetIntExtra("id", 0);
			m.Nome = Intent.GetStringExtra("nome");
			m.Categoria = Intent.GetStringExtra("categoria");
			m.Descricao = Intent.GetStringExtra("descricao");
			m.Preco = Intent.GetDoubleExtra("preco", 0.00);
			m.Foto = Intent.GetByteArrayExtra("foto");

			nome = FindViewById<EditText>(Resource.Id.editNomeMaterial);
			categoria = FindViewById<EditText>(Resource.Id.editCategoria);
			descricao = FindViewById<EditText>(Resource.Id.editDescricao);
			preco = FindViewById<EditText>(Resource.Id.editPreco);
			imagemEdicao = FindViewById<ImageView>(Resource.Id.imagemViewEdicao);

			if (m.Foto.Length > 0)
			{
				Console.WriteLine("Foto NÃO é null");
				Bitmap bmp = BitmapFactory.DecodeByteArray(m.Foto, 0, m.Foto.Length);

				if(bmp != null)
				{
					imagemEdicao.SetImageBitmap(bmp);
				}

				if (bmp == null)
				{
					Console.WriteLine("ImageData no Detalhes: " + Encoding.Default.GetString(m.Foto));
				}
			}
			else
			{
				Console.WriteLine("Foto é null");
			}

			if (IsThereAnAppToTakePictures())
			{
				CriarDiretorioParaImagens();

				imagemEdicao.Click += (sender, e) =>
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

			botaoSalvar = FindViewById<Button>(Resource.Id.btnSalvarEdicao);
			botaoCancelar = FindViewById<Button>(Resource.Id.btnCancelar);

			nome.Text = m.Nome;
			categoria.Text = m.Categoria;
			descricao.Text = m.Descricao;
			preco.Text = m.Preco.ToString();

			botaoCancelar.Click += (sender, e) => 
			{
				voltar();
			};

			botaoSalvar.Click += (sender, e) => 
			{
				editarMaterial();
			};
		}

		public void editarMaterial()
		{
			MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

			try
			{
				m.Nome = nome.Text;
				m.Categoria = categoria.Text;
				m.Descricao = descricao.Text;
				double precoConvertido;
				if (!Double.TryParse(preco.Text, out precoConvertido))
				{
					Toast.MakeText(this, "Digite apenas números para o preço.", ToastLength.Short).Show();
				}
				else
				{
					m.Preco = precoConvertido;

					if (con.State == ConnectionState.Closed)
					{
						con.Open();
						Console.WriteLine("Conectado com sucesso!");
						MySqlCommand cmd = new MySqlCommand("UPDATE material SET nome = @nome, categoria = @categoria, descricao = @descricao, preco = @preco, foto = @foto WHERE id = @id", con);
						cmd.Parameters.AddWithValue("@id", m.Id);
						cmd.Parameters.AddWithValue("@nome", m.Nome);
						cmd.Parameters.AddWithValue("@categoria", m.Categoria);
						cmd.Parameters.AddWithValue("@descricao", m.Descricao);
						cmd.Parameters.AddWithValue("@preco", m.Preco);
						cmd.Parameters.AddWithValue("@foto", imagemArray);
						cmd.ExecuteNonQuery();
						Toast.MakeText(this, "Edição realizada com sucesso.", ToastLength.Short).Show();
						voltar();
					}
				}
			}
			catch (MySqlException ex)
			{
				Console.WriteLine("MySqlException: " + ex.Message);
			}
			finally
			{
				con.Close();
			}
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
				imagemEdicao.SetImageURI(uri);

				Bitmap foto_bitmap = null;
				foto_bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, uri);

				MemoryStream stream = new MemoryStream();
				foto_bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
				imagemArray = stream.ToArray();
				m.Foto = imagemArray;
			}
			else if (requestCode == RequestCamera)
			{
				Console.WriteLine("Entrou na parte da Camera.");
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
				int width = imagemEdicao.Height;
				App.bitmap = App.arquivo.Path.LoadAndResizeBitmap(width, height);

				Console.WriteLine("Antes do if!!");

				if (App.bitmap != null)
				{
					imagemEdicao.SetImageBitmap(App.bitmap);

					MemoryStream stream = new MemoryStream();
					App.bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
					imagemArray = stream.ToArray();
					m.Foto = imagemArray;

					Console.WriteLine("Transformou em byte array em OnActivityResult!\nByte Array: " + imagemArray);

					App.bitmap = null;
				}
				else
				{
					Console.WriteLine("Não serializou!\nByte Array: " + imagemArray);
				}
				// Dispose of the Java side bitmap.
				GC.Collect();
			}
		}

		public void voltar()
		{
			var intent = new Intent(this, typeof(DetalhesMaterial));
			intent.PutExtra("id", m.Id);
			intent.PutExtra("nome", m.Nome);
			intent.PutExtra("categoria", m.Categoria);
			intent.PutExtra("descricao", m.Descricao);
			intent.PutExtra("preco", m.Preco);
			intent.PutExtra("foto", m.Foto);
			StartActivity(intent);
			Finish();
		}
	}
}
