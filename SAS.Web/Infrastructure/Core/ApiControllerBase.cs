using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SAS.Web.Infrastructure.Core
{
    public class ApiControllerBase : ApiController
    {
        public  static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage request, Func<HttpResponseMessage> function)
        {
            HttpResponseMessage response = null;

            try
            {
                response = function.Invoke();
            }
            catch (DbUpdateException ex)
            {
                log.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

            return response;
        }
    }
}