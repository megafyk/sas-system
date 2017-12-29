
using Data;
using SAS.Service.Utilities;

namespace SAS.Service.Abstract
{
    public interface IMembershipService
    {
        MembershipContext ValidateUser(string username, string password);
        User_Master GetUser(string user_id);
    }
}
