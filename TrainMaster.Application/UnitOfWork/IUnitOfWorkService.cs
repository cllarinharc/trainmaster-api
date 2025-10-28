using TrainMaster.Application.Services;

namespace TrainMaster.Application.UnitOfWork
{
    public interface IUnitOfWorkService
    {
        UserService UserService { get; }
        AuthService AuthService { get; }
        ProfilePessoalService ProfilePessoalService { get; }
        ProfileProfessionalService ProfileProfessionalService { get; }
        AddressService AddressService { get; }
        EducationLevelService EducationLevelService { get; }
        CourseService CourseService { get; }
        DepartmentService DepartmentService { get; }
        TeamService TeamService { get; }
        HistoryPasswordService HistoryPasswordService { get; }
        CourseAvaliationService CourseAvaliationService { get; }
        CourseActivitieService CourseActivitieService { get; }
        NotificationService NotificationService { get; }
        CourseFeedbackService CourseFeedbackService { get; }
        QuestionService QuestionService { get; }
        ExamService ExamService { get; }
        BadgeService BadgeService { get; }
        FaqService FaqService { get; }
    }
}