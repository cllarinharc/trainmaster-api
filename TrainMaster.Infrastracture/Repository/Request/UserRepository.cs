using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserEntity> Add(UserEntity userEntity)
        {
            if (userEntity is null)
                throw new ArgumentNullException(nameof(userEntity), "User cannot be null");

            var result = await _context.UserEntity.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public UserEntity Delete(UserEntity userEntity)
        {
            var response = _context.UserEntity.Remove(userEntity);
            return response.Entity;
        }

        public async Task<List<UserDto>> Get()
        {
            return await _context.UserEntity
                .AsNoTracking()
                .OrderBy(user => user.Id)
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Cpf = user.Cpf,
                    IsActive = user.IsActive
                })
                .ToListAsync();
        }

        public async Task<List<UserDto>> GetPaginated(int pageNumber, int pageSize)
        {
            return await _context.UserEntity
                .AsNoTracking()
                .OrderBy(user => user.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize) 
                .Select(user => new UserDto
                {
                    Id = user.Id, 
                    Email = user.Email,
                    Cpf = user.Cpf,
                    IsActive = user.IsActive
                })
                .ToListAsync();
        }


        public async Task<List<UserEntity>> GetAllActives()
        {
            return await _context.UserEntity
                .Where(user => user.IsActive)
                .ToListAsync();
        }

        public async Task<UserEntity?> GetById(int? id)
        {
            return await _context.UserEntity.FirstOrDefaultAsync(userEntity => userEntity.Id == id);
        }

        public async Task<UserEntity?> GetByEmail(string? email)
        {
            return await _context.UserEntity.FirstOrDefaultAsync(userEntity => userEntity.Email == email);
        }

        public async Task<UserEntity?> GetByCpf(string? cpf)
        {
            return await _context.UserEntity.FirstOrDefaultAsync(userEntity => userEntity.Cpf == cpf);
        }

        public UserEntity Update(UserEntity userEntity)
        {
            var response = _context.UserEntity.Update(userEntity);
            return response.Entity;
        }

        public UserEntity UpdateByActive(int userId, bool isActive)
        {
            var user = _context.UserEntity.Find(userId);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.IsActive = false;
            _context.UserEntity.Attach(user).Property(u => u.IsActive).IsModified = true;
            _context.SaveChanges();

            return user;
        }
    }
}