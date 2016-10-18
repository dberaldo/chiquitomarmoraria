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
    public class AgendamentoAdapter : BaseAdapter<Agendamento>
    {
        private readonly Activity context;
        private readonly List<Agendamento> agendamentos;

        public AgendamentoAdapter(Activity context, List<Agendamento> agendamentos)
        {
            this.context = context;
            this.agendamentos = agendamentos;
        }

        public override Agendamento this[int position]
        {
            get
            {
                return agendamentos[position];
            }
        }

        public override int Count
        {
            get
            {
                return agendamentos.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return agendamentos[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ItemAgendamento, parent, false);

            var txtTipo = view.FindViewById<TextView>(Resource.Id.txt_tipo);
            var txtData = view.FindViewById<TextView>(Resource.Id.txt_data);
            var txtStatus = view.FindViewById<TextView>(Resource.Id.txt_status);

            if (agendamentos[position].IdServico == 1)
                txtTipo.Text = "Medicao";
            else if (agendamentos[position].IdServico == 2)
                txtTipo.Text = "Entrega";
            else if (agendamentos[position].IdServico == 3)
                txtTipo.Text = "Instalacao";

            if (agendamentos[position].Confirmado == -1)
                txtStatus.Text = "Status: Pendente";
            else if (agendamentos[position].Confirmado == 0)
                txtStatus.Text = "Status: Cancelado";
            else if (agendamentos[position].Confirmado == 1)
                txtStatus.Text = "Status: Confirmado";

            int dia = agendamentos[position].Data.Day;
            int mes = agendamentos[position].Data.Month;
            int ano = agendamentos[position].Data.Year;

            txtData.Text = "Data: "+dia + "/" + mes + "/" + ano;

            return view;
        }



    }


}