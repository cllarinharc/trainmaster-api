using System.ComponentModel;

namespace TrainMaster.Shared.Enums
{
    public enum EducationLevelErrors
    {
        [Description("'Title' can not be null or empty!")]
        EducationLevel_Error_TitleCanNotBeNullOrEmpty,

        [Description("'Institution' can not be null or empty!")]
        EducationLevel_Error_InstitutionCanNotBeNullOrEmpty,

        [Description("'StartedAt' can not be null or empty!")]
        EducationLevel_Error_StartedAtCanNotBeNullOrEmpty,

        [Description("'EndeedAt' can not be null or empty!")]
        EducationLevel_Error_EndeedAtCanNotBeNullOrEmpty,
    }
}