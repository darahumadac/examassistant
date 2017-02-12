using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamAssistant.Models
{
    public interface IRepository
    {
        List<ExamInformation> GetExamList();
        List<User> GetStudentList();
        Exam GetExamById(string examId);
        List<StudentExam> GetExamsByUser(int userId);
        KeyValuePair<LoginStatus, User> VerifyUser(string username, string password);
        RegisterStatus AddUser(string username, string password, string firstName, string lastName, 
            int gradeLevel, string section, bool isAdmin);
        bool DeactivateUser(int student);
        bool ActivateUser(int student);
    }
}
