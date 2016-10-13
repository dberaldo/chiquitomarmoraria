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
        Button btnCancelar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AgendamentoUsuario);

            string id = Intent.GetStringExtra("id") ?? "Data not available";

            calendario = FindViewById<CalendarView>(Resource.Id.calendario);
            btnAgendar = FindViewById<Button>(Resource.Id.btn_agendar);
            btnCancelar = FindViewById<Button>(Resource.Id.btn_cancelar);

            int day=0;
            int month=0;
            int year=0;

            calendario.DateChange += (s, e) => {
                day = e.DayOfMonth;
                month = e.Month;
                year = e.Year;
            };

            btnCancelar.Click += (object sender, EventArgs e) =>
            {
                new AlertDialog.Builder(this)
                    .SetPositiveButton("Sim", (sender2, args) =>
                    {
                        // User pressed yes
                        Finish();
                    })
                    .SetNegativeButton("Não", (sender2, args) =>
                    {
                        // User pressed no 
                    })
                    .SetMessage("Tem certeza que deseja cancelar a solicitação?")
                    .SetTitle("Alerta")
                    .Show();
            };


            btnAgendar.Click += (object sender, EventArgs e) =>
            {
                if (day == 0 && month == 0 && year == 0)
                {
                    //Exibir mensagem de erro "Escolha uma data"
                }
                else
                {

                    //Cadastrar agendamento no banco (TABLE Agendamento columns id, data, tipo_servico, id_usuario, confirmado)
                    //table deve ter uma coluna boolean "confirmado"
                    //tipo_servico = 1-Medição, 2-Entrega, 3-Instalação
                    //quando o usuario envia o agendamento a coluna confirmado deve ter valor false
                    //somente terá valor true quando o adm confirmar a solicitacao de agendamento

                    // apos cadastrar o agendamento no banco exibir uma mensaeg para o usaurio
                    // informando que a solicitaçao está pendente e que quando for confirmada uma mensagem será enviada
                    // ou algo do tipo


                    //apos exibir mensagem chamar tela meus agendamentos
                    var intent = new Intent(this, typeof(MeusAgendamentos));
                    StartActivity(intent);
                    Finish();

                }
            };
        }
    }
}