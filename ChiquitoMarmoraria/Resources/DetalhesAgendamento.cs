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

            };

        }
    }
}