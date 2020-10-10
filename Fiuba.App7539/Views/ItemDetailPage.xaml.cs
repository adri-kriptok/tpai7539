using System.ComponentModel;
using Xamarin.Forms;
using Fiuba.App7539.ViewModels;

namespace Fiuba.App7539.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}