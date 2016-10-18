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
    [Activity(Label = "MeusAgendamentos")]
    public class MeusAgendamentos : Activity
    {

        Button btnVoltar;
        ListView listaAgendamentos;
        JavaList<string> agendamentosDisplay = new JavaList<string>();
        List<Agendamento> agendamentos = new List<Agendamento>();
        //ArrayAdapter adapter;
        AgendamentoAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MeusAgendamentos);

            string id = Intent.GetStringExtra("id") ?? "Data not available";

            btnVoltar = FindViewById<Button>(Resource.Id.btn_voltar);
            listaAgendamentos = FindViewById<ListView>(Resource.Id.listaAgendamentos);
            //adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, agendamentosDisplay);
            adapter = new AgendamentoAdapter(this, agendamentos);
            retrieve(id);


            btnVoltar.Click += (sender, e) =>
            {
                Finish();
            };

            listaAgendamentos.ItemClick += (sender, e) => {
                var intent = new Intent(this, typeof(DetalhesAgendamento));
                intent.PutExtra("id", agendamentos[e.Position].Id);
                intent.PutExtra("idservico", agendamentos[e.Position].IdServico);
                intent.PutExtra("idusuario", agendamentos[e.Position].IdUsuario);
                intent.PutExtra("day", agendamentos[e.Position].Data.Day);
                intent.PutExtra("month", agendamentos[e.Position].Data.Month);
                intent.PutExtra("year", agendamentos[e.Position].Data.Year);
                intent.PutExtra("status", agendamentos[e.Position].Confirmado);

                StartActivity(intent);
            };
        }


        public void retrieve(string id)
        {

            MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso2!");

                    MySqlCommand cmd = new MySqlCommand("select id, data, id_servico, id_usuario, confirmado from agendamento where id_usuario = @id_usuario", con);
                    cmd.Parameters.AddWithValue("@id_usuario", id);

                    Console.WriteLine("Passou comando2!");
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            //   materiaisDisplay.Add(reader["nome"].ToString());
                            // Console.WriteLine("Adicionando. MateriaisDisplay: " + materiaisDisplay);


                            Console.WriteLine("Passou execute reader2!");
                            int id2 = reader.GetOrdinal("id");
                            int idservico = reader.GetOrdinal("id_servico");
                            int idusuario = reader.GetOrdinal("id_usuario");
                            int conf = reader.GetOrdinal("confirmado");

                            Console.WriteLine("Passou execute2");

                            Agendamento a = new Agendamento();

                            Console.WriteLine("Passou execute3");
                            a.Id = reader.GetInt32(id2);
                            Console.WriteLine("Passou execute4");
                            a.Data = reader.GetDateTime("data");
                            Console.WriteLine("Passou execute5");
                            a.IdServico = reader.GetInt32(idservico);
                            a.IdUsuario = reader.GetInt32(idusuario);
                            a.Confirmado = reader.GetInt32(conf);

                            Console.WriteLine("Id= " + a.Id + " Data= " + a.Data + " idservico= " + a.IdServico);
                            Console.WriteLine("idusuario= " + a.IdUsuario + "confirmado= " + a.Confirmado);

                            agendamentos.Add(a);
                            if (a.IdServico == 1)
                                agendamentosDisplay.Add("Medi??o");
                            else if (a.IdServico == 2)
                                agendamentosDisplay.Add("Entrega");
                            else if (a.IdServico == 3)
                                agendamentosDisplay.Add("Instala??o");

                        }


                    }

                    if (agendamentosDisplay.Size() > 0)
                    {
                        listaAgendamentos.Adapter = adapter;
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

        }

        T[] InicializarArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }

            return array;
        }
    }
}