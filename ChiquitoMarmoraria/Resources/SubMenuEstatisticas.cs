
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

namespace ChiquitoMarmoraria
{
	[Activity(Label = "Estatísticas")]
	public class SubMenuEstatisticas : Activity
	{
		Button materiais_pedidos;
		Button regioes_mais_pedidas;
		Button btnVoltar;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.SubMenuEstatisticas);

			materiais_pedidos = FindViewById<Button>(Resource.Id.btnMateriaisPedidos);
			regioes_mais_pedidas = FindViewById<Button>(Resource.Id.btnRegioesPedidas);
			btnVoltar = FindViewById<Button>(Resource.Id.btnVoltarEstatisticas);

			materiais_pedidos.Click += (sender, e) => 
			{
				var intent = new Intent(this, typeof(EstatisticasMateriais));
				StartActivity(intent);
			};

			regioes_mais_pedidas.Click += (sender, e) =>
			{
				var intent = new Intent(this, typeof(EstatisticasUsuarios));
				StartActivity(intent);
			};

			btnVoltar.Click += (sender, e) => 
			{
				Finish();
			};
		}
	}
}
