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
using Fiuba7539.Droid.Helpers;
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
    /// <summary>
    /// https://abhiandroid.com/ui/searchview
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class SearchActivity : AppCompatActivity, 
        BottomNavigationView.IOnNavigationItemSelectedListener,
        SearchView.IOnQueryTextListener
    {
        private SearchView editsearch;
        private ListView listView;

        private IList<SearchProcessItemModel> searchResults = new List<SearchProcessItemModel>();
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

            // Pass results to ListViewAdapter Class
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new string[] 
            {
                "Hola",
                "Chau"
            });

            listView.SetAdapter(adapter);
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
            // Esto es para que me permita ejecutarlo sincrónicamente.
            StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().PermitAll().Build();
            StrictMode.SetThreadPolicy(policy);

            var search = URLEncoder.Encode(query);
            var response = RestHelper.Post<SearchProcessItemModel[]>($"api/svc/search?text={search}");

            // string text = newText;
            // adapter.filter(text);

            //adapter.Clear();
            // adapter.AddAll((System.Collections.ICollection)response.Select(p =>
            // {
            //     return p.Name;
            // }).ToArray());

            // Pass results to ListViewAdapter Class
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, response.Select(p =>
            {
                return p.Name;
            }).ToArray());

            listView.SetAdapter(adapter);

            return false;
        }
    }
}
