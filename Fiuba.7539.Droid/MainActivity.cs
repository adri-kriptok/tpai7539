using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Linq;
using Xamarin.Essentials;

namespace Fiuba7539.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

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


            var welcomeText = "Bienvenido! \n\n Háblame y empecemos a hacer cosas juntos!";


            var locales = await TextToSpeech.GetLocalesAsync();

#if DEBUG
            var locs = locales.Select(l =>
            {
                return $"{l.Language}-{l.Country}";
            }).OrderBy(p => p).ToArray();
#endif

            // Grab the first locale
            var locale = locales.FirstOrDefault(l => l.Country.ToLower() == "us" && l.Language.ToLower() == "es");

            var settings = new SpeechOptions()
            {
                Volume = .75f,
                Pitch = 1.0f,
                Locale = locale
            };

            await TextToSpeech.SpeakAsync(welcomeText, settings).ContinueWith(t =>
            {
                WaitForCommand();
            });
        }

        private void WaitForCommand()
        {
            // create the voice intent  
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

            // message and modal dialog  
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");

            // end capturing speech if there is 3 seconds of silence  
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 3000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 3000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 30000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            // method to specify other languages to be recognised here if desired  
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            StartActivityForResult(voiceIntent, 10);
        }

        //public async override void OnUserInteraction()
        //{
        //    base.OnUserInteraction();

        //}


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 10)
            {
                if (resultCode == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        //string textInput = mFilterText.Text + matches[0];

                        //mFilterText.Text = textInput;
                    }
                    else
                    {
                        // mFilterText.Text = "Nothing was recognized";
                    }
                }
            }

            base.OnActivityResult(requestCode, resultCode, data);

            // if (requestCode == VOICE)
            // {
            //     if (resultVal == Result.Ok)
            //     {
            //         var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
            //         if (matches.Count != 0)
            //         {
            //             string textInput = textBox.Text + matches[0];
            //             textBox.Text = textInput;
            //             switch (matches[0].Substring(0, 5).ToLower())
            //             {
            //                 case "north":
            //                     MovePlayer(0);
            //                     break;
            //                 case "south":
            //                     MovePlayer(1);
            //                     break;
            //             }
            //         }
            //         else
            //         {
            //             textBox.Text = "No speech was recognised";
            //         }
            //     }
            //     base.OnActivityResult(requestCode, resultVal, data);
            // }
        }
    }
}
