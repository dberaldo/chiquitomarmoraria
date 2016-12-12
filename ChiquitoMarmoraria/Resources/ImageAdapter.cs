using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Collections.Generic;

namespace ChiquitoMarmoraria
{
	public class ImageAdapter : BaseAdapter
	{
		private readonly Context context;
		private readonly List<Bitmap> lista_fotos;
		private List<string> lista_nomes;

		public ImageAdapter(Context c, List<Bitmap> lista_fotos, List<string> lista_nomes)
		{
			context = c;
			this.lista_fotos = lista_fotos;
			this.lista_nomes = lista_nomes;
		}

		public override int Count
		{
			get { return lista_nomes.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			return lista_nomes[position];
		}

		public override long GetItemId(int position)
		{
			return 0;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			ImageView imageView;
			View v = new View(context);

			if (convertView == null)
			{
				// if it's not recycled, initialize some attributes
				LayoutInflater inflater = LayoutInflater.From(parent.Context);
				v = inflater.Inflate(Resource.Layout.subLayoutView, parent, false);
			}
			else
			{
				v = (View) convertView;
			}

			imageView = v.FindViewById<ImageView>(Resource.Id.imagemMaterial);
			imageView.LayoutParameters = new Android.Widget.LinearLayout.LayoutParams(400, 240);
			imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
			imageView.SetPadding(8, 8, 8, 8);
			imageView.SetImageBitmap(lista_fotos[position]);
			TextView txt = v.FindViewById<TextView>(Resource.Id.legenda);
			txt.Text = lista_nomes[position];
			return v;
		}
	}
}
