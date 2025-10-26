using Serilog;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class CourseFeedbackService : ICourseFeedbackService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public CourseFeedbackService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        public async Task<List<CourseFeedbackEntity>> GetAll()
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var feedbacks = await _repositoryUoW.CourseFeedbackRepository.GetAll();
                _repositoryUoW.Commit();
                return feedbacks;
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao carregar todos os feedbacks: {ex.Message}");
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao carregar a lista de feedbacks.", ex);
            }
            finally
            {
                Log.Information("Consulta de todos os feedbacks finalizada.");
                transaction.Dispose();
            }
        }

        public async Task<List<CourseFeedbackEntity>> GetByCourseId(int courseId)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var feedbacks = await _repositoryUoW.CourseFeedbackRepository.GetByCourseId(courseId);
                _repositoryUoW.Commit();
                return feedbacks;
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao carregar feedbacks do curso {courseId}: {ex.Message}");
                transaction.Rollback();
                throw new InvalidOperationException("Erro ao carregar os feedbacks do curso.", ex);
            }
            finally
            {
                Log.Information($"Consulta de feedbacks do curso {courseId} finalizada.");
                transaction.Dispose();
            }
        }
    }
}