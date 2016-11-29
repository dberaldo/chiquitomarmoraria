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
    [Activity(Label = "MeuOrcamento")]
    public class MeuOrcamento : Activity
    {

        Button btnVoltar;
        Button btnAdicionar;
        Button btnLimpar;

        TextView txt;

        ListView listaOrcamentos;
        JavaList<string> orcamentosDisplay = new JavaList<string>();
        List<OrcamentoM> orcamentos = new List<OrcamentoM>();
        OrcamentoAdapter adapter;

        float total;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MeuOrcamento);

            string id = Intent.GetStringExtra("id") ?? "Data not available";


            btnVoltar = FindViewById<Button>(Resource.Id.btn_voltar);
            btnAdicionar = FindViewById<Button>(Resource.Id.btn_adicionar);
            btnLimpar = FindViewById<Button>(Resource.Id.btn_limpar);

            txt = FindViewById<TextView>(Resource.Id.lbl_total);

            listaOrcamentos = FindViewById<ListView>(Resource.Id.listaOrcamentos);
            adapter = new OrcamentoAdapter(this, orcamentos);

            total = 0;

            if (retrieve(id) == 0)
                txt.Text = "Seu orçamento está vazio!";
            else
                txt.Text = "Total = R$" + total.ToString();



            listaOrcamentos.ItemClick += (sender, e) =>
            {

            };

            btnVoltar.Click += (object sender, EventArgs e) =>
            {
                Finish();

            };

            btnAdicionar.Click += (object sender, EventArgs e) =>
            {
                var intent = new Intent(this, typeof(Orcamento));
                //intent.PutExtra("id", id);
                StartActivity(intent);

            };

            btnLimpar.Click += (object sender, EventArgs e) =>
            {
                // setar todos as entradas com visivel=false

                total = 0;

                if (retrieve(id) == 0)
                    txt.Text = "Seu orçamento está vazio!";
                else
                    txt.Text = "Total = R$" + total.ToString();

            };
        }


        public int retrieve(string id)
        {

            MySqlConnection con = new MySqlConnection("Server=mysql873.umbler.com;Port=41890;database=ufscarpds;User Id=ufscarpds;Password=ufscar1993;charset=utf8");

            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Conectado com sucesso!");

                    MySqlCommand cmd = new MySqlCommand("select id, material, id_usuario, quantidade, altura, largura, preco, visivel from orcamento where id_usuario = @id_usuario AND visivel=true", con);
                    cmd.Parameters.AddWithValue("@id_usuario", id);

                    Console.WriteLine("Passou comando2!");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int aux = 0;
                        while (reader.Read())
                        {
                            aux++;
                            //   materiaisDisplay.Add(reader["nome"].ToString());
                            // Console.WriteLine("Adicionando. MateriaisDisplay: " + materiaisDisplay);

                            int id2 = reader.GetOrdinal("id");
                            int idusuario = reader.GetOrdinal("id_usuario");
                            int quantidade = reader.GetOrdinal("quantidade");
                            float preco = reader.GetFloat("preco");
                            float altura = reader.GetFloat("altura");
                            float largura = reader.GetFloat("largura");

                            OrcamentoM o = new OrcamentoM();
                            
                            o.Id = reader.GetInt32(id2);
                            o.Material = reader["material"].ToString();
                            o.IdUsuario = reader.GetInt32(idusuario);
                            o.Quantidade = reader.GetInt32(quantidade);
                            o.Preco = preco;
                            o.Visivel = reader.GetBoolean("visivel");
                            o.Largura = largura;
                            o.Altura = altura;
                            
                            orcamentos.Add(o);

                            total = total + (quantidade * preco * altura * largura);
                            
                        }

                        if (aux == 0)
                            return 0;
                        
                    }

                    if (orcamentosDisplay.Size() > 0)
                    {
                        listaOrcamentos.Adapter = adapter;
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

            return 1;

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