using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAssistant.Models
{
    public static class LoginManager
    {
        public static KeyValuePair<LoginStatus, User> Login(string username, string password)
        {
           return Startup.Repository.VerifyUser(username, password);
        }
        
    }

    public class UnauthorizedUser : User
    {
        public UnauthorizedUser()
        {
            Id = -1;
            Username = "Unauthorized";
        }
    }

    public enum LoginStatus
    {
        Fail,
        Success,
        LockedOut
    }
}