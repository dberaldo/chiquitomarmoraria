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
    public class Agendamento
    {
        public int Id { get; set; }
        public int IdServico { get; set; }
        public int IdUsuario { get; set; }
        public bool Confirmado { get; set; }
        public DateTime Data { get; set; }
    }
}