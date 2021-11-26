using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using Shop8.Common;
using PagedList;

namespace Shop8.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        public ActionResult Edit(int id)
        {

            var user = new UserDao().ViewDetail(id);
            SetViewBag(user.GroupID);
            return View(user);
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            SetViewBag();
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var encryptedMd5pas = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMd5pas;
                long id = dao.Insert(user);
                if (id > 0)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user thành công.");
                }
            }
            return View("Index");
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var encryptedMd5pas = Encryptor.MD5Hash(user.Password);
                    user.Password = encryptedMd5pas;
                }

                var result = dao.Update(user);
                if (result)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Sửa user thất bại.");
                }
            }
            SetViewBag(user.GroupID);
            return View("Edit");
        }

        public ActionResult Delete(long id)
        {
            var dao = new UserDao();
            dao.Delete(id);

            return RedirectToAction("Index");
        }
        public JsonResult ChangeStatus(long id)
        {
            var dao = new UserDao();
            var res = dao.ChangeStatus(id);
            return Json(new { status = res });
        }
        public void SetViewBag()
        {
            var dao = new UserGroupDao();
            ViewBag.GroupID = new SelectList(dao.ListAll(), "ID", "ID", null);
        }
        public void SetViewBag(string selectedId)
        {
            var dao = new UserGroupDao();
            ViewBag.GroupID = new SelectList(dao.ListAll(), "ID", "ID", selectedId);
        }
    }
}