using System.ComponentModel;

namespace TrainMaster.Shared.Enums
{
    public enum CourseErrors
    {
        [Description("'Name' can not be null or empty!")]
        Course_Error_NameCanNotBeNullOrEmpty,

        [Description("'Description' can not be null or empty!")]
        Course_Error_DescriptionCanNotBeNullOrEmpty,

        [Description("'Start Date' can not be null or empty!")]
        Course_Error_StartDateCanNotBeNullOrEmpty,

        [Description("'End Date' can not be null or empty!")]
        Course_Error_EndDateCanNotBeNullOrEmpty,
    }
}