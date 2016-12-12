using System;
using Android.Views;
using Android.Widget;
using Android.Content;
using Java.Lang;
using System.Collections.Generic;

namespace ChiquitoMarmoraria
{
	public class ListViewAdapter : BaseAdapter
	{
		private readonly Context context;
		List<string> lista_string;
		List<int> lista_int;

		public ListViewAdapter(Context c, List<string> lista_materiais, List<int> lista_qtdes)
		{
			context = c;
			this.lista_string = lista_materiais;
			this.lista_int = lista_qtdes;
		}

		public override int Count
		{
			get
			{
				return lista_string.Count;
			}
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return null;
		}

		public override long GetItemId(int position)
		{
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			TextView text1, text2;
			View v = new View(context);

			if(convertView == null)
			{
				LayoutInflater inflater = LayoutInflater.From(parent.Context);
				v = inflater.Inflate(Resource.Layout.ListItemEstatisticas, parent, false);
				Console.WriteLine("inflou");
			}
			else
			{
				v = (View) convertView;
			}

			text1 = v.FindViewById<TextView>(Resource.Id.textNomeMaterial);
			text2 = v.FindViewById<TextView>(Resource.Id.textQuantidade);

			text1.Text = lista_string[position];
			text2.Text = lista_int[position].ToString();

			if (position % 2 == 0)
			{
				text1.SetBackgroundColor(Android.Graphics.Color.ParseColor("#CFD8DC"));
				text2.SetBackgroundColor(Android.Graphics.Color.ParseColor("#CFD8DC"));
			}

			return v;
		}
	}
}
