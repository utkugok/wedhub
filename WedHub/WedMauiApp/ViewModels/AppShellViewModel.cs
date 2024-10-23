﻿using CommunityToolkit.Mvvm.Input;
using WedMauiApp.Views.Startup;

namespace WedMauiApp.ViewModels;

public partial class AppShellViewModel : BaseViewModel
{

    [RelayCommand]
    async void SignOut()
    {
        if (Preferences.ContainsKey(nameof(App.UserDetails)))
        {
            Preferences.Remove(nameof(App.UserDetails));
        }
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}