using Microsoft.Identity.Client;
using MudBlazor;

namespace FineWoodworkingBasic.Service;
public class AccountLogin{
    static private bool OverlayIsOpen = false;
    static private bool LoggedIn = false;
    static private bool Register = false;

    private string? LinkAddress = "/";


    /// <summary>
    /// Get the linked address
    /// </summary>
    /// <returns>String of link address</returns>
    public string? GetLinkAddress(){
        return LinkAddress;
    }

    /// <summary>
    /// Change the link address 
    /// </summary>
    /// <param name="NewLinkAddress">Set current address to inputed address</param>
    public void SetLinkAddress(string NewLinkAddress){
        LinkAddress = NewLinkAddress;
    }

    /// <summary>
    /// Show the overlay of all the different states of login
    /// </summary>
    /// <returns>True: Current state of login view should be visiable; otherwise should not be</returns>
    public bool IsOverlayOpen(){
        return OverlayIsOpen;
    }


    /// <summary>
    /// Show the login view
    /// </summary>
    /// <returns>True: Login in view should be visiable; otherwise should not be</returns>
    public bool IsLoggedIn(){
        return LoggedIn;
    }

    /// <summary>
    /// Show the registration view
    /// </summary>
    /// <returns>True: Register page should be visiable; Otherwise should not be</returns>
    public bool NeedsRegister(){
        return Register;
    }

    /// <summary>
    /// Toggles the login overlay
    /// </summary>
    public void ToggleOverlay(){
        if (OverlayIsOpen)
        {
            OverlayIsOpen = false;
        }
        else{
            OverlayIsOpen = true;
        }
    }

    /// <summary>
    /// Set if the user should be logged in or not
    /// </summary>
    /// <param name="LoggedInStatu">True: login user; otherwise do not login user</param>
    public void ToggleLoggedIn(bool LoggedInStatus){
        LoggedIn = LoggedInStatus;
    }

    public void ToggleRegister()
    {
        if (Register)
        {
            Register = false;
        }
        else{
            Register = true;
        }
    }







}