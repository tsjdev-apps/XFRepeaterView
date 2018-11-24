using Xamarin.Forms;
using XFRepeaterView.ViewModels;

namespace XFRepeaterView
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((MainViewModel)BindingContext).LoadPersonsCommand.Execute(null);
        }
    }
}
