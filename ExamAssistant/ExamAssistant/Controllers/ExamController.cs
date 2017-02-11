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
        public ExamController()
        {
             //TODO: Implement login user
        }
        // GET: Exam
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Take(string selectedExam)
        {
            //TODO: Implement showing of the exam questions
            return View();
        }

        //TODO: Add Exam - Add option to add exam from file or create new exam

    }
}