using TrainMaster.Application.ExtensionError;
using TrainMaster.Domain.Dto;

namespace TrainMaster.Application.Services.Interfaces
{
    public interface ICalendarService
    {
        Task<Result<List<CalendarItemDto>>> GetByMonth(int year, int month);
    }
}