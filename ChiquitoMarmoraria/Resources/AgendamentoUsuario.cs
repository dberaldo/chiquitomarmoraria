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

namespace ChiquitoMarmoraria.Resources
{
    [Activity(Label = "Agendamento")]
    public class AgendamentoUsuario : Activity
    {

        CalendarView calendario;
        Button btnAgendar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AgendamentoUsuario);

            calendario = FindViewById<CalendarView>(Resource.Id.calendario);
            btnAgendar = FindViewById<Button>(Resource.Id.btn_agendar);

            int day=0;
            int month=0;
            int year=0;

            calendario.DateChange += (s, e) => {
                day = e.DayOfMonth;
                month = e.Month;
                year = e.Year;
            };

            btnAgendar.Click += (object sender, EventArgs e) =>
            {
                if (day == 0 && month == 0 && year == 0)
                {
                    //Exibir mensagem de erro "Escolha uma data"
                }
                else
                {
                    
                    //Cadastrar agendamento no banco
                    //table deve ter uma coluna boolean "confirmado"
                    //quando o usuario envia o agendamento a coluna confirmado deve ter valor false
                    //somente terá valor true quando o adm confirmar a solicitacao de agendamento

                // apos cadastrar o agendamento no banco exibir uma mensaeg para o usaurio
                // informando que a solicitaçao está pendente e que quando for confirmada uma mensagem será enviada
                // ou algo do tipo

                }
            };
        }
    }
}