using Model.Dao;
using Model.EF;
using Shop8.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop8.Areas.Admin.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Admin/Content
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ContentDao();
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
        public ActionResult Create(Content Entity)
        {
            if (ModelState.IsValid)
            {
                Entity.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserName.ToString();  // Người tạo
                Entity.CreatedDate = DateTime.Now;  // Thời gian tạo

                var dao = new ContentDao();
                if (dao.Insert(Entity))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới tin tức không thành công!");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var dao = new ContentDao();
            var result = dao.GetByID(id);

            if (result != null)
            {
                return View(result);
            }
            else return View();
        }
        [HttpPost]
        public ActionResult Edit(Content Entity)
        {
            if (ModelState.IsValid)
            {
                Entity.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserName.ToString(); // Người cập nhật
                Entity.ModifiedDate = DateTime.Now; // Thời gian cập nhật

                var dao = new ContentDao();
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
            var dao = new ContentDao();
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
            var result = new ContentDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

    }
}