using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary
{
   public class ThienService
    {
       QLKhachSanEntities db = new QLKhachSanEntities();

       #region Users

       public int GetUser(string UserName, string Password)
       {
           var query = (from p in db.Users where p.UserName == UserName && p.Password == Password & p.IsActive == true select p);
           if (query.Count() == 1)
               return 1;
           return 0;
       }

       public User GetUsers(string UserName, string Password)
       {
           var query = (from p in db.Users where p.UserName == UserName && p.Password == Password & p.IsActive == true select p).SingleOrDefault();
           return query;
       }

       public User GetUsers(int ID)
       {
           var query = db.Users.Find(ID);
           return query;
       }


       public int InsertUser(User model)
       {
           try
           {
               db.Users.Add(model);

               db.SaveChanges();
               return 1;
           }
           catch(Exception e)
           {
               return 0;
           }
       }

       public int UpdateUser(User model)
       {
           try
           {
               User team = db.Users.First(i => i.Id==model.Id);
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



       #region BookingInfor
      
       public IList<BookingInfo> GetListBookingInfoByRoomId(int roomId)
       { 
        var q=from b in db.BookingInfoes 
              where !b.Deleted&&!b.HasChangeRoom&&b.RoomId==roomId&&b.StatusId!=(int)BookingInfoStatusEnums.PhongTrong&&b.StatusId!=(int)BookingInfoStatusEnums.PhongDaCheckOut select b;
              return q.ToList();
        
       }

       public BookingInfo GetBookingInforById(int ID)
       {
           var query = (from p in db.BookingInfoes where p.Id == ID  && p.Deleted != true select p).SingleOrDefault();
           return query;
       }
       public BookingInfo GetBookingInforByRoomId(int RoomID, int StatusID)
       {
           var query = (from p in db.BookingInfoes where p.Id == RoomID && p.StatusId == StatusID && p.Deleted != true select p).SingleOrDefault();
           return query;
       }

       public int InsertBookingInfo(BookingInfo model)
       {
           try
           {
               db.BookingInfoes.Add(model);
               db.SaveChanges();
               return 1;
           }
           catch (Exception e)
           {
               return 0;
           }
       }

       public int UpdateBookingInfo(BookingInfo model)
       {
           try
           {
               BookingInfo team = db.BookingInfoes.First(i => i.Id == model.Id);
               team.CustomerName = model.CustomerName;
               team.CustomerCardNo = model.CustomerCardNo;
               team.PhoneNumber = model.PhoneNumber;
               team.CustomerInfoOther = model.CustomerInfoOther;
               team.BookingDate = model.BookingDate;
               team.RoomId = model.RoomId;
               team.HasChangeRoom = model.HasChangeRoom;
               team.FromBookingInfoId = model.FromBookingInfoId;
               team.UserId = model.UserId;
               team.CreatedDate = model.CreatedDate;
               team.LastUpdateDate = model.LastUpdateDate;
               team.StatusId = model.StatusId;
               team.Deleted = model.Deleted;

               db.SaveChanges();

               return 1;
           }
           catch (Exception e)
           {
               return 0;
           }
       }



        #endregion


       #region BookingInfoDetail
       public BookingInfoDetail GetBookingInfoDetailById(int id)
       {
           var q = db.BookingInfoDetails.Where(x => x.Id.Equals(id)).FirstOrDefault();
           return q;
       }
       public IList<BookingInfoDetail> GetBookingInfoDetailByBookingInfoId(int id)
       {
           return db.BookingInfoDetails.Where(x => x.BookingInfoId == id).ToList();
                 
                  
       }
       public int DeleteBookingInfoDetail(BookingInfoDetail entity)
       {
           try
           {
               db.BookingInfoDetails.Remove(entity);
               db.SaveChanges();
               return 1;
           }
           catch
           {
            return 0;
           }
       }

       public int InsertBookingInfoDetail(BookingInfoDetail model)
       {
           try
           {
               db.BookingInfoDetails.Add(model);
               db.SaveChanges();
               return 1;
           }
           catch (Exception e)
           {
               return 0;
           }
       }

       public int UpdateBookingInfoDetail(BookingInfoDetail model)
       {
           try
           {
               BookingInfoDetail team = db.BookingInfoDetails.First(i => i.Id == model.Id);
               team.BookingInfoId = model.BookingInfoId;
               team.ServiceId = model.ServiceId;
               team.ServiceName = model.ServiceName;
               team.Price = model.Price;
               team.Quatity = model.Quatity;
               team.Total = model.Total;
               team.Note = model.Note;


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

       //public int UpdateLog(Log model)
       //{
       //    try
       //    {
       //        Log team = db.Logs.First(i => i.Id == model.Id);
       //        team.ActionTypeId = model.ActionTypeId;
       //        team.ActionName = model.ActionName;
       //        team.CreatedDate = model.CreatedDate;
       //        team.UserId = model.Id;

       //        db.SaveChanges();

       //        return 1;
       //    }
       //    catch (Exception e)
       //    {
       //        return 0;
       //    }
       //}



       #endregion


        #region Sevice

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
       public IList<Service> GetRooms(int type=0)
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
                   where !r.Deleted && r.IsActive && r.IsRoom && r.Id==id
                   select r;
           return q.FirstOrDefault();
       }
       public Service ServicebyId(int id)
       {
           var q = from r in db.Services
                   where !r.Deleted && r.IsActive && r.Id == id
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
       public bool CheckRoomContent(int roomId, DateTime date)
       {
           var q = from b in db.BookingInfoes
                   where b.StatusId !=(int)BookingInfoStatusEnums.PhongDaCheckOut && b.RoomId == roomId
                   && b.CheckingDate <=date && b.CheckOutDate >= date
                   select b;
           bool a = q.Count() > 0;
               return a;
           


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


       #endregion

       public void WriteLogAction(string TileLog, int UserId)
       {
           Log team = new Log();
           team.ActionName = TileLog;
           team.CreatedDate = DateTime.Now;
           team.UserId = UserId;
           InsertLog(team);
       }






       public IList<User> GetListUsers()
       {
           return db.Users.Where(x => x.IsActive).ToList();
       }

       public User GetUsersByID(int id)
       {
           return db.Users.Single(x => x.Id.Equals(id));
       }
    }
}
