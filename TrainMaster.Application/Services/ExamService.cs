using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class ExamService : IExamService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public ExamService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<Result<ExamEntity>> Add(ExamEntity entity)
        {
            await using var tx = _repositoryUoW.BeginTransaction();
            try
            {
                if (entity is null)
                    return Result<ExamEntity>.Error("Payload não pode ser nulo.");

                if (string.IsNullOrWhiteSpace(entity.Title))
                    return Result<ExamEntity>.Error("Title é obrigatório.");
                entity.StartAt = DateTime.SpecifyKind(entity.StartAt, DateTimeKind.Utc);
                entity.EndAt = DateTime.SpecifyKind(entity.EndAt, DateTimeKind.Utc);

                if (entity.EndAt <= entity.StartAt)
                    return Result<ExamEntity>.Error("EndAt deve ser maior que StartAt.");
                var course = await _repositoryUoW.CourseRepository.GetById(entity.CourseId);
                if (course is null)
                    return Result<ExamEntity>.Error("Curso não encontrado.");

                entity.CreateDate = DateTime.UtcNow;
                entity.ModificationDate = DateTime.UtcNow;

                await _repositoryUoW.ExamRepository.Add(entity);
                await _repositoryUoW.SaveAsync();
                await tx.CommitAsync();

                Log.Information("Prova criada com sucesso (Id: {ExamId}) para o curso {CourseId}", entity.Id, entity.CourseId);
                return Result<ExamEntity>.Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao adicionar prova");
                await tx.RollbackAsync();
                return Result<ExamEntity>.Error("Erro ao adicionar prova.");
            }
        }

        public async Task<List<ExamEntity>> GetAll()
        {
            using var tx = _repositoryUoW.BeginTransaction();
            try
            {
                var list = await _repositoryUoW.ExamRepository.GetAll();
                _repositoryUoW.Commit();
                return list;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao listar provas");
                tx.Rollback();
                throw new InvalidOperationException("Erro ao listar provas.");
            }
            finally
            {
                tx.Dispose();
            }
        }

        public async Task<ExamEntity?> GetById(int id)
        {
            using var tx = _repositoryUoW.BeginTransaction();
            try
            {
                var entity = await _repositoryUoW.ExamRepository.GetById(id);
                _repositoryUoW.Commit();
                return entity;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao buscar prova por ID");
                tx.Rollback();
                throw new InvalidOperationException("Erro ao buscar prova por ID.");
            }
            finally
            {
                tx.Dispose();
            }
        }
    }
}