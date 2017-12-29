
using Data;
using System.Security.Principal;

namespace SAS.Service.Utilities
{
    public class MembershipContext
    {

        public IPrincipal Principal { get; set; }
        public User_Master User_Master { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return Principal != null;
        }
    }
}
