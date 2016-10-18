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

namespace ChiquitoMarmoraria.Resources
{
    [Activity(Label = "DetalhesAgendamento")]
    public class DetalhesAgendamento : Activity
    {

        TextView txtTipo;
        TextView txtData;
        TextView txtStatus;
        Agendamento a;
        Button btnVoltar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DetalhesAgendamento);

            // Create your application here

            txtTipo = FindViewById<TextView>(Resource.Id.txt_tipo);
            txtStatus = FindViewById<TextView>(Resource.Id.txt_status);
            txtData = FindViewById<TextView>(Resource.Id.txt_data);
            btnVoltar = FindViewById<Button>(Resource.Id.btn_voltar);

            int dia = Intent.GetIntExtra("day", 0);
            int mes = Intent.GetIntExtra("month", 0);
            int ano = Intent.GetIntExtra("year", 0);


            a = new Agendamento();
            a.Id = Intent.GetIntExtra("id", 0);
            a.IdServico = Intent.GetIntExtra("idservico", 0);
            a.IdUsuario = Intent.GetIntExtra("idusuario", 0);
            a.Confirmado = Intent.GetIntExtra("status", 0);

            if (a.IdServico == 1)
                txtTipo.Text = "Medição";
            else if (a.IdServico == 2)
                txtTipo.Text = "Entrega";
            else if (a.IdServico == 3)
                txtTipo.Text = "Instalação";

            if (a.Confirmado == -1)
                txtStatus.Text = "Pendente";
            else if (a.Confirmado == 0)
                txtStatus.Text = "Cancelado";
            else if (a.Confirmado == 1)
                txtStatus.Text = "Confirmado";

            txtData.Text = dia + "/" + mes + "/" + ano;

            btnVoltar.Click += (sender, e) =>
            {
                Finish();
            };

            }
    }
}