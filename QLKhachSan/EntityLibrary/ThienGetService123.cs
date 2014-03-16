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
        public List<Log> GetListLog(string FromDay, string ToDay)
        {
            string DayNow = DateTime.Now.ToShortDateString();

            string DayNowStar = DayNow + " 12:00:00 AM";
            string DayNowEnd = DayNow + " 11:59:59 PM";
            if (FromDay != null && ToDay == null)
            {
                DayNowStar = FromDay + " 12:00:00 AM";
            }
            if (FromDay != null && ToDay != null)
            {
                DayNowStar = FromDay + " 12:00:00 AM";
                DayNowEnd = ToDay + " 11:59:59 PM";
            }

            DateTime team1 = Convert.ToDateTime(DayNowStar);
            DateTime team2 = Convert.ToDateTime(DayNowEnd);
            if (FromDay != null && ToDay == null)
            {
              var query = from p in db.Logs where p.CreatedDate>=team1 orderby p.Id descending select p;
              return query.ToList();
            }
            if (FromDay != null && ToDay != null)
            {
                var query = from p in db.Logs where p.CreatedDate >= team1 && p.CreatedDate<=team2 orderby p.Id descending select p;
                return query.ToList();
            }
            var query1 = from p in db.Logs orderby p.Id descending select p;
            return query1.ToList();
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


        #region ReportList

        public List<ReportEntity> GetReportNowDay(string FromDay,string ToDay)
        {
            string DayNow = DateTime.Now.ToShortDateString();

            //3/16/2014 12:00:00 AM

            string DayNowStar = DayNow + " 12:00:00 AM";
            string DayNowEnd = DayNow + " 11:59:59 PM";
            if (FromDay != null && ToDay == null)
            {
                DayNowStar = FromDay + " 12:00:00 AM";
            }
            if (FromDay!=null && ToDay!=null)
            {
                DayNowStar = FromDay + " 12:00:00 AM";
                DayNowEnd = ToDay + " 11:59:59 PM";
            }
          
            DateTime team1 = Convert.ToDateTime(DayNowStar);
            DateTime team2 = Convert.ToDateTime(DayNowEnd); 

            List<ReportEntity> List = new List<ReportEntity>();
            var query = from p in db.BookingInfoes where p.CheckOutDate>=team1 && p.CheckOutDate<=team2 && p.StatusId==4 select p;
            foreach (BookingInfo sv in query)
            {
                ReportEntity gt = new ReportEntity();
                gt.Rom = db.Services.Find(sv.RoomId).Name;
                gt.Name = sv.CustomerName;
                gt.CMND = sv.CustomerCardNo;
                gt.StarDay = sv.CheckingDate.ToString();
                gt.EndDay = sv.CheckOutDate.ToString();
                gt.PriceRom = GetPriceRoom(sv.Id);
                gt.NameService = GetNameService(sv.Id);
                gt.PriceService = GetPriceService(sv.Id);
                gt.SumPrice = (Convert.ToDecimal(gt.PriceRom) + Convert.ToDecimal(gt.PriceService)).ToString();
                List.Add(gt);
            }

            return List;
        }

        public string GetPriceRoom(int BookingInfoId)
        {
            string PriceRomm = "0";
            try
            {
                var query = (from p in db.BookingInfoDetails
                             join q in db.Services on p.ServiceId equals q.Id
                             where p.BookingInfoId == BookingInfoId && q.IsRoom == true
                             select p.Total).Sum();
                PriceRomm = query.ToString();
            }
            catch (Exception e) { }
            return PriceRomm;
        }
        public string GetNameService(int BookingInfoId)
        {
           
                string NameService = "Không";
                try
                {
                var query = from p in db.BookingInfoDetails
                            join q in db.Services on p.ServiceId equals q.Id
                            where p.BookingInfoId == BookingInfoId && q.IsRoom == false
                            select q;
                if (query.Count() > 0)
                {
                    NameService = "";
                    foreach (var sv in query)
                    {
                        NameService = NameService + sv.Name + ",";
                    }
                }
            }
            catch (Exception e) { }
            return NameService;
        }


        public string GetPriceService(int BookingInfoId)
        {
            string PriceRomm = "0";
            try
            {
                var query = (from p in db.BookingInfoDetails
                             join q in db.Services on p.ServiceId equals q.Id
                             where p.BookingInfoId == BookingInfoId && q.IsRoom == false
                             select p.Total).Sum();
                PriceRomm = query.ToString();
            }
            catch (Exception e) { }
            return PriceRomm;
        }
        #endregion

    }
}
