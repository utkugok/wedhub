using WedMauiApp.ViewModels.Startup;

namespace WedMauiApp.Views.Startup;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}