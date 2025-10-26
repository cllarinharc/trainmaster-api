using System.ComponentModel;

namespace TrainMaster.Shared.Enums
{
    public enum DepartmentErrors
    {
        [Description("'Name' can not be null or empty!")]
        Department_Error_NameCanNotBeNullOrEmpty,

        [Description("'Description' can not be null or empty!")]
        Department_Error_DescriptionCanNotBeNullOrEmpty,
    }
}