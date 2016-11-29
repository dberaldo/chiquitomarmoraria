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
    public class OrcamentoM
    {
        public int Id { get; set; }
        public string Material { get; set; }
        public int IdUsuario { get; set; }
        public int Quantidade { get; set; }
        public float Largura { get; set; }
        public float Altura { get; set; }
        public DateTime Data { get; set; }
        public bool Visivel { get; set; }
        public float Preco { get; set; }
    }
}