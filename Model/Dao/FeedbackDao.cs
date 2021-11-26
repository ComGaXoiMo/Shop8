using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.Dao
{
    public class FeedbackDao
    {
        Shop8DbContext db = null;
        public FeedbackDao()
        {
            db = new Shop8DbContext();
        }

        public int GetQuantityByMonth(int month)
        {
            return db.Feedbacks.Where(x => x.CreatedDate.Value.Month == month && x.CreatedDate.Value.Year == DateTime.Now.Year).Count();
        }
        //Cập nhật
        public IEnumerable<Feedback> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Feedback> Entity = db.Feedbacks;

            if (searchString == "Đã xem")
            {
                Entity = Entity.Where(x => x.Status == true);
            }
            else if (searchString == "Chưa xem")
            {
                Entity = Entity.Where(x => x.Status == false);
            }
            return Entity.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        //tìm theo id
        public Feedback GetByID(long id)
        {
            return db.Feedbacks.Find(id);
        }
        //thêm mới
        public bool Insert(Feedback entity)
        {
            try
            {
                db.Feedbacks.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        //Thay đổi trạng thái
        public bool ChangeStatus(long id)
        {
            var Entity = db.Feedbacks.SingleOrDefault(x => x.ID == id);
            Entity.Status = !Entity.Status;
            db.SaveChanges();

            return Entity.Status;
        }

    }
}
