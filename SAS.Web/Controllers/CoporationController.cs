using Data;
using SAS.Web.Infrastructure.Core;
using SAS.Web.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SAS.Web.Controllers
{
    [RoutePrefix("api/coporation")]
    public class CoporationController : ApiControllerBase
    {
        /// <summary>
        /// Get coporations in db
        /// </summary>
        /// <param name="request"></param>
        /// <param name="value"></param>
        /// <returns>json data include coporations</returns>
        [HttpGet]
        [Route("{value:int=1}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int value)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                try
                {
                    CoporationMasterContext cmc = new CoporationMasterContext();
                    List<Coporation_Master> list = cmc.GetAll();
                    List<CoporationViewModel> copos = new List<CoporationViewModel>();
                    foreach (Coporation_Master c_m in list)
                    {
                        copos.Add(new CoporationViewModel()
                        {
                            CoporationCode = c_m.COPORATION_CODE,
                            CoporationName = c_m.COPORATION_NAME,
                            CoporationAddress1 = c_m.COPORATION_ADDRESS1,
                            CoporationAddress2 = c_m.COPORATION_ADDRESS2,
                            CoporationAddress3 = c_m.COPORATION_ADDRESS3,
                            EmailAddress = c_m.EMAIL_ADDRESS
                        });
                    }
                    response = request.CreateResponse(HttpStatusCode.OK, copos);
                }catch(Exception ex)
                {
                    log.Error(ex);
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false});
                } 
                return response;
            });
        }
    }
}
