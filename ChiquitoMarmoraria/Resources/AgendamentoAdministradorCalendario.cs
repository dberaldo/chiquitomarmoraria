
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
using ChiquitoMarmoraria.Resources.Model;
using ChiquitoMarmoraria.Resources;
using System.Data;
using Square.TimesSquare;

namespace ChiquitoMarmoraria
{
    [Activity(Label = "AgendamentoAdministradorCalendario")]
    public class AgendamentoAdministradorCalendario : Activity
    {
        CalendarPickerView calendarPickerView1;
        // ListView resultAgenda;
        // AgendamentoAdapter adapter;
        // List<Agendamento> agendamentos = new List<Agendamento>();
        // JavaList<string> agendamentosDisplay = new JavaList<string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AgendamentoAdministradorCalendario);

            calendarPickerView1 = FindViewById<CalendarPickerView>(Resource.Id.calendarPickerView1);
            // resultAgenda = FindViewById<ListView>(Resource.Id.resultAgenda);
            // adapter = new AgendamentoAdapter(this, agendamentos);

            var nextYear = DateTime.Now.AddYears(1);

            calendarPickerView1
                .Init(DateTime.Now, nextYear)
                .InMode(CalendarPickerView.SelectionMode.Single)
                .WithSelectedDate(DateTime.Now);

            calendarPickerView1.DateSelected += delegate
            {
                DateTime selectedDate = calendarPickerView1.SelectedDate;

                MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        Console.WriteLine("Conectado com sucesso2!");

                        MySqlCommand cmd = new MySqlCommand("select id from agendamento where data = @data AND confirmado=1", con);
                        cmd.Parameters.AddWithValue("@data", selectedDate);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // abrir AgendamentoAdmin com filtro de data para selectedDate
                                var intent = new Intent(this, typeof(AgendamentoAdministrador));
                                intent.PutExtra("data", selectedDate.ToString());
                                StartActivity(intent);
                            }
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
            };

            // DateTime today = DateTime.Now.Date;
            // Console.WriteLine("DATE = " + today.Year + "-" + today.Month + "-" + today.Day);

            retrieveAndHighlight(calendarPickerView1);
        }

        public void retrieveAndHighlight(CalendarPickerView calendar)
        {
            MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso Agendamento Usuario!");
                    MySqlCommand cmd = new MySqlCommand("select data from agendamento where confirmado = 1 group by data", con);
                    // MySqlCommand cmd = new MySqlCommand("select id, data, id_servico, id_usuario, confirmado from agendamento WHERE data >= @dataIni AND data <= @dataFim AND confirmado=1", con);

                    // cmd.Parameters.AddWithValue("@dataIni", today);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime data = reader.GetDateTime("data");

                            if (DateTime.Now <= data && data <= DateTime.Now.AddYears(1))
                                calendar.HighlightDates(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Toast.MakeText(this, "Erro ao carregar agenda!", ToastLength.Short).Show();

            }
            finally
            {
                con.Close();
            }
        }
    }
}
