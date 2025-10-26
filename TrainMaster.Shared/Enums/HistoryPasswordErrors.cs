using System.ComponentModel;

namespace TrainMaster.Shared.Enums
{
    public enum HistoryPasswordErrors
    {
        [Description("'OldPassword' can not be null or empty!")]
        HistoryPassword_Error_OldPasswordCanNotBeNullOrEmpty,
    }
}