using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ContactDao
    {
        Shop8DbContext db = null;
        public ContactDao()
        {
            db = new Shop8DbContext();
        }

        //Tìm theo id
        public Contact GetByID(long id)
        {
            return db.Contacts.SingleOrDefault(x => x.ID == id);
        }

        public List<Contact> ListAll()
        {
            return db.Contacts.Where(x => x.Status == true).ToList();
        }
        
        public IEnumerable<Contact> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Contact> Entity = db.Contacts;
            if (!string.IsNullOrEmpty(searchString))
            {
                Entity = Entity.Where(x => x.Name.Contains(searchString));
            }
            return Entity.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }

        // Thêm mới
        public bool Insert(Contact Entity)
        {
            try
            {
                db.Contacts.Add(Entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        // Cập nhật
        public bool Update(Contact NewEntity)
        {
            try
            {
                var Entity = db.Contacts.Find(NewEntity.ID);

                Entity.Name = NewEntity.Content;
                Entity.Content = NewEntity.Content;
                Entity.Status = NewEntity.Status;

                db.SaveChanges(); 

                return true;

            }
            catch (Exception) { return false; }
        }
        // Xóa
        public bool Delete(long id)
        {
            try
            {
                var Entity = db.Contacts.SingleOrDefault(x => x.ID == id);
                db.Contacts.Remove(Entity);
                db.SaveChanges();

                return true;
            }
            catch (Exception) { return false; }
        }

        // Thay đổi trạng thái
        public bool ChangeStatus(long id)
        {
            var Entity = db.Contacts.SingleOrDefault(x => x.ID == id);
            Entity.Status = !Entity.Status;
            db.SaveChanges();

            return Entity.Status;
        }
    }
}
