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
    [Activity(Label = "DetalhesAgendamento")]
    public class DetalhesAgendamento : Activity
    {

        TextView lblTipo;
        TextView lblData;
        TextView lblStatus;
        Agendamento a;
        Button btnVoltar;
        Button btnAlterar;
        Button btnCancelar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DetalhesAgendamento);

            // Create your application here

            lblTipo = FindViewById<TextView>(Resource.Id.txt_tipo);
            lblStatus = FindViewById<TextView>(Resource.Id.txt_status);
            lblData = FindViewById<TextView>(Resource.Id.txt_data);
            btnVoltar = FindViewById<Button>(Resource.Id.btn_voltar);
            btnAlterar = FindViewById<Button>(Resource.Id.btn_alterar);
            btnCancelar = FindViewById<Button>(Resource.Id.btn_cancelar);

            int dia = Intent.GetIntExtra("day", 0);
            int mes = Intent.GetIntExtra("month", 0);
            int ano = Intent.GetIntExtra("year", 0);
            
            a = new Agendamento();
            a.Id = Intent.GetIntExtra("id", 0);
            a.IdServico = Intent.GetIntExtra("idservico", 0);
            a.IdUsuario = Intent.GetIntExtra("idusuario", 0);
            a.Confirmado = Intent.GetIntExtra("status", 0);

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

            btnAlterar.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(EdicaoAgendamentoUsuario));
                intent.PutExtra("id", a.Id);
                intent.PutExtra("tipo", a.IdServico);
                intent.PutExtra("dia", dia);
                intent.PutExtra("mes", mes);
                intent.PutExtra("ano", ano);
                intent.PutExtra("idusuario", a.IdUsuario);

                StartActivity(intent);
                Finish();
            };

            btnCancelar.Click += (sender, e) =>
            {
                new AlertDialog.Builder(this)
                    .SetPositiveButton("Sim", (sender2, args) =>
                    {
                    // User pressed yes
                    //Chamar função de deletar agendamento
                    //delete from agendamento where id=@id

                    MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

                        try
                        {
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                                Console.WriteLine("Conectado com sucesso CANCELAMENTO Agendamento Usuario!");
                                MySqlCommand cmd = new MySqlCommand("DELETE agendamento WHERE id = @id", con);
                                cmd.Parameters.AddWithValue("@id", a.Id);
                                cmd.ExecuteNonQuery();

                                Toast.MakeText(this, "Agendamento cancelado com sucesso!", ToastLength.Short).Show();

                                var intent = new Intent(this, typeof(MeusAgendamentos));
                                intent.PutExtra("id", a.IdUsuario);
                                StartActivity(intent);
                                Finish();
                            }
                        }
                        catch (MySqlException ex)
                        {
                            Console.WriteLine(ex.Message);
                            Toast.MakeText(this, "Erro ao atualizar agendamento!", ToastLength.Short).Show();

                        }
                        finally
                        {
                            con.Close();
                        }

                    })
                    .SetNegativeButton("Não", (sender2, args) =>
                    {
                        // User pressed no 
                    })
                    .SetMessage("Tem certeza que deseja cancelar esse agendamento?")
                    .SetTitle("Alerta")
                    .Show();
            };

        }
    }
}