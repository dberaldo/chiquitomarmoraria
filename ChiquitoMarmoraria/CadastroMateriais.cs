
using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;

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

			buttonCadastrar.Click += (object sender, EventArgs e) =>
			{
                MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        Console.WriteLine("Conectado com sucesso!");

                        Console.WriteLine("antes do comando");
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO material (nome, categoria, descricao, preco) VALUES (@nome, @categoria, @descricao, @preco)", con);
                        cmd.Parameters.AddWithValue("@nome", txtNomeMaterial.Text);
                        cmd.Parameters.AddWithValue("@categoria", txtCategoria.Text);
                        cmd.Parameters.AddWithValue("@descricao", txtDescricao.Text);
                        cmd.Parameters.AddWithValue("@preco", txtPreco.Text);

                        Console.WriteLine("antes do executa");
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
                    Console.WriteLine(ex.Message);
                    Toast.MakeText(this, "Não foi possível cadastrar o material.", ToastLength.Short).Show();
                }
                finally
                {
                    con.Close();
                }

               // con.Close();

            };

            buttonVoltar.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(VisualizacaoMateriais));
                StartActivity(intent);
                Finish();
            };
        }
           
	}
}
