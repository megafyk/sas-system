using Data;
using SAS.Service;
using SAS.Service.Abstract;
using SAS.Service.Utilities;
using SAS.Web.Infrastructure.Core;
using SAS.Web.Infrastructure.Validator;
using SAS.Web.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SAS.Web.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiControllerBase
    {
        private IMembershipService _membershipService = null;
        /// <summary>
        /// Validate user when user login
        /// </summary>
        /// <param name="request">client's request header</param>
        /// <param name="user">user login data</param>
        /// <returns> Response json data with bool attribute 'success' </returns>
        [AllowAnonymous]
        [Route("authenticate")]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request, LoginViewModel user)
        {
            var _Session = HttpContext.Current;
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                bool isAuthUser = false;
                string msg = string.Empty;
                try
                {
                    if (ModelState.IsValid)
                    {
                        LoginViewModelValidator validator = new LoginViewModelValidator();
                        // Validate user in db
                        if (validator.ValidateLogin(user.Username, user.Password))
                        {

                            UserMasterContext umc = new UserMasterContext();
                            if (umc.IsUsernameExists(user.Username.ToLower()))
                            {
                                _membershipService = new MembershipService();
                                MembershipContext userContext = _membershipService.ValidateUser(user.Username, user.Password);
                                isAuthUser = (userContext.Principal != null);
                                if (!isAuthUser) msg = "Username or Password is incorrect.";
                            }
                            else
                            {
                                msg = "Username is not exists.";
                                isAuthUser = false;
                            }
                        }
                        else
                        {
                            isAuthUser = false;
                        }
                    }
                    else
                    {
                        isAuthUser = false;
                    }
                    response = (isAuthUser) ? 
                    request.CreateResponse(HttpStatusCode.OK, new { success = true }): 
                    request.CreateResponse(HttpStatusCode.OK, new { success = false, serverMsg = msg});
                }
                catch (Exception e)
                {
                    log.Error(e);
                    request.CreateResponse(HttpStatusCode.OK, new { success = false });
                }
                return response;
            });
        }
    }
}
