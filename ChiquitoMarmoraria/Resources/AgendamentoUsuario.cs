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
using Java.Text;
using Java.Util;

namespace ChiquitoMarmoraria.Resources
{
    [Activity(Label = "Agendamento")]
    public class AgendamentoUsuario : Activity
    {

        EditText txtDia;
        EditText txtMes;
        EditText txtAno;
        Button btnAgendar;
        Button btnCancelar;
        RadioButton rbMedicao;
        RadioButton rbEntrega;
        RadioButton rbInstalacao;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AgendamentoUsuario);

            string id = Intent.GetStringExtra("id") ?? "Data not available";

            txtDia = FindViewById<EditText>(Resource.Id.txt_dia);
            txtMes = FindViewById<EditText>(Resource.Id.txt_mes);
            txtAno = FindViewById<EditText>(Resource.Id.txt_ano);
            btnAgendar = FindViewById<Button>(Resource.Id.btn_agendar);
            btnCancelar = FindViewById<Button>(Resource.Id.btn_cancelar);
            rbEntrega = FindViewById<RadioButton>(Resource.Id.rb_entrega);
            rbMedicao = FindViewById<RadioButton>(Resource.Id.rb_medicao);
            rbInstalacao = FindViewById<RadioButton>(Resource.Id.rb_instalacao);

            RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            RadioButton radioButton = FindViewById<RadioButton>(radioGroup.CheckedRadioButtonId);

            int day=0;
            int month=0;
            int year=0;

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
               
                day = Int32.Parse(txtDia.Text);
                month = Int32.Parse(txtMes.Text);
                year = Int32.Parse(txtAno.Text);

                if (day == 0 && month == 0 && year == 0)
                {
                    //Exibir mensagem de erro "Escolha uma data"
                    Toast.MakeText(this, "Escolha uma data", ToastLength.Short).Show();
                }
                else
                {
                    DateTime data = new DateTime(year, month, day);

                    int tipo_servico=0;

                    if (radioButton.Id == Resource.Id.rb_entrega)
                    {
                        tipo_servico = 2;
                    }
                    else if (radioButton.Id == Resource.Id.rb_medicao)
                    {
                        tipo_servico = 1;
                    }
                    else if (radioButton.Id == Resource.Id.rb_instalacao)
                    {
                        tipo_servico = 3;
                    }

                    Console.WriteLine(tipo_servico);

                    //Cadastrar agendamento no banco (TABLE Agendamento columns id, data, id_servico, id_usuario, confirmado)
                    //table deve ter uma coluna boolean "confirmado"
                    //tipo_servico = 1-Medição, 2-Entrega, 3-Instalação
                    
                    MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");
                    
                    try
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                            Console.WriteLine("Conectado com sucesso Agendamento Usuario!");
                            MySqlCommand cmd = new MySqlCommand("INSERT INTO agendamento (data, id_servico, id_usuario, confirmado) VALUES (@data, @id_servico, @id_usuario, @confirmado)", con);
                            cmd.Parameters.AddWithValue("@data", data);
                            cmd.Parameters.AddWithValue("@id_servico", tipo_servico);
                            cmd.Parameters.AddWithValue("@id_usuario", id);
                            cmd.Parameters.AddWithValue("@confirmado", -1);
                            cmd.ExecuteNonQuery();
                            //quando o usuario envia o agendamento a coluna confirmado deve ter valor false
                            //somente terá valor true quando o adm confirmar a solicitacao de agendamento

                            // apos cadastrar o agendamento no banco exibir uma mensaeg para o usaurio
                            // informando que a solicitaçao está pendente 
                            Toast.MakeText(this, "Agendamento solicitado com sucesso. Confira o status do agendamento no menu Meus Agendamentos!", ToastLength.Short).Show();
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Toast.MakeText(this, "Erro ao solicitar agendamento!", ToastLength.Short).Show();

                    }
                    finally
                    {
                        con.Close();
                    }
                    
                    //apos exibir mensagem chamar tela meus agendamentos
                    var intent = new Intent(this, typeof(MeusAgendamentos));
                    intent.PutExtra("id", id);
                    StartActivity(intent);
                    Finish();

                }
            };
        }
    }
}