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
using Fiuba7539.Droid.Base;
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
    public class SearchActivity : ActivityBase, 
        BottomNavigationView.IOnNavigationItemSelectedListener,
        SearchView.IOnQueryTextListener,
        ListView.IOnItemClickListener
    {
        private SearchView editsearch;
        private ListView listView;

        private IDictionary<string, string> searchResults = null;
        private ArrayAdapter<string> adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_search);

            // Locate the EditText in listview_main.xml
            editsearch = FindViewById<SearchView>(Resource.Id.search);
            editsearch.SetOnQueryTextListener(this);

            listView = FindViewById<ListView>(Resource.Id.listview);
            listView.OnItemClickListener = this;
            
            SetValues(new Dictionary<string, string>());
        }

        protected override async void OnPostResume()
        {
            base.OnPostResume();

            Search(string.Empty);
            await Speak("¿Qué buscamos?", () => WaitForCommand());
        }

        private void SetValues(IDictionary<string, string> results)
        {
            this.searchResults = results;
           
            // Pass results to ListViewAdapter Class
            adapter = new ArrayAdapter<string>(this, Resource.Drawable.items, results.Values.ToArray());            
            listView.Adapter = adapter;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            return false;
        }

        public override void OnActionModeStarted(ActionMode mode)
        {
            base.OnActionModeStarted(mode);
        }

        public bool OnQueryTextChange(string newText)
        {
            return false;
        }

        public bool OnQueryTextSubmit(string query)
        {
            Search(URLEncoder.Encode(query));

            return false;
        }

        private void Search(string search)
        {            
            // Esto es para que me permita ejecutarlo sincrónicamente.
            StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().PermitAll().Build();
            StrictMode.SetThreadPolicy(policy);

            var response = HttpHelper.Post<SearchProcessItemModel[]>($"api/svc/search?text={search}");

            SetValues(response.ToDictionary(p => p.Id, p => p.Name));
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            var activity = new Intent(this, typeof(ProcessActivity));

            activity.PutExtra("Id", searchResults.ToArray()[id].Key);

            ShutUp();
            StopListening();
            StartActivity(activity);
        }

        protected override void ExecuteCommand(string command)
        {
            if (command == Commands.Back)
            {
                ShutUp();
                StopListening();
                Finish();
            }
        }

        protected override IEnumerable<string> GetAvailableCommands()
        {            
            yield return Commands.Back;
        }

        protected async override Task OnNotKnownCommand(string search)
        {
            editsearch.SetQuery(search, true);
        }
    }
}
