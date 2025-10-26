using System.ComponentModel;

namespace TrainMaster.Shared.Enums
{
    public enum PessoalProfileErrors
    {
        [Description("'Name' can not be null or empty!")]
        PessoalProfile_Error_NameCanNotBeNullOrEmpty,

        [Description("'Name' can not be less 4 letters!")]
        PessoalProfile_Error_NameLenghtLessFour,

        [Description("'Name' user already exists with that name!")]
        PessoalProfile_Error_FullNameExistLenghtLessFour,
    }
}