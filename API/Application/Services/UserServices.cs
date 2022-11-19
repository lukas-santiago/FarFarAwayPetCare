using Application.Configuration;
using Application.Errors;
using Application.Models;
namespace Application.Services
{
    public class UserServices
    {
        private readonly ApiContext _connection;

        public UserServices(ApiContext connection)
        {
            _connection = connection;
        }
        public User Get(string username, string password)
        {
            User? user = _connection.User.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == x.Password).FirstOrDefault();

            if (user == null)
                throw new NotFoundException("Usuário não encontrado ou senha incorreta");
            
            return user;
        }
    }
}