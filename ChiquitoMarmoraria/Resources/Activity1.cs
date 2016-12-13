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
using ChiquitoMarmoraria.Resources.Model;
using MySql.Data.MySqlClient;
using System.Data;

namespace ChiquitoMarmoraria.Resources
{
    [Activity(Label = "Activity1")]
    public class Activity1 : Activity
    {

        TextView lblTipo;
        TextView lblData;
        TextView lblStatus;
        TextView txtNome;
        TextView txtEndereco;
        TextView txtCidade;
        TextView txtNumero;
        TextView txtTelefone;

        Agendamento a;
        Button btnVoltar;
        Button btnAlterar;
        Button btnCancelar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Activity1);

            // Create your application here

            lblTipo = FindViewById<TextView>(Resource.Id.txt_tipo);
            lblStatus = FindViewById<TextView>(Resource.Id.txt_status);
            lblData = FindViewById<TextView>(Resource.Id.txt_data);
            txtNome = FindViewById<TextView>(Resource.Id.txt_nome);
            txtEndereco = FindViewById<TextView>(Resource.Id.txt_endereco);
            txtCidade = FindViewById<TextView>(Resource.Id.txt_cidade);
            txtNumero = FindViewById<TextView>(Resource.Id.txt_numero);
            txtTelefone = FindViewById<TextView>(Resource.Id.txt_telefone);
            btnVoltar = FindViewById<Button>(Resource.Id.btn_voltar);
            btnAlterar = FindViewById<Button>(Resource.Id.btn_alterar);
            btnCancelar = FindViewById<Button>(Resource.Id.btn_cancelar);

            int dia = Intent.GetIntExtra("day", 0);
            int mes = Intent.GetIntExtra("month", 0);
            int ano = Intent.GetIntExtra("year", 0);

            //string idString = Intent.GetStringExtra("idusuario") ?? "Data not available";
            // Console.WriteLine("PROBLEM: " + idString);
            int idInt = Intent.GetIntExtra("idusuario", 0); //Int32.Parse(idString);
            string idString = "" + idInt;




            a = new Agendamento();
            a.Id = Intent.GetIntExtra("id", 0);
            a.IdServico = Intent.GetIntExtra("idservico", 0);
            a.IdUsuario = idInt;
            a.Confirmado = Intent.GetIntExtra("status", 0);


            Console.WriteLine("DetalhesAgendamento ID = " + a.IdUsuario);



            MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

            string endereco = "";
            string nome = "";
            int numero = 0;
            string cidade = "";
            string complemento = "";
            string telefone = "";

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso!");

                }

                MySqlCommand cmd = new MySqlCommand("Select nome, endereco, numero, cidade, telefone, complemento from pessoa where id=@id;", con);
                cmd.Parameters.AddWithValue("@id", a.IdUsuario.ToString());

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Passou execute reader!");
                    if (reader.Read())
                    {

                        numero = reader.GetOrdinal("numero");
                        nome = reader["nome"].ToString();
                        endereco = reader["endereco"].ToString();
                        cidade = reader["cidade"].ToString();
                        complemento = reader["complemento"].ToString();
                        telefone = reader["telefone"].ToString();
                        Console.WriteLine("Passou REad reader!");
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

            txtNome.Text = nome;
            txtEndereco.Text = endereco + ", " + numero.ToString();
            txtCidade.Text = cidade;
            txtTelefone.Text = telefone;



            if (a.IdServico == 1)
                lblTipo.Text = "Medicao";
            else if (a.IdServico == 2)
                lblTipo.Text = "Entrega";
            else if (a.IdServico == 3)
                lblTipo.Text = "Instalacao";

            if (a.Confirmado == -1)
                lblStatus.Text = "Pendente";
            else if (a.Confirmado == 0)
                lblStatus.Text = "Cancelado";
            else if (a.Confirmado == 1)
                lblStatus.Text = "Confirmado";

            lblData.Text = dia + "/" + mes + "/" + ano;

            btnVoltar.Click += (sender, e) =>
            {
                Finish();
            };

           

        }
    }
}