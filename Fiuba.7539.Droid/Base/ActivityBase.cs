using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Fiuba7539.Droid
{
    /// <summary>
    /// https://stackoverflow.com/questions/19724471/speech-recognition-without-google-dialog-boxes
    /// </summary>
    public abstract class ActivityBase : AppCompatActivity
    {
        private const int StartListening = 636;
        private const int VoiceCommandResult = 10;

        private SpeechOptions speakSettings;
        private SpeechRecognizer mSpeechRecognizer;
        //private Intent voiceIntent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.mSpeechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            
            //var mSpeechRecognizerIntent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
            //var mSpeechRecognizerIntent.PutExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL,
            //                                 RecognizerIntent.LANGUAGE_MODEL_FREE_FORM);
            //var mSpeechRecognizerIntent.PutExtra(RecognizerIntent.EXTRA_CALLING_PACKAGE,
            //                                 this.getPackageName());


            mSpeechRecognizer.Results += MSpeechRecognizer_Results;
            mSpeechRecognizer.Error += MSpeechRecognizer_Error;
        }

        private void MSpeechRecognizer_Error(object sender, ErrorEventArgs e)
        {
            // await IDoNotUnderstand();
            WaitForCommand();
        }

        private async void MSpeechRecognizer_Results(object sender, ResultsEventArgs e)
        {
            var matches = e.Results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);


            //wordsList.setAdapter(new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, matches));
            //
            //
            //if (matches.contains("hello") {
            //
            //    Toast.makeText(getBaseContext(), "Recognision OK!!!", Toast.LENGTH_SHORT).show();
            //
            //}


            //var matches = voiceIntent.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
            ////var matches = e.Results;
            if (matches.Count != 0)
            {
                await ProcessCommand(matches.First());
                //string textInput = mFilterText.Text + matches[0];
            
                //mFilterText.Text = textInput;
            }
            else
            {
                await IDoNotUnderstand();
                // mFilterText.Text = "Nothing was recognized";
            }
        }

        protected async override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            var locales = await TextToSpeech.GetLocalesAsync();

#if DEBUG
            var locs = locales.Select(l =>
            {
                return $"{l.Language}-{l.Country}";
            }).OrderBy(p => p).ToArray();
#endif

            // Grab the first locale
            var locale = locales.FirstOrDefault(l => l.Country.ToLower() == "us" && l.Language.ToLower() == "es");

            speakSettings = new SpeechOptions()
            {
                Volume = .75f,
                Pitch = 1.0f,
                Locale = locale
            };
        }

        protected async Task Speak(string message, Action afterFinished)
        {
            await TextToSpeech.SpeakAsync(message, speakSettings).ContinueWith(t =>
            {
                afterFinished();
            });
        }

        protected void WaitForCommand()
        {
            
            //this.SetResult(Result.Ok); 
            MainThread.BeginInvokeOnMainThread(() =>
            {

                // create the voice intent  
                var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                //// message and modal dialog  
                //voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Esperando");

                // end capturing speech if there is 3 seconds of silence  
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 6000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 60000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 30000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                // method to specify other languages to be recognised here if desired  
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                //StartActivityForResult(voiceIntent, VoiceCommandResult);

                // mSpeechRecognizer.SetRecognitionListener(this);
                //mSpeechRecognizer.StartListening(voiceIntent);

                // Code to run on the main thread
                mSpeechRecognizer.StartListening(voiceIntent);
            });

            //// create the voice intent  
            //var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            //voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

            ////// message and modal dialog  
            ////voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Esperando");

            //// end capturing speech if there is 3 seconds of silence  
            //voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 999993000);
            //voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 999993000);
            //voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 30000);
            //voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            //// method to specify other languages to be recognised here if desired  
            //voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
            //StartActivityForResult(voiceIntent, VoiceCommandResult);
        }

        //public async override void OnUserInteraction()
        //{
        //    base.OnUserInteraction();

        //}


        protected async override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == StartListening)
            {
                
            }
            else if (requestCode == VoiceCommandResult)
            {
                if (resultCode == Result.Ok)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        await ProcessCommand(matches.First());
                        //string textInput = mFilterText.Text + matches[0];

                        //mFilterText.Text = textInput;
                    }
                    else
                    {
                        await IDoNotUnderstand();
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

        protected async Task IDoNotUnderstand()
        {
            await Speak("No te entiendo. Inténtalo otra vez.", () => WaitForCommand());
        }

        private async Task ProcessCommand(string command)
        {
            await Speak($"Comando recibido: '{command}'", () => WaitForCommand());
        }







        //public void onBeginningOfSpeech() 
        //{
        //}

        //public void onBufferReceived(byte[] buffer)
        //{ 
        //}

        //public void onEndOfSpeech() 
        //{ 
        //}

        //public void onError(int error)
        //{

        //    //mSpeechRecognizer.startListening(mSpeechRecognizerIntent);

        //}

        //public void onEvent(int eventType, Bundle args) 
        //{ 
        //}


        //public void onPartialResults(Bundle partialResults) 
        //{ 
        //}


        //public void onReadyForSpeech(Bundle args)
        //{


        //    //Toast.makeText(getBaseContext(), "Voice recording starts", Toast.LENGTH_SHORT).show();

        //}

        //public void onResults(Bundle results)
        //{

        //    //ArrayList<String> matches = results.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION);


        //    //wordsList.setAdapter(new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, matches));


        //    //if (matches.contains("hello") {

        //    //    Toast.makeText(getBaseContext(), "Recognision OK!!!", Toast.LENGTH_SHORT).show();

        //    //}

        //}

        //public void onRmsChanged(float rmsdB) 
        //{ 
        //}

        //void IRecognitionListener.OnBeginningOfSpeech()
        //{            
        //}

        //void IRecognitionListener.OnBufferReceived(byte[] buffer)
        //{            
        //}

        //void IRecognitionListener.OnEndOfSpeech()
        //{           
        //}

        //async void IRecognitionListener.OnError(SpeechRecognizerError error)
        //{
        //    WaitForCommand();
        //}

        //void IRecognitionListener.OnEvent(int eventType, Bundle @params)
        //{            
        //}

        //void IRecognitionListener.OnPartialResults(Bundle partialResults)
        //{            
        //}

        //void IRecognitionListener.OnReadyForSpeech(Bundle @params)
        //{
        //}

        //void IRecognitionListener.OnResults(Bundle results)
        //{            
        //}

        //void IRecognitionListener.OnRmsChanged(float rmsdB)
        //{            
        //}
    }
}
