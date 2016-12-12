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

namespace ChiquitoMarmoraria.Resources
{
    [Activity(Label = "EdicaoAgendamentoUsuario")]
    public class EdicaoAgendamentoUsuario : Activity
    {

        EditText txtData;
        TextView lblTipo;
        ImageButton btnCalendario;
        Button btnSalvar;
        Button btnCancelar;


        int day = 0;
        int month = 0;
        int year = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EdicaoAgendamentoUsuario);

            int id = Intent.GetIntExtra("id", 0);
            int tipo = Intent.GetIntExtra("tipo", 0);
            int dia = Intent.GetIntExtra("dia", 0);
            int mes = Intent.GetIntExtra("mes", 0);
            int ano = Intent.GetIntExtra("ano", 0);
            int status = Intent.GetIntExtra("status", 0);


            string idString = Intent.GetStringExtra("idusuario") ?? "Data not available";
            int idUsuario = Int32.Parse(idString);
            

            day = dia;
            month = mes;
            year = ano;

            txtData = FindViewById<EditText>(Resource.Id.txt_displayData);
            lblTipo = FindViewById<TextView>(Resource.Id.txt_tipo);
            btnCalendario = FindViewById<ImageButton>(Resource.Id.img_calendario);
            btnCalendario.Click += DateSelect_OnClick;
            btnSalvar = FindViewById<Button>(Resource.Id.btn_salvar);
            btnCancelar = FindViewById<Button>(Resource.Id.btn_cancelar);

            if (tipo == 1)
                lblTipo.Text = "Medição";
            else if (tipo == 2)
                lblTipo.Text = "Entrega";
            else if (tipo == 3)
                lblTipo.Text = "Instalação";

            txtData.Text = dia + "/" + mes + "/" + ano;

            btnCancelar.Click += (object sender, EventArgs e) =>
            {
                //tem ctz que deseja sair sem salvar as alterações?
                new AlertDialog.Builder(this)
                    .SetPositiveButton("Sim", (sender2, args) =>
                    {
                        // User pressed yes
                        var intent = new Intent(this, typeof(DetalhesAgendamento));
                        intent.PutExtra("id", id);
                        intent.PutExtra("idservico", tipo);
                        intent.PutExtra("idusuario", idString);
                        intent.PutExtra("day", dia);
                        intent.PutExtra("month", mes);
                        intent.PutExtra("year", ano);
                        intent.PutExtra("status", status);

                        StartActivity(intent);
                        Finish();
                    })
                    .SetNegativeButton("Não", (sender2, args) =>
                    {
                        // User pressed no 
                    })
                    .SetMessage("Tem certeza que deseja sair sem salvar as alterações?")
                    .SetTitle("Alerta")
                    .Show();
            };

            btnSalvar.Click += (object sender, EventArgs e) =>
            {

                DateTime data = new DateTime(year, month, day);

                MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        Console.WriteLine("Conectado com sucesso EDICAO Agendamento Usuario!");
                        MySqlCommand cmd = new MySqlCommand("UPDATE agendamento SET data = @data, confirmado=-1, needNotifyAdmin=1 WHERE id = @id", con);
                        cmd.Parameters.AddWithValue("@data", data);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                        
                        Toast.MakeText(this, "Agendamento atualizado com sucesso!", ToastLength.Short).Show();

                        status = -1;

                        var intent = new Intent(this, typeof(DetalhesAgendamento));
                        intent.PutExtra("id", id);
                        intent.PutExtra("idservico", tipo);
                        intent.PutExtra("idusuario", idString);
                        intent.PutExtra("day", day);
                        intent.PutExtra("month", month);
                        intent.PutExtra("year", year);
                        intent.PutExtra("status", status);
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


            };
        }

        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                txtData.Text = String.Format("{0:d/M/yyyy}", time);

                day = time.Day;
                month = time.Month;
                year = time.Year;

            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
    }
}