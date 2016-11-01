
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
using Android.Graphics;
using System.Drawing;
using System.IO;
using System.Windows;

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
		Button botaoEditarMaterial, botaoExcluirMaterial, botaoVoltar;
		ImageView imagemDetalhes;

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
			botaoVoltar = FindViewById<Button>(Resource.Id.btnVoltar);
			imagemDetalhes = FindViewById<ImageView>(Resource.Id.imageViewDetalhes);
				
			m = new Material();
			m.Id = Intent.GetIntExtra("id", 0);
			m.Nome = Intent.GetStringExtra("nome");
			m.Categoria = Intent.GetStringExtra("categoria");
			m.Descricao = Intent.GetStringExtra("descricao");
			m.Preco = Intent.GetDoubleExtra("preco", 0.00);
			m.Foto = Intent.GetByteArrayExtra("foto");

			Console.WriteLine("Length dos bytes: " + m.Foto.Length);

			if (m.Foto.Length > 0)
			{
				Console.WriteLine("Foto NÃO é null");
				Bitmap bmp = BitmapFactory.DecodeByteArray(m.Foto, 0, m.Foto.Length);

				if(bmp == null)
				{
					Console.WriteLine("ImageData no Detalhes: " + Encoding.Default.GetString(m.Foto));
					Console.WriteLine("Nao deu pra carregar o bitmap.");
				}
				else
				{
					Console.WriteLine("Deu pra carregar o bitmap.");
				}

				imagemDetalhes.SetImageBitmap(bmp);
			}
			else
			{
				Console.WriteLine("Foto é null");
			}

			txtNome.Text = m.Nome;
			txtCategoria.Text = m.Categoria;
			txtDescricao.Text = m.Descricao;
			txtPreco.Text = m.Preco.ToString("0.00");

			botaoExcluirMaterial.Click += (sender, e) => 
			{
				MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

				try
				{
					if (con.State == ConnectionState.Closed)
					{
						con.Open();
						Console.WriteLine("Conectado com sucesso!");
						MySqlCommand cmd = new MySqlCommand("DELETE FROM material WHERE id = @id", con);
						cmd.Parameters.AddWithValue("@id", m.Id);
						cmd.ExecuteNonQuery();
						Toast.MakeText(this, "Material excluído com sucesso.", ToastLength.Short).Show();
						voltar();
					}
				}
				catch (MySqlException ex)
				{
					Console.WriteLine("MySqlException: " + ex.Message);
					Toast.MakeText(this, "Não foi possível excluir o material", ToastLength.Short).Show();
				}
				finally
				{
					con.Close();
				}
			};

			botaoEditarMaterial.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(EdicaoMaterial));
				intent.PutExtra("id", m.Id);
				intent.PutExtra("nome", m.Nome);
				intent.PutExtra("categoria", m.Categoria);
				intent.PutExtra("descricao", m.Descricao);
				intent.PutExtra("preco", m.Preco);
				intent.PutExtra("foto", m.Foto);

				StartActivity(intent);
				Finish();
			};

			botaoVoltar.Click += (sender, e) => 
			{
				voltar();
			};
		}

		public void voltar()
		{
			var intent = new Intent(this, typeof(VisualizacaoMateriais));
			StartActivity(intent);
			Finish();
		}
	}
}
