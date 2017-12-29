using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class UserMasterContext
    {
        /// <summary>
        /// Get all Users in Db
        /// </summary>
        /// <returns>List of all Users</returns>
        public List<User_Master> GetAll()
        {
            using (var umc = new SASDBEntities())
            {
                return umc.User_Master.ToList();
            }
        }
        public bool IsUsernameExists(string user_ID)
        {
            using(var umc = new SASDBEntities())
            {
                return umc.User_Master.Any(x => x.USER_ID.Equals(user_ID));
            }
        }
        /// <summary>
        /// Find User in Db by User_ID
        /// </summary>
        /// <param name="user_ID">User_ID string</param>
        /// <returns>Users which satisfy user_id</returns>
        public User_Master GetSingleByUsername(string user_ID)
        {
            using (var umc = new SASDBEntities())
            {
                return umc.User_Master.FirstOrDefault(x => x.USER_ID == user_ID);
            }
        }
        /// <summary>
        /// Get user's class by user_ID
        /// </summary>
        /// <param name="user_ID"></param>
        /// <returns>List of user's class</returns>
        public List<string> GetUserClass(string user_ID)
        {

            List<string> userClasses = new List<string>();
            var existingUser = GetSingleByUsername(user_ID);
            if (existingUser != null)
            {
                userClasses.Add(existingUser.CLASS);
            }
            return userClasses;
        }
    }
}
