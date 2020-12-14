using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Fiuba.App7539.Helpers;
using Fiuba7539.Droid.Base;
using Fiuba7539.Droid.Models;
using Java.IO;
using Java.Lang;
using Java.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Fiuba7539.Droid.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class SearchActivity : ActivityBase, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;

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
            textMessage.SetText(Resource.String.title_search);

            return false;
        }

        public override void OnActionModeStarted(ActionMode mode)
        {
            base.OnActionModeStarted(mode);
        }

        protected async override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            await Speak("Este es el buscador. Qué quieres buscar?", () =>
            {
                WaitForCommand();
            });
        }

        protected override IEnumerable<string> GetAvailableCommands()
        {
            yield return Commands.Back;
        }

        protected override void ExecuteCommand(string command)
        {
            if (command == Commands.Back)
            {
                Finish();
            }
        }

        protected async override Task OnNotKnownCommand(string search)
        {
            await Speak($"Buscando: '{search}'", () =>
            {
                Search(search);
            });
        }

        private async void Search(string search)
        {
            var responseJson = GetSearchJson(search);

            var parsed = JsonHelper.Parse<SearchProcessItemModel[]>(responseJson);

            await Speak($"Encontré: {StringHelper.Join(parsed.Select(p => p.Name))}.", () =>
            {
                WaitForSelection();
            });
        }

        private async void WaitForSelection()
        {
            await Speak($"Cual quieres hacer?", () =>
            {
                WaitForCommand();
            });
        }

        private static string GetSearchJson(string search)
        {
            var response = string.Empty;

            search = URLEncoder.Encode(search);

            var url = new URL($"http://www.prismasoft.com.ar/demos/Fiuba7539/api/svc/search?text={search}");
            var conn = (HttpURLConnection)url.OpenConnection();


            var authData = string.Format("{0}:{1}", "adrian@prismasoft.com.ar", "asdasd");
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            conn.AddRequestProperty("Authorization", $"Basic {authHeaderValue}");

            conn.RequestMethod = "POST";

            if (conn.ResponseCode == HttpStatus.Ok)
            {
                var reader = new BufferedReader(new InputStreamReader(conn.InputStream));
                var line = reader.ReadLine();

                while (line != null)
                {
                    response += line;
                    line = reader.ReadLine();
                }

                reader.Close();
            }
            conn.Disconnect();
            return response;
        }
    }
}
