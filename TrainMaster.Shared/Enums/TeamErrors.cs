using System.ComponentModel;

namespace TrainMaster.Shared.Enums
{
    public enum TeamErrors
    {
        [Description("'Name' can not be null or empty!")]
        Team_Error_NameCanNotBeNullOrEmpty,

        [Description("'Description' can not be null or empty!")]
        Team_Error_DescriptionCanNotBeNullOrEmpty,
    }
}