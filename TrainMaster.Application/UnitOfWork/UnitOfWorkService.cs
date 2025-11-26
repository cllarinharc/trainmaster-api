using TrainMaster.Application.Services;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Infrastracture.Security.Cryptography;
using TrainMaster.Infrastracture.Security.Token.Access;

namespace TrainMaster.Application.UnitOfWork
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IRepositoryUoW _repositoryUoW;
        private readonly TokenService _tokenService;
        private readonly BCryptoAlgorithm _crypto;

        private UserService userService;
        private ProfilePessoalService profilePessoalService;
        private ProfileProfessionalService profileProfessionalService;
        private AuthService authService;
        private AddressService addressService;
        private EducationLevelService educationLevelService;
        private CourseService courseService;
        private DepartmentService departmentService;
        private TeamService teamService;
        private HistoryPasswordService historyPasswordService;
        private CourseAvaliationService courseAvaliationService;
        private CourseActivitieService courseActivitieService;
        private NotificationService notificationService;
        private CourseFeedbackService courseFeedbackService;
        private QuestionService questionService;
        private ExamService examService;
        private BadgeService badgeService;
        private FaqService faqService;
        private CalendarService calendarService;
        private ExamHistoryService examHistoryService;
        private CourseEnrollmentService courseEnrollmentService;

        public UnitOfWorkService(IRepositoryUoW repositoryUoW, TokenService tokenService, BCryptoAlgorithm crypto)
        {
            _repositoryUoW = repositoryUoW;
            _tokenService = tokenService;
            _crypto = crypto;
        }

        public ExamHistoryService ExamHistoryService
        {
            get
            {
                if (examHistoryService is null)
                    examHistoryService = new ExamHistoryService(_repositoryUoW);
                return examHistoryService;
            }
        }

        public CalendarService CalendarService
        {
            get
            {
                if (calendarService is null)
                    calendarService = new CalendarService(_repositoryUoW);
                return calendarService;
            }
        }

        public FaqService FaqService
        {
            get
            {
                if (faqService is null)
                    faqService = new FaqService(_repositoryUoW);
                return faqService;
            }
        }

        public BadgeService BadgeService
        {
            get
            {
                if (badgeService is null)
                    badgeService = new BadgeService(_repositoryUoW);
                return badgeService;
            }
        }

        public ExamService ExamService
        {
            get
            {
                if (examService is null)
                    examService = new ExamService(_repositoryUoW);
                return examService;
            }
        }

        public QuestionService QuestionService
        {
            get
            {
                if (questionService is null)
                    questionService = new QuestionService(_repositoryUoW);
                return questionService;
            }
        }

        public CourseFeedbackService CourseFeedbackService
        {
            get
            {
                if (courseFeedbackService is null)
                    courseFeedbackService = new CourseFeedbackService(_repositoryUoW);
                return courseFeedbackService;
            }
        }

        public TeamService TeamService
        {
            get
            {
                if (teamService is null)
                    teamService = new TeamService(_repositoryUoW);
                return teamService;
            }
        }

        public HistoryPasswordService HistoryPasswordService
        {
            get
            {
                if (historyPasswordService is null)
                    historyPasswordService = new HistoryPasswordService(_repositoryUoW);
                return historyPasswordService;
            }
        }

        public DepartmentService DepartmentService
        {
            get
            {
                if (departmentService is null)
                    departmentService = new DepartmentService(_repositoryUoW);
                return departmentService;
            }
        }

        public CourseService CourseService
        {
            get
            {
                if (courseService is null)
                    courseService = new CourseService(_repositoryUoW);
                return courseService;
            }
        }

        public EducationLevelService EducationLevelService
        {
            get
            {
                if (educationLevelService is null)
                    educationLevelService = new EducationLevelService(_repositoryUoW);
                return educationLevelService;
            }
        }

        public ProfileProfessionalService ProfileProfessionalService
        {
            get
            {
                if (profileProfessionalService is null)
                    profileProfessionalService = new ProfileProfessionalService(_repositoryUoW);
                return profileProfessionalService;
            }
        }

        public AddressService AddressService
        {
            get
            {
                if (addressService is null)
                    addressService = new AddressService(_repositoryUoW);
                return addressService;
            }
        }

        public ProfilePessoalService ProfilePessoalService
        {
            get
            {
                if (profilePessoalService is null)
                    profilePessoalService = new ProfilePessoalService(_repositoryUoW);
                return profilePessoalService;
            }
        }

        public AuthService AuthService
        {
            get
            {
                if (authService is null)
                    authService = new AuthService(UserService, _repositoryUoW.UserRepository, _tokenService, _crypto);
                return authService;
            }
        }

        public UserService UserService
        {
            get
            {
                if (userService is null)
                    userService = new UserService(_repositoryUoW);
                return userService;
            }
        }

        public CourseAvaliationService CourseAvaliationService
        {
            get
            {
                if (courseAvaliationService is null)
                    courseAvaliationService = new CourseAvaliationService(_repositoryUoW);
                return courseAvaliationService;
            }
        }

        public CourseActivitieService CourseActivitieService
        {
            get
            {
                if (courseActivitieService is null)
                    courseActivitieService = new CourseActivitieService(_repositoryUoW);
                return courseActivitieService;
            }
        }

        public NotificationService NotificationService
        {
            get
            {
                if (notificationService is null)
                    notificationService = new NotificationService(_repositoryUoW);
                return notificationService;
            }
        }

        public CourseEnrollmentService CourseEnrollmentService
        {
            get
            {
                if (courseEnrollmentService is null)
                    courseEnrollmentService = new CourseEnrollmentService(_repositoryUoW);
                return courseEnrollmentService;
            }
        }
    }
}