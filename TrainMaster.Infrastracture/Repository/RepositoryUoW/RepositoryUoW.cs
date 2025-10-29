using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;
using TrainMaster.Infrastracture.Repository.Request;

namespace TrainMaster.Infrastracture.Repository.RepositoryUoW
{
    public class RepositoryUoW : IRepositoryUoW
    {
        private readonly DataContext _context;
        private bool _disposed = false;
        private IUserRepository? _userEntityRepository = null;
        private IPessoalProfileRepository? _pessoalProfileEntityRepository = null;
        private IAddressRepository? _addressRepository = null;
        private IEducationLevelRepository? _educationLevelRepository = null;
        private IProfessionalProfileRepository? _professionalProfileRepository = null;
        private ICourseRepository _courseRepository = null;
        private IDepartmentRepository _departmentRepository = null;
        private ITeamRepository _teamRepository = null;
        private IHistoryPasswordRepository _historyPasswordRepository = null;
        private ICourseAvaliationRepository _courseAvaliationRepository = null;
        private ICourseActivitieRepository _courseActivitieRepository = null;
        private INotificationRepository _notificationRepository = null;
        private ICourseFeedbackRepository _courseFeedbackRepository = null;
        private IQuestionRepository _questionRepository = null;
        private IQuestionOptionRepository _questionOptionRepository = null;
        private IExamRepository _examRepository = null;
        private IExamQuestionRepository _examQuestionRepository = null;
        private IBadgeRepository _badgeRepository = null;
        private IFaqRepository _faqRepository = null;
        private ICalendarRepository _calendarRepository = null;

        public RepositoryUoW(DataContext context)
        {
            _context = context;
        }

        public ICalendarRepository CalendarRepository
        {
            get
            {
                if (_calendarRepository is null)
                {
                    _calendarRepository = new CalendarRepository(_context);
                }
                return _calendarRepository;
            }
        }

        public IFaqRepository FaqRepository
        {
            get
            {
                if (_faqRepository is null)
                {
                    _faqRepository = new FaqRepository(_context);
                }
                return _faqRepository;
            }
        }

        public IBadgeRepository BadgeRepository
        {
            get
            {
                if (_badgeRepository is null)
                {
                    _badgeRepository = new BadgeRepository(_context);
                }
                return _badgeRepository;
            }
        }

        public IExamQuestionRepository ExamQuestionRepository
        {
            get
            {
                if (_examQuestionRepository is null)
                {
                    _examQuestionRepository = new ExamQuestionRepository(_context);
                }
                return _examQuestionRepository;
            }
        }

        public IExamRepository ExamRepository
        {
            get
            {
                if (_examRepository is null)
                {
                    _examRepository = new ExamRepository(_context);
                }
                return _examRepository;
            }
        }

        public IQuestionOptionRepository QuestionOptionRepository
        {
            get
            {
                if (_questionOptionRepository is null)
                {
                    _questionOptionRepository = new QuestionOptionRepository(_context);
                }
                return _questionOptionRepository;
            }
        }

        public IQuestionRepository QuestionRepository
        {
            get
            {
                if (_questionRepository is null)
                {
                    _questionRepository = new QuestionRepository(_context);
                }
                return _questionRepository;
            }
        }

        public ICourseFeedbackRepository CourseFeedbackRepository
        {
            get
            {
                if (_courseFeedbackRepository is null)
                {
                    _courseFeedbackRepository = new CourseFeedbackRepository(_context);
                }
                return _courseFeedbackRepository;
            }
        }

        public INotificationRepository NotificationRepository
        {
            get
            {
                if (_notificationRepository is null)
                {
                    _notificationRepository = new NotificationRepository(_context);
                }
                return _notificationRepository;
            }
        }

        public IHistoryPasswordRepository HistoryPasswordRepository
        {
            get
            {
                if (_historyPasswordRepository is null)
                {
                    _historyPasswordRepository = new HistoryPasswordRepository(_context);
                }
                return _historyPasswordRepository;
            }
        }

        public ITeamRepository TeamRepository
        {
            get
            {
                if (_teamRepository is null)
                {
                    _teamRepository = new TeamRepository(_context);
                }
                return _teamRepository;
            }
        }

        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                if (_departmentRepository is null)
                {
                    _departmentRepository = new DepartmentRepository(_context);
                }
                return _departmentRepository;
            }
        }

        public ICourseRepository CourseRepository
        {
            get
            {
                if (_courseRepository is null)
                {
                    _courseRepository = new CourseRepository(_context);
                }
                return _courseRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userEntityRepository is null)
                {
                    _userEntityRepository = new UserRepository(_context);
                }
                return _userEntityRepository;
            }
        }

        public IPessoalProfileRepository PessoalProfileRepository
        {
            get
            {
                if (_pessoalProfileEntityRepository is null)
                {
                    _pessoalProfileEntityRepository = new PessoalProfileRepository(_context);
                }
                return _pessoalProfileEntityRepository;
            }
        }

        public IAddressRepository AddressRepository
        {
            get
            {
                if (_addressRepository is null)
                {
                    _addressRepository = new AddressRepository(_context);
                }
                return _addressRepository;
            }
        }

        public IProfessionalProfileRepository ProfessionalProfileRepository
        {
            get
            {
                if (_professionalProfileRepository is null)
                {
                    _professionalProfileRepository = new ProfessionalProfileRepository(_context);
                }
                return _professionalProfileRepository;
            }
        }

        public IEducationLevelRepository EducationLevelRepository
        {
            get
            {
                if (_educationLevelRepository is null)
                {
                    _educationLevelRepository = new EducationLevelRepository(_context);
                }
                return _educationLevelRepository;
            }
        }

        public ICourseAvaliationRepository CourseAvaliationRepository
        {
            get
            {
                if (_courseAvaliationRepository is null)
                {
                    _courseAvaliationRepository = new CourseAvaliationRepository(_context);
                }
                return _courseAvaliationRepository;
            }
        }

        public ICourseActivitieRepository CourseActivitieRepository
        {
            get
            {
                if (_courseActivitieRepository is null)
                {
                    _courseActivitieRepository = new CourseActivitieRepository(_context);
                }
                return _courseActivitieRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error($"Database connection failed: {ex.Message}");
                throw new ApplicationException("Database is not available. Please check the connection.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}