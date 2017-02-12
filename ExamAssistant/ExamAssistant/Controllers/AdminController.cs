﻿using System;
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
        
        public ActionResult Index()
        {
            AdminViewModel adminViewModel = new AdminViewModel()
            {
                Exams = _repository.GetExamList().ToPagedList(1, _recordsPerPage),
                Students =
                new KeyValuePair<string, IPagedList<User>>
                    (string.Empty, _repository.GetStudentList().ToPagedList(1, _recordsPerPage))
            };

            return View(adminViewModel);
        }

        public ActionResult GetStudentsInPage(int page, string keyword = "")
        {
            if (Request.IsAjaxRequest())
            {
                KeyValuePair<string, IPagedList<User>> students = getStudentList(keyword, page);
                return PartialView("_StudentListPartial", students);
            }
            return RedirectToAction("Index");

        }

        private KeyValuePair<string, IPagedList<User>> getStudentList(string keyword, int page)
        {
            IPagedList<User> searchResults = _repository.GetStudentList().ToPagedList(page, _recordsPerPage);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                searchResults = _repository.GetStudentList()
                .FindAll(s => s.FullName.ToLower().Contains(keyword.ToLower()) ||
                              s.Username.Equals(keyword)).ToPagedList(page, _recordsPerPage);
            }
            return new KeyValuePair<string, IPagedList<User>>(keyword, searchResults);
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

        public ActionResult DeactivateStudent(int student)
        {
            _repository.DeactivateUser(student);
            return GetStudentsInPage(1, string.Empty);
        }

        public ActionResult ActivateStudent(int student)
        {
            _repository.ActivateUser(student);
            return GetStudentsInPage(1, string.Empty);
        }

    }
}