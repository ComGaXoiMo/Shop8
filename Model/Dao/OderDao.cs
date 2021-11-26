using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;
using PagedList;
namespace Model.Dao
{
    public class OrderDao
    {
        Shop8DbContext db = null;
        public OrderDao()
        {
            db = new Shop8DbContext();
        }

        public int OrderQuantity(int month)
        {
            return db.Orders.Where(x => x.CreatedDate.Value.Month == month).Count();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }

        public IQueryable<OrderViewModel> ListAll()
        {
            var Order = from order in db.Orders
                        join orderDetail in db.OrderDetails on order.ID equals orderDetail.OrderID
                        join product in db.Products on orderDetail.ProductID equals product.ID
                        select new
                        {
                            OrderID = order.ID,
                            CustomerName = order.ShipName,
                            ProductID = orderDetail.ProductID,
                            Quantity = orderDetail.Quantity,
                            Price = orderDetail.Price,
                            CreatedDate = order.CreatedDate,
                            Status = order.Status,
                        };

            var Entity = from kq in Order
                         group kq by kq.OrderID into g
                         select new OrderViewModel
                         {
                             OrderID = g.Key,
                             CustomerName = (from gg in g select gg.CustomerName).FirstOrDefault(),
                             Products = (from product in db.Products join gg in g on product.ID equals gg.ProductID select new ProductViewModel { ID = gg.ProductID, Name = product.Name, Quantity = gg.Quantity, Price = gg.Price }).ToList(),
                             PriceTotal = g.Sum(x => x.Price),
                             CreatedDate = (DateTime)(from gg in g select gg.CreatedDate).FirstOrDefault(),
                             Status = (from gg in g select gg.Status).FirstOrDefault(),
                         };
            return Entity;
        }
        public List<OrderViewModel> ListAllPaging(string searchString, ref int totalRecord, int page, int pageSize)
        {
            var Entity = ListAll();
            if (searchString == "Chưa thanh toán")
            {
                Entity = Entity.Where(x => x.Status == false);
            }
            else if (searchString == "Đã thanh toán")
            {
                Entity = Entity.Where(x => x.Status == true);
            }
            totalRecord = Entity.Count();
            return Entity.OrderBy(x => x.OrderID).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public enum FilterType
        {
            Day, Month, Year, Unpaid, Paid, All
        }

        // Lọc
        public IQueryable<OrderViewModel> Filter(DateTime date, FilterType typeTime, FilterType typePaid)
        {
            var Entity = ListAll();
            if (typeTime == FilterType.Month)
            {
                Entity = Entity.Where(x => x.CreatedDate.Value.Month == date.Month && x.CreatedDate.Value.Year == date.Year);
            }
            else if (typeTime == FilterType.Year)
            {
                Entity = Entity.Where(x => x.CreatedDate.Value.Year == date.Year);
            }

            if (typePaid == FilterType.Paid)
            {
                Entity = Entity.Where(x => x.Status == true);
            }
            else if (typePaid == FilterType.Unpaid)
            {
                Entity = Entity.Where(x => x.Status == false);
            }
            return Entity;
        }


        // Thay đổi trạng thái 
        public bool ChangeStatus(long id)
        {
            try
            {
                var Entity = db.Orders.Find(id);
                Entity.Status = !Entity.Status; 
                db.SaveChanges();

                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
