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
using ChiquitoMarmoraria.Resources.model;
using Java.Lang;

namespace ChiquitoMarmoraria.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtName { get; set; }
        public TextView txtEmail { get; set; }
        public TextView txtSenha { get; set; }
    }

    public class ListViewAdapter:BaseAdapter
    {
        private Activity activity;
        private List<Pessoa> lstPessoa;

        public ListViewAdapter(Activity activity, List<Pessoa> lstPessoa)
        {
            this.activity = activity;
            this.lstPessoa = lstPessoa;
        }

        public override int Count
        {
            get
            {
                return lstPessoa.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return lstPessoa[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            return null;
         //   var view = convertView??activity.LayoutInflater.Inflate(Resource.Layout.lis)
        }
    }
}