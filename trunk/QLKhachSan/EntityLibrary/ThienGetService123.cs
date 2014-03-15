using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
    public class ThienGetService123
    {
        QLKhachSanEntities db = new QLKhachSanEntities();

        #region Users

        public int GetUser(string UserName, string Password)
        {
            var query = (from p in db.Users where p.UserName == UserName && p.Password == Password && p.IsActive == true select p);
            if (query.Count() == 1)
                return 1;
            return 0;
        }

        public List<User> GetListUsers()
        {
            var query = (from p in db.Users where p.IsActive == true select p);
            return query.ToList();
        }

        public User GetUsers(string UserName, string Password)
        {
            var query = (from p in db.Users where p.UserName == UserName && p.Password == Password && p.IsActive == true select p).SingleOrDefault();
            return query;
        }

        public User GetUsersByID(int ID)
        {
            var query = db.Users.Find(ID);
            return query;
        }


        public int InsertUser(User model)
        {
            var query = (from p in db.Users where p.UserName == model.UserName select p);
            if (query.Count() >= 1)
                return 0;
            try
            {

                db.Users.Add(model);

                db.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int UpdateUser(User model)
        {
            try
            {
                User team = db.Users.First(i => i.Id == model.Id);
                team.UserName = model.UserName;
                team.Password = model.Password;
                team.Name = model.Name;
                team.IsActive = model.IsActive;
                team.CreateDate = model.CreateDate;
                team.LastUpdateDate = model.LastUpdateDate;
                team.PermissonId = model.PermissonId;

                db.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        #endregion



        


        #region Log

        public int InsertLog(Log model)
        {
            try
            {
                db.Logs.Add(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public void WriteLogAction(string TileLog, int UserId)
        {
            Log team = new Log();
            team.ActionName = TileLog;
            team.CreatedDate = DateTime.Now;
            team.UserId = UserId;
            InsertLog(team);
        }

        public List<Log> GetListLog()
        {
            var query = from p in db.Logs orderby p.Id descending select p;
            return query.ToList();
        }




        #endregion


        #region Sevice

        public List<Service> GetListSevice(bool IsRoom)
        {
            var query = from p in db.Services where p.Deleted == false && p.IsRoom == IsRoom select p;
            return query.ToList();
        }


        public Service GetServiceById(int id)
        {
            Service kq = db.Services.Find(id);
            return kq;
        }

        public int InsertService(Service model)
        {
            try
            {
                db.Services.Add(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int UpdateService(Service model)
        {
            try
            {
                Service team = db.Services.First(i => i.Id == model.Id);

                team.Name = model.Name;
                team.FloorNumber = model.FloorNumber;
                team.IsRoom = model.IsRoom;
                team.ServiceTypeId = model.ServiceTypeId;
                team.Price = model.Price;
                team.Description = model.Description;
                team.IsActive = model.IsActive;
                team.Deleted = model.Deleted;

                db.SaveChanges();

                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public IList<Service> GetRooms(int type = 0)
        {
            var q = from r in db.Services
                    where !r.Deleted && r.IsActive && r.IsRoom
                    select r;
            if (type != 0)
                q = q.Where(x => x.ServiceTypeId.Equals(type));
            return q.ToList();
        }
        public Service GetRoomById(int id)
        {
            var q = from r in db.Services
                    where !r.Deleted && r.IsActive && r.IsRoom && r.Id == id
                    select r;
            return q.FirstOrDefault();
        }
        public IList<Service> GetServiceS()
        {
            var q = from r in db.Services
                    where !r.Deleted && r.IsActive && !r.IsRoom
                    select r;

            return q.ToList();
        }

        #endregion



        #region ServiceType

        public int InsertSeviceType(ServiceType model)
        {
            try
            {
                db.ServiceTypes.Add(model);
                db.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int UpdateServiceType(ServiceType model)
        {
            try
            {
                ServiceType team = db.ServiceTypes.First(i => i.Id == model.Id);

                team.Id = model.Id;
                team.Name = model.Name;

                db.SaveChanges();

                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public ServiceType GetServiceTypeById(int p)
        {
            var q = db.ServiceTypes.Where(x => x.Id.Equals(p)).FirstOrDefault();
            return q;
        }

        public List<ServiceType> GetListServiceType()
        {
            var q = from p in db.ServiceTypes select p;
            return q.ToList();
        }

        #endregion
    }
}
