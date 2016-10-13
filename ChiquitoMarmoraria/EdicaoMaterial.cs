
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

			nome = FindViewById<EditText>(Resource.Id.editNomeMaterial);
			categoria = FindViewById<EditText>(Resource.Id.editCategoria);
			descricao = FindViewById<EditText>(Resource.Id.editDescricao);
			preco = FindViewById<EditText>(Resource.Id.editPreco);
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
			MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=ufscarpds;User Id=ufscarpds;Password=19931993;charset=utf8");

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
						MySqlCommand cmd = new MySqlCommand("UPDATE material SET nome = @nome, categoria = @categoria, descricao = @descricao, preco = @preco WHERE id = @id", con);
						cmd.Parameters.AddWithValue("@id", m.Id);
						cmd.Parameters.AddWithValue("@nome", m.Nome);
						cmd.Parameters.AddWithValue("@categoria", m.Categoria);
						cmd.Parameters.AddWithValue("@descricao", m.Descricao);
						cmd.Parameters.AddWithValue("@preco", m.Preco);
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

		public void voltar()
		{
			Finish();
		}
	}
}
