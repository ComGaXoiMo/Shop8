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
    public class ProductCategoryController : BaseController
    {
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            var dao = new ProductCategoryDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var productcategory = new ProductCategoryDao();
            var result = productcategory.GetByID(id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                productCategory.CreatedDate = DateTime.Now;
                productCategory.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserName.ToString();
                
                if (dao.Insert(productCategory))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm productCategory Thất bại.");
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                var dao = new ProductCategoryDao();
                productCategory.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserName.ToString(); 
                productCategory.ModifiedDate = DateTime.Now;
                if (dao.Update(productCategory))
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
            var dao = new ProductCategoryDao();
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
            var dao = new ProductCategoryDao();
            var res = dao.ChangeStatus(id);
            return Json(new { status = res });
        }
    }
}