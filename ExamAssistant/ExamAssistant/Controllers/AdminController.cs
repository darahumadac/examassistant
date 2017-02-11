using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExamAssistant.Models;
using PagedList;

namespace ExamAssistant.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IRepository _repository;
        private int _recordsPerPage;

        public AdminController()
        {
            _repository = Startup.Repository;
            _recordsPerPage = int.Parse(ConfigurationManager.AppSettings["AdminPage_RowsDisplayedPerPage"]);
        }
        // GET: Admin
        public ActionResult Index()
        {
            int totalStudentCount = _repository.GetStudentList().Count;

            AdminViewModel adminViewModel = new AdminViewModel()
            {
                Exams = _repository.GetExamList().ToPagedList(1, _recordsPerPage),
                Students = _repository.GetStudentList().ToPagedList(1, _recordsPerPage)
            };

            return View(adminViewModel);
        }

        public ActionResult SearchStudents(string keyword)
        {
            if (Request.IsAjaxRequest())
            {
                IPagedList<User> searchResults = _repository.GetStudentList().ToPagedList(1, _recordsPerPage);
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    searchResults = _repository.GetStudentList()
                    .FindAll(s => s.FullName.ToLower().Contains(keyword.ToLower()) ||
                                  s.Username.Equals(keyword)).ToPagedList(1, _recordsPerPage);
                }

                return PartialView("_StudentListPartial", searchResults);
            }

            return RedirectToAction("Index");
        }

        public ActionResult GetStudentsInPage(int page)
        {
            if (Request.IsAjaxRequest())
            {
                IPagedList<User> students = _repository.GetStudentList().ToPagedList(page, _recordsPerPage);
                return PartialView("_StudentListPartial", students);
            }
            return RedirectToAction("Index");

        }

        public ActionResult GetExamsInPage(int page)
        {
            if (Request.IsAjaxRequest())
            {
                IPagedList<ExamInformation> exams = _repository.GetExamList().ToPagedList(page, _recordsPerPage);
                return PartialView("_ExamListPartial", exams);
            }
            return RedirectToAction("Index");

        }

    }
}