using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Fiuba.App7539.Helpers;
using Fiuba7539.Droid.Base;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Fiuba7539.Droid
{
    /// <summary>
    /// https://stackoverflow.com/questions/19724471/speech-recognition-without-google-dialog-boxes
    /// 
    /// https://stackoverflow.com/questions/11951723/disable-ready-sound-of-recognition-listener
    /// </summary>
    public abstract class ActivityBase : AppCompatActivity
    {
        private bool listening = false;
        private SpeechOptions speakSettings;
        private SpeechRecognizer speechRecognizer;
        private AudioManager audioManager;
        private int currentStreamVolume;

        //private Intent voiceIntent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.audioManager = (AudioManager)GetSystemService(Context.AudioService);
            this.speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(this);

            // Guardo el volumen que tenía cuando se ejcutó la aplicación.
            currentStreamVolume = audioManager.GetStreamVolume(Stream.Notification);

            speechRecognizer.Results += MSpeechRecognizer_Results;
            speechRecognizer.Error += MSpeechRecognizer_Error;

            speechRecognizer.BeginningOfSpeech += MSpeechRecognizer_BeginningOfSpeech;
            speechRecognizer.BufferReceived += MSpeechRecognizer_BufferReceived;
            speechRecognizer.EndOfSpeech += MSpeechRecognizer_EndOfSpeech;
            speechRecognizer.Event += MSpeechRecognizer_Event;
            speechRecognizer.PartialResults += MSpeechRecognizer_PartialResults;
            speechRecognizer.ReadyForSpeech += MSpeechRecognizer_ReadyForSpeech;
            speechRecognizer.RmsChanged += MSpeechRecognizer_RmsChanged;

            MuteMicSound();
        }

        private void MuteMicSound()
        {
            // Debugger.Log(1, "DEBUG", "Mute");
            // 
            // UpdateMicSound();
            // // Muteo para que no se escuche el ruido molesto de que se activa el micrófono.
            // audioManager.SetStreamVolume(Stream.Notification, 0, 0);
        }

        /// <summary>
        /// Guardo el volumen que tiene actualmente configurado el teléfono para los
        /// sonidos de sistema.
        /// </summary>
        private void UpdateMicSound()
        {               
            currentStreamVolume = audioManager.GetStreamVolume(Stream.Notification);
        }

        private void UnmuteMic()
        {
            // Debugger.Log(1, "DEBUG", "Unmute");
            // 
            // // this methods called when Speech Recognition is ready
            // // also this is the right time to un-mute system volume because the annoying sound played already
            // // again setting the system volume back to the original, un-mutting
            // audioManager.SetStreamVolume(Stream.Notification, currentStreamVolume, 0);
        }

        private void MSpeechRecognizer_ReadyForSpeech(object sender, ReadyForSpeechEventArgs e)
        {            
        }

        private void MSpeechRecognizer_BeginningOfSpeech(object sender, EventArgs e)
        {
        }

        private void MSpeechRecognizer_EndOfSpeech(object sender, EventArgs e)
        {
        }

        private void MSpeechRecognizer_PartialResults(object sender, PartialResultsEventArgs e)
        {            
        }

        private void MSpeechRecognizer_Event(object sender, EventEventArgs e)
        {            
        }

        private void MSpeechRecognizer_BufferReceived(object sender, BufferReceivedEventArgs e)
        {
        }

        private void MSpeechRecognizer_RmsChanged(object sender, RmsChangedEventArgs e)
        {
        }

        private void MSpeechRecognizer_Error(object sender, ErrorEventArgs e)
        {
            if (e.Error == SpeechRecognizerError.NoMatch)
            {
                //await IDoNotUnderstandAsync();
                WaitForCommand();                
            }
            else
            {
                Debugger.Log(1, "ERROR", $"{e.Error}");                
            }
        }

        private async void MSpeechRecognizer_Results(object sender, ResultsEventArgs e)
        {            
            var matches = e.Results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);

            if (matches.Count != 0)
            {
                await ProcessCommand(matches.First().ToLower().Trim());
                //string textInput = mFilterText.Text + matches[0];

                //mFilterText.Text = textInput;
            }
            else
            {
                await IDoNotUnderstandAsync();
                // mFilterText.Text = "Nothing was recognized";
            }
        }

        private async Task LoadSettings()
        {
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
            Debugger.Log(1, "DEBUG", message);
            StopRecognition();

            if (speakSettings == null)
            {
                await LoadSettings();
            }

            //UnmuteMic();
            await TextToSpeech.SpeakAsync(message, speakSettings).ContinueWith(t =>
            {
                //MuteMicSound();
                afterFinished();
            });
            
        }

        private void StopRecognition()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {                                 
                if (listening)
                {
                    Debugger.Log(1, "DEBUG", "Cancel");
                    speechRecognizer.Cancel();
                    listening = false;
                }
            });
        }

        protected void WaitForCommand()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                StopRecognition();

                if (!listening)
                {
                    // create the voice intent  
                    var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                    intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                    // end capturing speech if there is 3 seconds of silence  
                    intent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 3000);
                    intent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 3000);
                    intent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 30000);
                    intent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                    //intent.PutExtra(RecognizerIntent.ExtraPreferOffline, true);

                    // method to specify other languages to be recognised here if desired  
                    intent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

                    Debugger.Log(1, "DEBUG", "StartListening");

                    listening = true;

                    speechRecognizer.StartListening(intent);
                }
            });
        }

        //public async override void OnUserInteraction()
        //{
        //    base.OnUserInteraction();
        //}


        //protected async override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    if (requestCode == StartListening)
        //    {

        //    }
        //    else if (requestCode == VoiceCommandResult)
        //    {
        //        if (resultCode == Result.Ok)
        //        {
        //            var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
        //            if (matches.Count != 0)
        //            {
        //                await ProcessCommand(matches.First());
        //                //string textInput = mFilterText.Text + matches[0];

        //                //mFilterText.Text = textInput;
        //            }
        //            else
        //            {
        //                await IDoNotUnderstand();
        //                // mFilterText.Text = "Nothing was recognized";
        //            }
        //        }
        //    }

        //    base.OnActivityResult(requestCode, resultCode, data);

        //    // if (requestCode == VOICE)
        //    // {
        //    //     if (resultVal == Result.Ok)
        //    //     {
        //    //         var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
        //    //         if (matches.Count != 0)
        //    //         {
        //    //             string textInput = textBox.Text + matches[0];
        //    //             textBox.Text = textInput;
        //    //             switch (matches[0].Substring(0, 5).ToLower())
        //    //             {
        //    //                 case "north":
        //    //                     MovePlayer(0);
        //    //                     break;
        //    //                 case "south":
        //    //                     MovePlayer(1);
        //    //                     break;
        //    //             }
        //    //         }
        //    //         else
        //    //         {
        //    //             textBox.Text = "No speech was recognised";
        //    //         }
        //    //     }
        //    //     base.OnActivityResult(requestCode, resultVal, data);
        //    // }
        //}

        protected async Task IDoNotUnderstandAsync()
        {
            await Speak("No te entiendo. Inténtalo otra vez.", () =>
            {
                //listening = false;
                WaitForCommand();
            });
        }

        private async Task ProcessCommand(string command)
        {
            if (command == Commands.Exit)
            {
                await Speak($"Hasta pronto", () =>
                {
                    FinishAffinity();
                    //Finish();
                    //JavaSystem.Exit(0);
                });
            }
            else
            {
                var availableCommands = GetAvailableCommands().Select(p => p.Trim().ToLower()).ToArray();
            
                if (command == Commands.Command ||
                    command == Commands.Help)
                {
                    var list = availableCommands.ToList();
                    list.Add(Commands.Exit);
            
                    //await Speak($"Los comandos que entiendo son: '{string.Join("', '", list)}'", WaitForCommand);
                    await Speak($"Los comandos que entiendo son: {StringHelper.Join(list)}.", WaitForCommand);
                }
                else if (availableCommands.Contains(command))
                {
                    ExecuteCommand(command);
                }
                else
                {
                    await OnNotKnownCommand(command);
                }
            }
        }

        protected virtual async Task OnNotKnownCommand(string command)
        {
            await Speak($"No conozco el comando: {command}", WaitForCommand);
        }

        protected abstract void ExecuteCommand(string command);

        protected abstract IEnumerable<string> GetAvailableCommands();
        
        protected override void OnDestroy()
        {
            UnmuteMic();
            base.OnDestroy();
        }
    }
}
