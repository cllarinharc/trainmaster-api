using Microsoft.EntityFrameworkCore.Storage;
using TrainMaster.Infrastracture.Repository.Interfaces;

namespace TrainMaster.Infrastracture.Repository.RepositoryUoW
{
    public interface IRepositoryUoW
    {
        IUserRepository UserRepository { get; }
        IPessoalProfileRepository PessoalProfileRepository { get; }
        IAddressRepository AddressRepository { get; }
        IEducationLevelRepository EducationLevelRepository { get; }
        IProfessionalProfileRepository ProfessionalProfileRepository { get; }
        ICourseRepository CourseRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        ITeamRepository TeamRepository { get; }
        IHistoryPasswordRepository HistoryPasswordRepository { get; }
        ICourseAvaliationRepository CourseAvaliationRepository { get; }
        ICourseActivitieRepository CourseActivitieRepository { get; }
        INotificationRepository NotificationRepository { get; }
        ICourseFeedbackRepository CourseFeedbackRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IQuestionOptionRepository QuestionOptionRepository { get; }
        IExamRepository ExamRepository { get; }
        IExamQuestionRepository ExamQuestionRepository { get; }
        IBadgeRepository BadgeRepository { get; }
        IFaqRepository FaqRepository { get; }
        ICalendarRepository CalendarRepository { get; }
        IExamHistoryRepository ExamHistoryRepository { get; }
        ICourseEnrollmentRepository CourseEnrollmentRepository { get; }
        ICourseProgressRepository CourseProgressRepository { get; }
        ICourseActivityProgressRepository CourseActivityProgressRepository { get; }

        Task SaveAsync();
        void Commit();
        IDbContextTransaction BeginTransaction();
    }
}