using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop8.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Admin/Contact
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContactDao();
            var result = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.searchString = searchString;
            return View(result);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContactDao();
                if (dao.Insert(contact))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật tin tức không thành công!");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ContactDao();
            var result = dao.GetByID(id);

            if (result != null)
            {
                return View(result);
            }
            else return View();
        }

        [HttpPost]
        public ActionResult Edit(Contact Entity)
        {
            if (ModelState.IsValid)
            {
                var dao = new ContactDao();
                if (dao.Update(Entity))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật tin tức không thành công!");
                }
            }
            return View();
        }

        public ActionResult Delete(long id)
        {
            var dao = new ContactDao();
            if (dao.Delete(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ContactDao().ChangeStatus(id);
            return Json(new { status = result });
        }
    }
}