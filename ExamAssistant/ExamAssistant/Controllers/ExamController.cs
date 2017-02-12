using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamAssistant.Models;
using PagedList;

namespace ExamAssistant.Controllers
{
    public class ExamController : Controller
    {
        private IRepository _repository;
        private int _recordsPerPage;
        private string _loggedInUsername;

        public ExamController()
        {
            _repository = Startup.Repository;
            _recordsPerPage = Startup.RecordsPerPage;
            
        }
        // GET: Exam
        public ActionResult Dashboard()
        {
            List<StudentExam> userExams = _repository.GetExamsByUser(User.Identity.Name);
            ExamDashboardViewModel dashboardViewModel = new ExamDashboardViewModel()
            {
                UnfinishedExams = userExams.Where(e => !e.IsCompleted).ToPagedList(1, _recordsPerPage),
                CompletedExams = userExams.Where(e => e.IsCompleted).ToPagedList(1, _recordsPerPage)
            };
            
            return View(dashboardViewModel);
        }

        public ActionResult GetExamsInPage(int page, StudentExamPartialViewModel model)
        {
            if (Request.IsAjaxRequest())
            {
                IPagedList<StudentExam> userExams = 
                    _repository.GetExamsByUser(User.Identity.Name)
                    .Where(e => e.IsCompleted == model.IsCompleted).ToPagedList(page, _recordsPerPage);

                StudentExamPartialViewModel viewModel = new StudentExamPartialViewModel()
                {
                    Exams = userExams,
                    ExamTableId = model.ExamTableId,
                    PagerId = model.PagerId,
                    IsCompleted = model.IsCompleted
                };

                return PartialView("_StudentExamPartial", viewModel);
            }
            return RedirectToAction("Dashboard");
        }

        public ActionResult Take(string selectedExam)
        {
            //TODO: Implement showing of the exam questions
            return View();
        }

        //TODO: Add Exam - Add option to add exam from file or create new exam

    }
}