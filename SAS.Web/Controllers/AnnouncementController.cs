using Data;
using SAS.Web.Infrastructure.Core;
using SAS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SAS.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/announcement")]
    public class AnnouncementController : ApiControllerBase
    {
        private const string _STR_ADMIN = "01";
        private const string _STR_STAFF = "02";
        private const string _COPORATIONCODE_1 = "aaaa";
        private const string _COPORATIONCODE_2 = "bbbb";
        /// <summary>
        /// Get annoucements by page in db
        /// </summary>
        /// <param name="request">client's request header</param>
        /// <param name="page">page's number</param>
        /// <param name="pageSize">page's size</param>
        /// <param name="filter">filter records</param>
        /// <returns>json data include pageset of announcements</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("{page:int=0}/{pageSize=5}/{filter?}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int? page, int? pageSize, string filter = null)
        {
            int currentPage = (page == null) ? 0 : page.Value;
            int currentPageSize = pageSize.Value;
            return CreateHttpResponse(request, () =>
            {
            HttpResponseMessage response = null;
            List<Announce_Information> announcements = null;
            int totalRecords = new int();
            AnnounceInfomationContext annInfoContext = new AnnounceInfomationContext();
                try
                {
                    announcements = annInfoContext
                      .GetAll()
                      .OrderByDescending(ai => ai.POST_DATE)
                      .Skip(currentPage * currentPageSize)
                      .Take(currentPageSize)
                      .ToList();
                    totalRecords = annInfoContext.GetAll().Count();
                    List<AnnouncementViewModel> announcementsVM = new List<AnnouncementViewModel>();
                    string userClass = HttpContext.Current.User.IsInRole("01") ? "01" : "02";
                    foreach (Announce_Information a_i in announcements)
                    {
                        announcementsVM.Add(new AnnouncementViewModel()
                        {
                            AnnounceID = a_i.ANNOUNCE_ID,
                            ScheduleDate = a_i.SCHEDULE_DATE,
                            PostDate = (DateTime)a_i.POST_DATE,
                            Contents = a_i.CONTENTS,
                            Subject = a_i.SUBJECT,
                            Rcv_Copo = a_i.RCV_COPO,
                            Rcv_Group = a_i.RCV_GRADE,
                            Rcv_Class = a_i.RCV_CLASS,
                            Rcv_User = a_i.RCV_USER,
                            UserClass = userClass,
                            Status = a_i.STATUS
                        });
                    }
                    PaginationSet<AnnouncementViewModel> pagedSet = new PaginationSet<AnnouncementViewModel>()
                    {
                        Page = currentPage,
                        TotalCount = totalRecords,
                        TotalPages = (int)Math.Ceiling((decimal)totalRecords / currentPageSize),
                        Items = announcementsVM
                    };

                    response = request.CreateResponse<PaginationSet<AnnouncementViewModel>>(HttpStatusCode.OK, pagedSet);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
                }

                return response;
            });
        }
        /// <summary>
        /// Delete announcements in db
        /// </summary>
        /// <param name="request">client's request header</param>
        /// <param name="records">records data</param>
        /// <returns>json data with bool attribute 'success'</returns>
        [HttpPost]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, List<int> records)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                AnnounceInfomationContext annInfoContext = new AnnounceInfomationContext();
                try
                {
                    annInfoContext.Delete(records);
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
                }
                return response;
            });
        }
        /// <summary>
        /// Create a new annoucement in db
        /// </summary>
        /// <param name="request">client's request header</param>
        /// <param name="ann">new announcement's data</param>
        /// <returns>json data with bool attribute 'success'</returns>
        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create(HttpRequestMessage request, AnnouncementViewModel ann)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                AnnounceInfomationContext annInfoContext = new AnnounceInfomationContext();
                try
                {
                    string coporation_code = string.Empty;
                    if (HttpContext.Current.User.IsInRole(_STR_ADMIN))
                    {
                        coporation_code = _COPORATIONCODE_1;
                    }
                    else
                    {
                        coporation_code = _COPORATIONCODE_2;
                    }
                    annInfoContext.Add(
                        coporation_code,
                        ann.ScheduleDate,
                        DateTime.Now,
                        ann.Subject,
                        ann.Contents,
                        ann.Rcv_Copo,
                        ann.Rcv_Group,
                        ann.Rcv_Class,
                        ann.Rcv_User,
                        "00");
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });
                }
                return response;
            });
        }
    }
}
