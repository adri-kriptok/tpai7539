using Android.App;
using Android.Content;
using Android.Content.PM;
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
using static Android.Views.View;

namespace Fiuba7539.Droid.Activities
{
    /// <summary>
    /// https://abhiandroid.com/ui/searchview
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProcessActivity : ActivityBase, IOnClickListener
    {
        private ProcessModel process;
        private ListView listView;
        private ArrayAdapter<string> adapter;
        private Button button;


        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_process);
            
            // Tomo el Id enviado por parámetro.
            string id = Intent.GetStringExtra("Id") ?? string.Empty;            

            // Parceo el proceso.
            process = RestHelper.Post<ProcessModel>($"api/svc/process?id={id}");

            // Reenumero los pasos según orden.
            process.Items = process.Items.OrderBy(p => p.Order).ToArray();
            for (int i = 0; i < process.Items.Length; i++)
            {
                process.Items[i].Order = i + 1;
            }
            
            var message = FindViewById<TextView>(Resource.Id.message);
            message.SetText(process.Name, TextView.BufferType.Normal);

            listView = FindViewById<ListView>(Resource.Id.listview);

            var array = process.Requirements.Select(p => p.Name).ToArray();

            for (int i = 0; i < array.Length; i++)
            {                
                array[i] = $"{i + 1}. {array[i]}";
            }

            //  Cargo la lista de requerimientos.
            adapter = new ArrayAdapter<string>(this, Resource.Drawable.items, array);            
            listView.Adapter = adapter;

            button = FindViewById<Button>(Resource.Id.button1);
            button.SetOnClickListener(this);

            await Speak(process.Name, () => WaitForCommand());
        }

        protected override void OnStart()
        {
            base.OnStart();

            button.Clickable = true;
        }

        public void OnClick(View v)
        {
            button.Clickable = false;

            Start();
        }

        private void Start()
        {
            ShutUp();
            StopListening();

            var activity = new Intent(this, typeof(ProcessStepActivity));
            activity.PutExtra("items", JsonHelper.Serialize(process.Items));
            StartActivity(activity);
        }

        protected override void ExecuteCommand(string command)
        {
            if (command == Commands.Initialize ||
                command == Commands.Start)
            {
                Start();
            }
        }

        protected override IEnumerable<string> GetAvailableCommands()
        {
            yield return Commands.Start;
            yield return Commands.Initialize;
        }
    }
}
