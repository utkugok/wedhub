using WedMauiApp.ViewModels;

namespace WedMauiApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        this.BindingContext = new AppShellViewModel();
    }
}
