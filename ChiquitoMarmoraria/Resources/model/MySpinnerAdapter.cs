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

namespace ChiquitoMarmoraria.Resources.Model
{
    public class MySpinnerAdapter : ArrayAdapter<Material>
    {
        private readonly int _resource;
        private readonly int _resourceId;

        public MySpinnerAdapter(Context context, int resource, int textViewResourceId, IList<Material> objects)
            : base(context, resource, textViewResourceId, objects)
        {
        Console.WriteLine("entrou construtor MySpinnver adapter");
            _resource = resource;
            _resourceId = textViewResourceId;
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            var inflater = LayoutInflater.FromContext(Context);
            var view = convertView ?? inflater.Inflate(_resource, parent, false);
            var textView = view.FindViewById<TextView>(_resourceId);
            textView.Text = GetItem(position).Nome;

            return view;
        }
    }
}