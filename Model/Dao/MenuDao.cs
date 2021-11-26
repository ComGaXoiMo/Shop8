using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList; //Phân trang MVC

namespace Model.Dao
{
    public class MenuDao
    {
        Shop8DbContext db = null;
        public MenuDao()
        {
            db = new Shop8DbContext();
        }

        // Tìm theo id
        public Menu GetByID(int id)
        {
            return db.Menus.SingleOrDefault(x => x.ID == id);
        }

        public List<Menu> ListByGroupId()
        {
            return db.Menus.Where(x=>x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public IEnumerable<Menu> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Menu> Entity = db.Menus;
            if (!string.IsNullOrEmpty(searchString))
            {
                Entity = Entity.Where(x => x.Text.Contains(searchString));
            }
            return Entity.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }

        // Thêm mới
        public bool Insert(Menu Entity)
        {
            try
            {
                db.Menus.Add(Entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        // Cập nhật
        public bool Update(Menu NewEntity)
        {
            try
            {
                var Entity = db.Menus.Find(NewEntity.ID);

                // Cập nhật
                Entity.Text = NewEntity.Text;
                Entity.Link = NewEntity.Link;
                Entity.DisplayOrder = NewEntity.DisplayOrder;
                Entity.Target = NewEntity.Target;
                Entity.Status = NewEntity.Status;
               

                db.SaveChanges(); // Lưu

                return true;

            }
            catch (Exception) { return false; }
        }
        // Xóa
        public bool Delete(int id)
        {
            try
            {
                var Entity = db.Menus.SingleOrDefault(x => x.ID == id);
                db.Menus.Remove(Entity);
                db.SaveChanges();

                return true;
            }
            catch (Exception) { return false; }
        }
        public bool ChangeStatus(long id)
        {
            var Entity = db.Menus.SingleOrDefault(x => x.ID == id);
            Entity.Status = !Entity.Status;
            db.SaveChanges();

            return Entity.Status;
        }
    }
}
