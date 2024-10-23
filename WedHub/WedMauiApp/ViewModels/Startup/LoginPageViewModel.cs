using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using WedMauiApp.Models;

namespace WedMauiApp.ViewModels.Startup;

public partial class LoginPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _password;

    #region Commands
    [RelayCommand]
    async void Login()
    {
        if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password))
        {
            var userDetails = new UserBasicInfo();
            userDetails.Email = Email;
            userDetails.FullName = "Test User Name";

            // Admin Role, Seller Role, Customer Role, Guest Role,

            if (Email.ToLower().Contains("customer"))
            {
                userDetails.RoleID = (int)Enums.RoleDetails.Customer;
                userDetails.RoleText = "Customer Role";
            }
            else if (Email.ToLower().Contains("wedding"))
            {
                userDetails.RoleID = (int)Enums.RoleDetails.Seller;
                userDetails.RoleText = "Seller Role";
            }
            else if (Email.ToLower().Contains("admin"))
            {
                userDetails.RoleID = (int)Enums.RoleDetails.Admin;
                userDetails.RoleText = "Admin Role";
            }
            else
            {
                userDetails.RoleID = (int)Enums.RoleDetails.Guest;
                userDetails.RoleText = "Guest Role";
            }


            // calling api 


            if (Preferences.ContainsKey(nameof(App.UserDetails)))
            {
                Preferences.Remove(nameof(App.UserDetails));
            }

            string userDetailStr = JsonConvert.SerializeObject(userDetails);
            Preferences.Set(nameof(App.UserDetails), userDetailStr);
            App.UserDetails = userDetails;
            await AppConstant.AddFlyoutMenusDetails();
        }


    }
    #endregion
}