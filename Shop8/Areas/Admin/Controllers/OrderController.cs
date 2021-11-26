using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop8.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Oder
        public ActionResult Index(string searchString, int page = 1, int pageSize = 5)
        {
            ViewBag.SearchString = searchString; // Tìm kiếm
            int totalRecord = 0; // Tổng số bản ghi

            var dao = new OrderDao().ListAllPaging(searchString, ref totalRecord, page, pageSize); // Get DS Đơn Hàng
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5; // Trang hiển thị tối đa
            int totalPage = 0; // Tổng số trang

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            if (page < @ViewBag.Last) ViewBag.Next = page + 1;
            else ViewBag.Next = page;
            if (page > @ViewBag.First) ViewBag.Prev = page - 1;
            else ViewBag.Prev = page;

            return View(dao);
        }

        
    }
}