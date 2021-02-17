using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Fiuba7539.Droid.Activities;
using Fiuba7539.Droid.Base;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using static Android.Views.View;

namespace Fiuba7539.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : ActivityBase, IOnClickListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
             var navigation = FindViewById<Button>(Resource.Id.buttonSearch);
             navigation.SetOnClickListener(this);            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnActionModeStarted(ActionMode mode)
        {
            base.OnActionModeStarted(mode);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            JavaSystem.Exit(0);
        }

        protected override async void OnPostResume()
        {
            base.OnPostResume();

            await Speak($"Bienvenido!", () => WaitForCommand());
        }

        protected override void ExecuteCommand(string command)
        {
            if (command == Commands.Search)
            {
                GoToSearch();
            }
        }

        protected override IEnumerable<string> GetAvailableCommands()
        {
            yield return Commands.Search;
        }

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.buttonSearch)
            {
                GoToSearch();
            }
        }

        private void GoToSearch()
        {
            ShutUp();
            StopListening();
            StartActivity(typeof(SearchActivity));
        }
    }
}
