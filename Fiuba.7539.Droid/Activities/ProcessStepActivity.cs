﻿using Android.App;
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
using static Android.Views.View;

namespace Fiuba7539.Droid.Activities
{
    /// <summary>
    /// https://abhiandroid.com/ui/searchview
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProcessStepActivity : ActivityBase, IOnClickListener
    {
        private ItemModel current;
        private ItemModel[] next;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_process_step);
            
            // Tomo el Id enviado por parámetro.
            string items = Intent.GetStringExtra("items");

            if (items != null)
            {
                var deserialized = JsonHelper.Parse<ItemModel[]>(items);
                current = deserialized.First();
                next = deserialized.Skip(1).ToArray();

                FindViewById<TextView>(Resource.Id.message)
                    .SetText($"{current.Order}. {current.Name}", TextView.BufferType.Normal);

                FindViewById<TextView>(Resource.Id.message2)
                    .SetText(current.Description, TextView.BufferType.Normal);

                FindViewById<Button>(Resource.Id.buttonNext).SetOnClickListener(this);
                // FindViewById<Button>(Resource.Id.buttonBack).SetOnClickListener(this);

                if (current.Image != null)
                {
                    var data = RestHelper.Post(current.Image);
                    byte[] imageAsBytes = Base64.Decode(data, Base64Flags.Default);
                    var bmp = BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);

                    var img = FindViewById<ImageView>(Resource.Id.imageView1);
                    img.SetImageBitmap(bmp);
                }
            }
        }

        private bool LastStep => next.Count() == 0;

        protected override async void OnPostResume()
        {
            base.OnPostResume();

            var text = $"Paso {current.Order}. ";

            if (LastStep)
            {
                text += "Y último.";
            }

            await Speak($"{text} {current.Name}. \n\n {current.Description}", () => WaitForCommand());
        }

        protected override void ExecuteCommand(string command)
        {
            if (command == Commands.Next ||
                command == Commands.Ready)
            {
                NextStep();
            }
            else if (command == Commands.Back 
                || command == Commands.Previews)
            {
                StopListening();
                Finish();
            }
        }

        private void NextStep()
        {
            // Si es el último paso.
            if (LastStep)
            {
                // Cierro todo y vuelvo al home.                
                BackToMain();
            }
            else
            {                
                var nextStepIntent = new Intent(this, typeof(ProcessStepActivity));
                nextStepIntent.PutExtra("items", JsonHelper.Serialize(next));
                StartActivity(nextStepIntent);
            }
        }

        private void BackToMain()
        {            
            SetResult(Result.Canceled);
            Finish();
        }

        protected override IEnumerable<string> GetAvailableCommands()
        {
            yield return Commands.Back;
            yield return Commands.Previews;
            yield return Commands.Next;
            yield return Commands.Ready;
        }

        public void OnClick(View v)
        {            
            if (v.Id == Resource.Id.buttonNext)
            {
                NextStep();
            }
        }
    }
}
