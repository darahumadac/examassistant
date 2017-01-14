using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExamAssistant.Controllers
{
    public class ExamController : Controller
    {
        // GET: Exam
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}