using System.ComponentModel;

namespace TrainMaster.Shared.Enums
{
    public enum ProfessionalProfileErrors
    {
        [Description("'JobTitle' can not be null or empty!")]
        ProfessionalProfile_Error_JobTitleCanNotBeNullOrEmpty,

        [Description("'YearsOfExperience' can not be null or empty!")]
        ProfessionalProfile_Error_YearsOfExperienceCanNotBeNullOrEmpty,

        [Description("'Skills' can not be null or empty!")]
        ProfessionalProfile_Error_SkillsCanNotBeNullOrEmpty,
    }
}