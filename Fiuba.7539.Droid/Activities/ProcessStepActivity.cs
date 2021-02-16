using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProcessStepActivity : ActivityBase
    {
        private ItemModel current;
        private ItemModel[] next;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_process_step);
            
            // Tomo el Id enviado por parámetro.
            string items = Intent.GetStringExtra("items") ?? string.Empty;            

            var deserialized = JsonHelper.Parse<ItemModel[]>(items);
            current = deserialized.First();
            next = deserialized.Skip(1).ToArray();                       

            FindViewById<TextView>(Resource.Id.message)
                .SetText($"{current.Order}. {current.Name}", TextView.BufferType.Normal);

            FindViewById<TextView>(Resource.Id.message2)
                .SetText(current.Description, TextView.BufferType.Normal);

            if (current.Image != null)
            {
                var data = RestHelper.Post(current.Image);
                byte[] imageAsBytes = Base64.Decode(data, Base64Flags.Default);
                var bmp = BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);

                var img = FindViewById<ImageView>(Resource.Id.imageView1);                
                img.SetImageBitmap(bmp);

                
                // img.Alpha = 0.5f;
                // img.BringToFront();
                // img.Enabled = true;
                
            }
        }

        private bool LastStep => next.Count() == 0;

        private async Task Speak()
        {
            var text = $"Paso {current.Order}. ";

            if (LastStep)
            {
                text += "Y último.";
            }

            await Speak($"{text} {current.Name}. \n\n {current.Description}", () =>
            {
                WaitForCommand();
            });
        }

        protected override async void OnPostResume()
        {
            base.OnPostResume();

            await Speak();
        }

        protected override void ExecuteCommand(string command)
        {
            if (command == Commands.Next ||
                command == Commands.Ready)
            {
                if (!LastStep)
                {
                    var activity = new Intent(this, typeof(ProcessStepActivity));
                    activity.PutExtra("items", JsonHelper.Serialize(next));
                    StartActivity(activity);
                }
            }
            else if (command == Commands.Back 
                || command == Commands.Previews)
            {
                StopListening();
                Finish();
            }
        }

        protected override IEnumerable<string> GetAvailableCommands()
        {
            yield return Commands.Back;
            yield return Commands.Previews;
            yield return Commands.Next;
            yield return Commands.Ready;
        }
    }
}
