using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.Request
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext _context;

        public DepartmentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<DepartmentEntity> Add(DepartmentEntity departmentEntity)
        {
            var result = await _context.DepartmentEntity.AddAsync(departmentEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public DepartmentEntity Delete(DepartmentEntity departmentEntity)
        {
            var response = _context.DepartmentEntity.Remove(departmentEntity);
            return response.Entity;
        }

        public async Task<List<DepartmentEntity>> Get()
        {
            return await _context.DepartmentEntity
             .OrderBy(department => department.Id)
             .Select(department => new DepartmentEntity
             {
                 Id = department.Id,
                 Name = department.Name,
                 Description = department.Description,
                 IsActive = department.IsActive,
             }).ToListAsync();
        }

        public async Task<DepartmentEntity?> GetById(int? id)
        {
            return await _context.DepartmentEntity.FirstOrDefaultAsync(department => department.Id == id);
        }

        public async Task<DepartmentEntity?> GetByUserId(int? id)
        {
            return await _context.DepartmentEntity.FirstOrDefaultAsync(department => department.UserId == id);
        }
        public async Task<DepartmentEntity> GetByUserId(int userId)
        {
            return await _context.DepartmentEntity
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<DepartmentEntity?> GetByName(string? name)
        {
            return await _context.DepartmentEntity.FirstOrDefaultAsync(department => department.Name == name);
        }

        public DepartmentEntity Update(DepartmentEntity departmentEntity)
        {
            var response = _context.DepartmentEntity.Update(departmentEntity);
            return response.Entity;
        }
    }
}