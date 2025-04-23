using RestoGestApp.ViewModels;

namespace RestoGestApp.Services;

public class AuthGuardService
{
    private readonly UserViewModel _userViewModel;
    
    public AuthGuardService(UserViewModel userViewModel)
    {
        _userViewModel = userViewModel;
    }
    
    public bool IsAuthenticated => _userViewModel.IsLoggedIn;
    
    public async Task<bool> CheckAuthenticationAsync()
    {
        if (!IsAuthenticated)
        {
            // Redirect to login page
            await Shell.Current.GoToAsync("//login");
            return false;
        }
        
        return true;
    }
}
