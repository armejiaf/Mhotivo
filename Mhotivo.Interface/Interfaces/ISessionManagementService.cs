﻿namespace Mhotivo.Interface.Interfaces
{
    public interface ISessionManagementService
    {
        bool LogIn(string userName, string password, bool remember = false, bool redirect=true);
        void LogOut(bool redirect = false);
        string GetUserLoggedName();
        string GetUserLoggedEmail();
        string GetUserLoggedRole();
        string GetUserLoggedId();
    }
}
