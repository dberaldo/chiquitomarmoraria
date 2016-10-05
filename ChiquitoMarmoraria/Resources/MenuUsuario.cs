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
using ChiquitoMarmoraria.Resources;

namespace ChiquitoMarmoraria.Resources
{
    [Activity(Label = "Cadastro")]
    public class MenuUsuario : Activity
    {
        TextView lblBemvindo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MenuUsuario);
            lblBemvindo = FindViewById<TextView>(Resource.Id.lbl_bemvindo);
            string id = Intent.GetStringExtra("id") ?? "Data not available";

            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=ufscarpds;User Id=ufscarpds;Password=19931993;charset=utf8");

            string email="";
            string nome="";
            string senha="";

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso!");

                }

                MySqlCommand cmd = new MySqlCommand("Select email, id, nome, senha from pessoa where id=@id;", con);
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Passou execute reader!");
                    if (reader.Read())
                    {
                        Console.WriteLine("Passou REad reader!");
                        email = reader["email"].ToString();
                        nome = reader["nome"].ToString();
                        senha = reader["senha"].ToString();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }

            lblBemvindo.Text = "Bem-vindo(a), " + nome + "!";

        }
    }
}