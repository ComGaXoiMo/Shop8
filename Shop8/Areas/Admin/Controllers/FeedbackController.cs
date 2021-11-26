using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop8.Areas.Admin.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: Admin/Feedback
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var result = new FeedbackDao().ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(result);
        }
        public ActionResult View(long id)
        {
            var result = new FeedbackDao().GetByID(id);
            if (result != null)
            {
                return View(result);
            }
            return View();
        }

    }
}