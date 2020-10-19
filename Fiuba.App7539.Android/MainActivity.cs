
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Views;
using Android.Widget;

namespace Fiuba.App7539.Droid
{
    [Activity(Label = "Fiuba.App7539", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        ////private FlexGrid mGrid;
        //private EditText mFilterText;
        ////private FilterCellFactory mFilterCellFactory;
        private Button mRecordButton;
        private bool isRecording;

        private Button testButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            // mRecordButton = FindViewById<Button>(3);
            
            

            //var a = 0;
            //string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            ////checking for Microphone on device  
            //if (rec != "android.hardware.microphone")
            //{
            //    var micAlert = new AlertDialog.Builder(mRecordButton.Context);
            //    micAlert.SetTitle("Device doesn't have a mic for recording");
            //    micAlert.SetPositiveButton("OK", (sender, e) =>
            //    {
            //        return;
            //    });
            //    micAlert.Show();
            //}
            //else
            //{
            //    mRecordButton.Click += delegate
            //    {
            //        isRecording = !isRecording;
            //        if (isRecording)
            //        {
            //            // create the voice intent  
            //            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

            //            // message and modal dialog  
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now");

            //            // end capturing speech if there is 3 seconds of silence  
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 3000);
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 3000);
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 30000);
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            //            // method to specify other languages to be recognised here if desired  
            //            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            //            StartActivityForResult(voiceIntent, 10);
            //        }
            //    };
            //}
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnUserInteraction()
        {
            base.OnUserInteraction();

            isRecording = !isRecording;
            if (isRecording)
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
            
        }

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