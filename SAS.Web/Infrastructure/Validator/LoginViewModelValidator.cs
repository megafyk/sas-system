using System.Linq;

namespace SAS.Web.Infrastructure.Validator
{
    public class LoginViewModelValidator
    {
        public bool ValidateLogin(string username, string password)
        {
            return (
                (!(string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))) &&
                (username.Length == 8 && password.Length == 8 && IsNummeric(password))
            );
        }
        private bool IsNummeric(string str)
        {
            return str.All(char.IsDigit);        
        }
    }
}