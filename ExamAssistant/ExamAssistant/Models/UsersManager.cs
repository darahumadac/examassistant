using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAssistant.Models
{
    public static class UsersManager
    {
        public static RegisterStatus CreateUser(User user)
        {
            return Startup.Repository.AddUser
                (user.Username, user.Password, user.FirstName, user.LastName,
                user.GradeLevel, user.Section, user.IsAdmin);
        }
    }

    public enum RegisterStatus
    {
        NotStarted,
        Success,
        Fail,
        AlreadyExists
    }
}