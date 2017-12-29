using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AnnounceInfomationContext
    {
        /// <summary>
        /// Get all announcements in DB
        /// </summary>
        /// <returns>List of Annoucement_Infomation</returns>
        public List<Announce_Information> GetAll()
        {
            using (var ctx = new SASDBEntities())
            {
                return ctx.Announce_Information.ToList();
            }
        }
        /// <summary>
        /// Delete annoucements in DB
        /// </summary>
        /// <param name="announcementIDs"></param>
        public void Delete(List<int> announcementIDs)
        {
            using (var ctx = new SASDBEntities())
            {
                var allRec =
                    from ann in ctx.Announce_Information
                    where announcementIDs.Contains(ann.ANNOUNCE_ID)
                    select ann;
                ctx.Announce_Information.RemoveRange(allRec);
                ctx.SaveChanges();
            }
        }
        /// <summary>
        /// Add new announcement to Db
        /// </summary>
        /// <param name="coporation_code"></param>
        /// <param name="schedule_date"></param>
        /// <param name="post_date"></param>
        /// <param name="subject"></param>
        /// <param name="contents"></param>
        /// <param name="rcv_copo"></param>
        /// <param name="rcv_grade"></param>
        /// <param name="rcv_class"></param>
        /// <param name="rcv_user"></param>
        /// <param name="status"></param>
        public void Add(string coporation_code, DateTime schedule_date, DateTime post_date, 
            string subject, string contents, string rcv_copo, string rcv_grade, string rcv_class, string rcv_user, string status)
        {
            using (var ctx = new SASDBEntities())
            {
                ctx.Announce_Information.Add(new Announce_Information()
                {
                    COPORATION_CODE = coporation_code,
                    SCHEDULE_DATE = schedule_date,
                    POST_DATE = post_date,
                    SUBJECT = subject,
                    CONTENTS = contents,
                    RCV_COPO = rcv_copo,
                    RCV_GRADE = rcv_grade,
                    RCV_CLASS = rcv_class,
                    RCV_USER = rcv_user,
                    STATUS = status
                });
                ctx.SaveChanges();
            }
        }
    }
}
