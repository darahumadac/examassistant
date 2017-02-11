using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Xml;
using PagedList;

namespace ExamAssistant.Models
{
    public class AdminViewModel
    {
        public IPagedList<ExamInformation> Exams { get; set; }
        public IPagedList<User> Students { get; set; }
    }

    public class ExamInformation : DbRecord
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string ExamType { get; set; }
        public int TotalPoints { get; set; }
    }

    public class Student
    {
        private string _id;
        private string _name;
        private string _grade;
        private string _section;
        private string _password;
        private Dictionary<Exam, ExamInfo> _userExams;

        public Student(XmlNode studentNode)
        {

        }

        public string Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Grade
        {
            get { return _grade; }
        }

        public string Section
        {
            get { return _section; }
        }

        public string Password { get { return _password; }}

        public Dictionary<Exam, ExamInfo> Exams {
            get { return _userExams; }
        }
    }

    public class ExamInfo
    {
        private Exam _exam;
        private ExamStatus _status;
        private DateTime _date;

        public ExamStatus Status
        {
            get { return _status; }
        }

        public DateTime Date
        {
            get { return _date; }
        }
    }

    public enum ExamStatus
    {
        New,
        Completed
    }
}