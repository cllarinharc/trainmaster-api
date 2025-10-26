using System.ComponentModel;

namespace TrainMaster.Shared.Enums
{
    public enum UserErrors
    {
        [Description("'Email' can not be null or empty!")]
        User_Error_EmailCanNotBeNullOrEmpty,

        [Description("'Email' invalid format!")]
        User_Error_InvalidEmailFormat,

        [Description("'Email' can not be less 4 letters!")]
        User_Error_EmailLenghtLessFour,

        [Description("'Password' must have at least 4 letters and 2 numbers!")]
        User_Error_PasswordInvalid,

        [Description("'Password' can not be null or empty!")]
        User_Error_PasswordCanNotBeNullOrEmpty,

        [Description("'Password' can not be less 4 letters!")]
        User_Error_PasswordLenghtLessFour
    }
}