using TrainMaster.Domain.Entity;

namespace TrainMaster.Infrastracture.Repository.Interfaces
{
    public interface ICalendarRepository
    {
        Task<List<CalendarEntity>> GetByPeriod(DateOnly start, DateOnly end);
        Task<List<CalendarEntity>> GetByMonth(int year, int month);
    }
}