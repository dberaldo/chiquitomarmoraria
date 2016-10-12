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
    [Activity(Label = "Orçamento Automático")]
    public class Orcamento : Activity
    {
        Spinner spinner;
        List<Material> listaMaterial = new List<Material>();
        TextView spin_txt;
        EditText txtLargura;
        EditText txtAltura;
        EditText txtQtd;
        Button btnCalcular;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Orcamento);
            // Create your application here

            retrieve();

            System.Console.WriteLine("PAAAAAAASSOU RETRIEVE!!!!!!!!!");

            spinner = FindViewById<Spinner>(Resource.Id.spn_material);
            System.Console.WriteLine("PAAAAAAASSOU finview!");
            
            spinner.Adapter = new MySpinnerAdapter(this, Resource.Layout.SppinerItem, Resource.Id.spinnerText, listaMaterial);
            //spinner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, listaMaterial);
            Material escolhido; 

            spin_txt = FindViewById<TextView>(Resource.Id.spinnerText);
            txtAltura = FindViewById<EditText>(Resource.Id.txt_altura);
            txtLargura = FindViewById<EditText>(Resource.Id.txt_largura);
            txtQtd = FindViewById<EditText>(Resource.Id.txt_qtd);
            btnCalcular = FindViewById<Button>(Resource.Id.btn_calcular);

            btnCalcular.Click += (object sender, EventArgs e) =>
            {
                float altura = (float)Convert.ToDouble(txtAltura.Text);
                float largura = (float)Convert.ToDouble(txtLargura.Text);
                float qtd = (float)Convert.ToDouble(txtQtd.Text);

                float resultado = altura * largura * qtd;
                //exibir popup com valor do orçamento
            };


            spinner.ItemSelected += (s, e) =>
            {
                escolhido = new Material();
                //Do something with the selected item
                //get the position with e.Position
                escolhido.Nome = listaMaterial[e.Position].Nome;
                escolhido.Id = listaMaterial[e.Position].Id;
                escolhido.Categoria = listaMaterial[e.Position].Categoria;
                escolhido.Descricao = listaMaterial[e.Position].Descricao;
                escolhido.Preco = listaMaterial[e.Position].Preco;
            };



        }


        public void retrieve()
        {

            MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3306;database=ufscarpds;User Id=ufscarpds;Password=19931993;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso2!");

                    MySqlCommand cmd = new MySqlCommand("select id, nome, categoria, descricao, preco from material", con);

                    Console.WriteLine("Passou comando2!");

                   

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            //   materiaisDisplay.Add(reader["nome"].ToString());
                            // Console.WriteLine("Adicionando. MateriaisDisplay: " + materiaisDisplay);


                            Console.WriteLine("Passou execute reader2!");
                            int id = reader.GetOrdinal("id");

                            Material mat = new Material();
                            mat.Id = reader.GetInt32(id);
                            mat.Nome = reader["nome"].ToString();
                            mat.Categoria = reader["categoria"].ToString();
                            mat.Descricao = reader["descricao"].ToString();
                            mat.Preco = reader.GetDouble(4);

                            Console.WriteLine("Id= " + mat.Id + " Nome= " + mat.Nome + " Cat= " + mat.Categoria);
                            Console.WriteLine("descr= " + mat.Descricao + "preco= " + mat.Preco);

                            listaMaterial.Add(mat);

                        }


                    }

                    /*if (materiaisDisplay.Size() > 0)
                    {
                        lista.Adapter = adapter;
                    }*/



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