using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fiuba.App7539.Helpers;
using Fiuba.App7539.Models;
using Fiuba7539.Droid.Base;
using Fiuba7539.Droid.Helpers;
using Fiuba7539.Droid.Models;
using Java.IO;
using Java.Lang;
using Java.Net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Fiuba7539.Droid.Activities
{
    /// <summary>
    /// https://abhiandroid.com/ui/searchview
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class ProcessActivity : AppCompatActivity
    {
        private ProcessModel process;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_process);
            
            // Tomo el Id enviado por parámetro.
            string id = Intent.GetStringExtra("Id") ?? string.Empty;

            this.process = RestHelper.Post<ProcessModel>($"api/svc/process?id={id}");
            
            var message = FindViewById<TextView>(Resource.Id.message);
            message.SetText(process.Name, TextView.BufferType.Normal);

            // Button button1 = FindViewById<Button>(Resource.Id.button1);
            // GradientDrawable bgDrawable = (GradientDrawable)button1.Background;
            // bgDrawable.SetColor(Color.CornflowerBlue.ToInt());
        }
    }
}
