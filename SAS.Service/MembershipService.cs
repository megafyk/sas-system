using Data;
using SAS.Service.Abstract;
using SAS.Service.Utilities;
using System;
using System.Security.Principal;

namespace SAS.Service
{
    public class MembershipService : IMembershipService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public User_Master GetUser(string user_id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Validate username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>principle of current user</returns>
        public MembershipContext ValidateUser(string username, string password)
        {
            var membershipCtx = new MembershipContext();
            UserMasterContext umc = new UserMasterContext();
            var user = umc.GetSingleByUsername(username);
            if (user != null && isUserValid(user, password))
            {
                var userClass = umc.GetUserClass(username);
                membershipCtx.User_Master = user;

                var identity = new GenericIdentity(user.USER_ID.ToString(), "Basic");
                membershipCtx.Principal = new GenericPrincipal(identity, userClass.ToArray());
            }
            return membershipCtx;
        }
        private bool isUserValid(User_Master user, string password)
        {
            return string.Equals(user.PASSWORD, password);
        }
    }
}
