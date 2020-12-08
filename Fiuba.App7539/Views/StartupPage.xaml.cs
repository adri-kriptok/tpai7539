using Fiuba.App7539.ViewModels;
using Shiny;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Fiuba.App7539.Views
{
    /// <summary>
    /// https://stackoverflow.com/questions/33300609/xamarin-forms-mixing-contentpage-and-android-activity
    /// 
    /// 
    /// </summary>
    public partial class StartupPage : ContentPage
    {
        public StartupPage()
        {
            InitializeComponent();

            /// var but = Content.FindByName<Button>("testButton");
            /// but.Clicked += But_Clicked;

            BindingContext = new StartupViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

//            var welcomeText = "Bienvenido!\n\nHáblame y empecemos a hacer cosas juntos!";

//            var label = Content.FindByName<Label>("welcomeLabel");
//            label.Text = welcomeText;

//            var locales = await TextToSpeech.GetLocalesAsync();

//#if DEBUG
//            var locs = locales.Select(l =>
//            {
//                return $"{l.Language}-{l.Country}";
//            }).OrderBy(p => p).ToArray();
//#endif

//            // Grab the first locale
//            var locale = locales.FirstOrDefault(l => l.Country.ToLower() == "us" && l.Language.ToLower() == "es");

//            var settings = new SpeechOptions()
//            {
//                Volume = .75f,
//                Pitch = 1.0f,
//                Locale = locale                
//            };

//            await TextToSpeech.SpeakAsync(welcomeText, settings).ContinueWith(t =>
//            {
                
//            });
        }

        //private Action<Task> ReadyText()
        //{
        //    return Task.Factory.StartNew(() =>
        //    {
        //        ;
        //
        //    });
        //}

        //private async void But_Clicked(object sender, EventArgs e)
        //{


        //    await TextToSpeech.SpeakAsync("Hola Popa", settings);

        //    //Shiny.ShinySpeechRecognizer
        //    //ShinySpeechRecognizer.Current
        //    //await TextToSpeech.SpeakAsync("Hello World");

        //    //TextToSpeech.SpeakAsync("Hello World").ContinueWith((t) =>
        //    //{
        //    //    // Logic that will run after utterance finishes.
        //    //
        //    //}, TaskScheduler.FromCurrentSynchronizationContext());

        //    //var acc = ShinySpeechRecognizer.RequestAccess().Result;
        //    //
        //    //if (acc == AccessState.Available)
        //    //{
        //    //    ShinySpeechRecognizer.ListenUntilPause().Subscribe(phrase =>
        //    //    {
        //    //        Trace.WriteLine(phrase);
        //    //    });
        //    //}
        //    //.Current
        //    //.ListenUntilPause()
        //    //.Subscribe(phrase =>
        //    //{
        //    //
        //    //});               

        //}
    }
}