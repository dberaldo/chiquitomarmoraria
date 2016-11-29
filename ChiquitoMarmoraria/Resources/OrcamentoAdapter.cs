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
    public class OrcamentoAdapter : BaseAdapter<OrcamentoM>
    {
        private readonly Activity context;
        private readonly List<OrcamentoM> orcamentos;

        public OrcamentoAdapter(Activity context, List<OrcamentoM> orcamentos)
        {
            this.context = context;
            this.orcamentos = orcamentos;
        }

        public override OrcamentoM this[int position]
        {
            get
            {
                return orcamentos[position];
            }
        }

        public override int Count
        {
            get
            {
                return orcamentos.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return orcamentos[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.ItemOrcamento, parent, false);

            var txtDescricao = view.FindViewById<TextView>(Resource.Id.txt_descricao);
            var txtQuantidade = view.FindViewById<TextView>(Resource.Id.txt_quantidade);
            var txtValor = view.FindViewById<TextView>(Resource.Id.txt_valor);

            float valor = orcamentos[position].Quantidade * orcamentos[position].Preco * orcamentos[position].Altura * orcamentos[position].Largura;

            txtDescricao.Text = orcamentos[position].Material;
            txtQuantidade.Text = "x " + orcamentos[position].Quantidade.ToString();
            txtValor.Text = valor.ToString();

            return view;
        }



    }
}