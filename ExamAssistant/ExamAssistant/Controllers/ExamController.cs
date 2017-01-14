using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamAssistant.Models;

namespace ExamAssistant.Controllers
{
    public class ExamController : Controller
    {
        private Admin _admin;
        private Student _student;

        public ExamController()
        {
            _admin = new Admin(ExamConfiguration.GetSetting("SettingsDir"));
            _student = _admin.Students.First(); //TODO: Implement login user
        }
        // GET: Exam
        public ActionResult Dashboard()
        {
            //TODO: Implement switch of view depending on logged in user
            //return View("Admin", _admin); //For Admins
            return View(_student);
        }

        public ActionResult Take(string selectedExam)
        {
            //TODO: Implement showing of the exam questions
            Exam exam = Admin.Exams.First(e => e.Name.Equals(selectedExam));
            return View(exam);
        }
    }
}