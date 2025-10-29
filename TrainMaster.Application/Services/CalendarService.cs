using Serilog;
using TrainMaster.Application.ExtensionError;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Domain.Dto;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;

namespace TrainMaster.Application.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IRepositoryUoW _repositoryUoW;

        public CalendarService(IRepositoryUoW repositoryUoW)
        {
            _repositoryUoW = repositoryUoW;
        }

        //public async Task<Result<List<CalendarItemDto>>> GetByMonth(int year, int month)
        //{
        //    using var transaction = _repositoryUoW.BeginTransaction();
        //    try
        //    {
        //        var first = new DateOnly(year, month, 1);
        //        var last = first.AddMonths(1).AddDays(-1);

        //        var startDateTimeUtc = DateTime.SpecifyKind(first.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
        //        var endDateTimeUtc = DateTime.SpecifyKind(last.ToDateTime(new TimeOnly(23, 59, 59)), DateTimeKind.Utc);

        //        var courses = await _repositoryUoW.CourseRepository.GetByPeriod(first, last);

        //        var items = courses
        //            .Where(c => c.StartDate <= endDateTimeUtc && c.EndDate >= startDateTimeUtc)
        //            .Select(c => new CalendarItemDto
        //            {
        //                Title = c.Name,
        //                Description = c.Description,
        //                StartDate = DateOnly.FromDateTime(c.StartDate),
        //                EndDate = DateOnly.FromDateTime(c.EndDate),
        //                Type = "Curso",
        //                CourseId = c.Id,
        //                ExamId = null,
        //                Location = null
        //            })
        //            .OrderBy(i => i.StartDate)
        //            .ToList();

        //        _repositoryUoW.Commit();
        //        return Result<List<CalendarItemDto>>.Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"Erro ao carregar calendário de cursos para {month}/{year}: {ex.Message}");
        //        transaction.Rollback();
        //        return Result<List<CalendarItemDto>>.Error("Erro ao carregar os eventos de cursos do mês solicitado.");
        //    }
        //    finally
        //    {
        //        transaction.Dispose();
        //    }
        //}

        public async Task<Result<List<CalendarItemDto>>> GetByMonth(int year, int month)
        {
            using var transaction = _repositoryUoW.BeginTransaction();
            try
            {
                var events = await _repositoryUoW.CalendarRepository.GetByMonth(year, month);

                var items = events
                    .Select(e => new CalendarItemDto
                    {
                        Title = e.Title,
                        Description = e.Description,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Type = e.Type,
                        CourseId = e.CourseId,
                        ExamId = e.ExamId,
                        Location = e.Location
                    })
                    .OrderBy(i => i.StartDate)
                    .ToList();

                _repositoryUoW.Commit();
                return Result<List<CalendarItemDto>>.Ok(items);
            }
            catch (Exception ex)
            {
                Log.Error($"Erro ao carregar calendário para {month}/{year}: {ex.Message}");
                transaction.Rollback();
                return Result<List<CalendarItemDto>>.Error("Erro ao carregar os eventos do mês solicitado.");
            }
            finally
            {
                transaction.Dispose();
            }
        }

    }
}