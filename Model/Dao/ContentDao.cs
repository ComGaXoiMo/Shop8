using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList; 

namespace Model.Dao
{
    public class ContentDao
    {
        Shop8DbContext db = null;
        public ContentDao()
        {
            db = new Shop8DbContext();
        }

        //Tìm theo id
        public Content GetByID(long id)
        {
            return db.Contents.SingleOrDefault(x => x.ID == id);
        }

        public List<Content> ListAll()
        {
            return db.Contents.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).ToList();
        }

        public List<Content> ListNewContent(int top)
        {
            return db.Contents.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<Content> ListTopHotContent(int top)
        {
            return db.Contents.Where(x => x.Status == true && x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public IEnumerable<Content> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Content> Entity = db.Contents;
            if (!string.IsNullOrEmpty(searchString))
            {
                Entity = Entity.Where(x => x.Name.Contains(searchString));
            }
            return Entity.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        // Thêm mới
        public bool Insert(Content Entity)
        {
            try
            {
                db.Contents.Add(Entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception) 
            { 
                return false;
            }
        }

        // Cập nhật
        public bool Update(Content NewEntity)
        {
            try
            {
                var Entity = db.Contents.Find(NewEntity.ID);

                // Cập nhật
                Entity.Name = NewEntity.Name;
                Entity.MetaTitle = NewEntity.MetaTitle;
                Entity.Description = NewEntity.Description;
                Entity.Image = NewEntity.Image;
                Entity.Detail = NewEntity.Detail;
                Entity.ModifiedDate = NewEntity.ModifiedDate;
                Entity.ModifiedBy = NewEntity.ModifiedBy;
                Entity.MetaKeywords = NewEntity.MetaKeywords;
                Entity.MetaDescriptions = NewEntity.MetaDescriptions;
                Entity.Status = NewEntity.Status;
                Entity.TopHot = NewEntity.TopHot;

                db.SaveChanges(); //Lưu

                return true;

            }
            catch (Exception) 
            {
                return false; 
            }
        }
        // Xóa
        public bool Delete(long id)
        {
            try
            {
                var Entity = db.Contents.SingleOrDefault(x => x.ID == id);
                db.Contents.Remove(Entity);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            { 
                return false; 
            }
        }

        // Thay đổi trạng thái
        public bool ChangeStatus(long id)
        {
            var Entity = db.Contents.SingleOrDefault(x => x.ID == id);
            Entity.Status = !Entity.Status;
            db.SaveChanges();

            return Entity.Status;
        }
    }
}
