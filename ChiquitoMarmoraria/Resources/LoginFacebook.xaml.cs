using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Xamarin.Forms;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;

namespace ChiquitoMarmoraria.Resources
{
    public partial class LoginFacebook //: ContentPage
    {
        private String clientId = "1800507543567768";

        public LoginFacebook()
        {
//            InitializeComponent();

            var apiRequest = "https://www.facebook.com/dialog/oauth?client_id="
                + clientId
                + "&display=popup&response_type=token&redirect_uri=http://www.facebook.com/connect/login_success.html";

  /*          var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1
            };*/
        }
    }
}