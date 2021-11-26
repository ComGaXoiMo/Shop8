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
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            SetViewBag();
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var productcategory = new ProductDao();
            var result = productcategory.GetByID(id);
            SetViewBag(result.CategoryID);
            return View(result);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            SetViewBag();
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                product.CreatedDate = DateTime.Now;
                product.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserName.ToString();

                if (dao.Insert(product))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm productCategory Thất bại.");
                }
            }
            SetViewBag(product.CategoryID);
            return View();
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductDao();
                product.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserName.ToString();
                product.ModifiedDate = DateTime.Now;
                if (dao.Update(product))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Sửa Productcategory thất bại.");
                }
            }
            return View();
        }

        public ActionResult Delete(long id)
        {
            var dao = new ProductDao();
            if (dao.Delete(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Xóa Productcategory thất bại.");
            }
            return View("Index");
        }
        public JsonResult ChangeStatus(long id)
        {
            var dao = new ProductDao();
            var res = dao.ChangeStatus(id);
            return Json(new { status = res });
        }
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}