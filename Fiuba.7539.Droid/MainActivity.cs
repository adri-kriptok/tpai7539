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
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
             var navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
             navigation.SetOnNavigationItemSelectedListener(this);            
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

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.navigation_search)
            {
                StartActivity(typeof(SearchActivity));
            }

            
            //DrawerLayout drawer = (DrawerLayout)findViewById(R.id.drawer_layout);
            //drawer.closeDrawer(GravityCompat.START);

            return false;
        }

        protected override async void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            await Speak($"Bienvenido!", () =>
            {
                WaitForCommand();
            });
        }

        protected override void ExecuteCommand(string command)
        {
            if (command == Commands.Search)
            {
                StartActivity(typeof(SearchActivity));
            }
        }

        protected override IEnumerable<string> GetAvailableCommands()
        {
            yield return Commands.Search;
        }
    }
}
