using Application.Configuration;
using Application.Errors;
using Application.Models;
using Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApiContext _connection;

        public UserService(ApiContext connection)
        {
            _connection = connection;
        }

        public async Task<User> Add(User value)
        {
            var entity = await _connection.User.FindAsync(value.Id);

            if (value.Id != 0)
                throw new AlreadyExistsException("Usuário já existente");

            await _connection.User.AddAsync(value);
            await _connection.SaveChangesAsync();

            return value;
        }

        public async Task Delete(int id)
        {
            var entity = await _connection.User.FindAsync(id);

            if (entity == null)
                throw new NotFoundException("Usuário não encontrado");

            _connection.User.Remove(entity);
            await _connection.SaveChangesAsync();
        }

        public async Task<User> Edit(User value)
        {
            var entity = await _connection.User.Where(e => e.Id == value.Id).FirstAsync();

            if (entity == null)
                throw new NotFoundException("Usuário não encontrado");

            //entity.Password = value.Nome;
            //entity.UpdatedDate = DateTime.Now;

            //_connection.Device.Update(entity);
            //await _connection.SaveChangesAsync();

            return entity;
        }

        public async Task<User> Get(int id)
        {
            var entity = await _connection.User.FindAsync(id);

            if (entity == null)
                throw new NotFoundException("Usuário não encontrado");

            return entity;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            List<User> result = new List<User>(); // _connection.User.OrderBy(f => f.Id).ToList();
            return await Task.FromResult(result);
        }

        public async Task<User> Get(string username, string password)
        {
            User? user = await _connection.User.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == x.Password).FirstOrDefaultAsync();

            if (user == null)
                throw new NotFoundException("Usuário não encontrado ou senha incorreta");

            return user;
        }
        
        public async Task<User> Get(string username)
        {
            User? user = await _connection.User.Where(x => x.Username.ToLower() == username.ToLower()).FirstOrDefaultAsync();

            if (user == null)
                throw new NotFoundException("Usuário não encontrado");

            return user;
        }
    }
}