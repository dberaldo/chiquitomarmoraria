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
using Square.TimesSquare;

namespace ChiquitoMarmoraria.Resources
{
    [Activity(Label = "MeusAgendamentosCalendario")]
    public class MeusAgendamentosCalendario : Activity
    {

        Button btnVoltar;
        CalendarPickerView calendarPickerView1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MeusAgendamentosCalendario);

            string id = Intent.GetStringExtra("id") ?? "Data not available";
            
            Console.WriteLine("IDDDD = " + id);

            btnVoltar = FindViewById<Button>(Resource.Id.btn_voltar);
            calendarPickerView1 = FindViewById<CalendarPickerView>(Resource.Id.calendarPickerView1);

            var nextYear = DateTime.Now.AddYears(1);

            calendarPickerView1
                .Init(DateTime.Now, nextYear)
                .InMode(CalendarPickerView.SelectionMode.Single)
                .WithSelectedDate(DateTime.Now);

            calendarPickerView1.DateSelected += delegate
            {
                DateTime selectedDate = calendarPickerView1.SelectedDate;
                Console.WriteLine("clicked on " + selectedDate.ToString());

                MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        Console.WriteLine("Conectado com sucesso2!");

                        MySqlCommand cmd = new MySqlCommand("select id from agendamento where id_usuario = @id_usuario and data = @data", con);
                        cmd.Parameters.AddWithValue("@id_usuario", id);
                        cmd.Parameters.AddWithValue("@data", selectedDate);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // abrir MeusAgendamentos com filtro de data para selectedDate
                                var intent = new Intent(this, typeof(MeusAgendamentos));
                                intent.PutExtra("id", id);
                                intent.PutExtra("data", selectedDate.ToString());
                                Console.WriteLine("MenuUsuario ID = " + id);
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

            retrieveAndHighlight(id, calendarPickerView1);


            btnVoltar.Click += (sender, e) =>
            {
                Finish();
            };

        }


        public void retrieveAndHighlight(string id, CalendarPickerView calendar)
        {

            MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso2!");

                    MySqlCommand cmd = new MySqlCommand("select data from agendamento where id_usuario = @id_usuario group by data", con);
                    cmd.Parameters.AddWithValue("@id_usuario", id);

                    Console.WriteLine("Passou comando2!");
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {
                            Console.WriteLine("Passou execute4");
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
            }
            finally
            {
                con.Close();
            }

        }
    }
}