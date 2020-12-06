using Shiny;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Fiuba.App7539.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            var but = Content.FindByName<Button>("testButton");
            but.Clicked += But_Clicked;
        }

        private async void But_Clicked(object sender, EventArgs e)
        {
            var locales = await TextToSpeech.GetLocalesAsync();

            // Grab the first locale
            var locale = locales.FirstOrDefault(l => l.Country.ToLower() == "es" && l.Language.ToLower() == "es");

            var settings = new SpeechOptions()
            {
                Volume = .75f,
                Pitch = 1.0f,
                Locale = locale
            };

            await TextToSpeech.SpeakAsync("Hola Popa", settings);

            //Shiny.ShinySpeechRecognizer
            //ShinySpeechRecognizer.Current
            //await TextToSpeech.SpeakAsync("Hello World");

            //TextToSpeech.SpeakAsync("Hello World").ContinueWith((t) =>
            //{
            //    // Logic that will run after utterance finishes.
            //
            //}, TaskScheduler.FromCurrentSynchronizationContext());

            //var acc = ShinySpeechRecognizer.RequestAccess().Result;
            //
            //if (acc == AccessState.Available)
            //{
            //    ShinySpeechRecognizer.ListenUntilPause().Subscribe(phrase =>
            //    {
            //        Trace.WriteLine(phrase);
            //    });
            //}
            //.Current
            //.ListenUntilPause()
            //.Subscribe(phrase =>
            //{
            //
            //});               

        }
    }
}