using Android.App;
using Android.Content;
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

namespace Fiuba7539.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : ActivityBase, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private TextView textMessage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            textMessage.SetText(Resource.String.title_home);

            return false;
        }

        public override void OnActionModeStarted(ActionMode mode)
        {
            base.OnActionModeStarted(mode);
        }

        protected async override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            await Speak("Bienvenido! \n\n Háblame y empecemos a hacer cosas juntos!", () =>
            {
                WaitForCommand();
            });
        }

        protected async override void OnRestart()
        {
            base.OnRestart();

            await Speak("Que quieres hacer?", () =>
            {
                WaitForCommand();
            });            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            JavaSystem.Exit(0);
        }

        protected override IEnumerable<string> GetAvailableCommands()
        {
            yield return Commands.Search;
        }

        protected override void ExecuteCommand(string command)
        {
            StartActivity(typeof(SearchActivity));
        }
    }
}
