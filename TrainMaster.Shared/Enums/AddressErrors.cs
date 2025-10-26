using System.ComponentModel;

namespace TrainMaster.Shared.Enums
{
    public enum AddressErrors
    {
        [Description("'PostalCode' can not be null or empty!")]
        Address_Error_PostalCodeCanNotBeNullOrEmpty,

        [Description("'Street' can not be null or empty!")]
        Address_Error_StreetCanNotBeNullOrEmpty,

        [Description("'Neighborhood' can not be null or empty!")]
        Address_Error_NeighborhoodCanNotBeNullOrEmpty,

        [Description("'City' can not be null or empty!")]
        Address_Error_CityCanNotBeNullOrEmpty,

        [Description("'Uf' can not be null or empty!")]
        Address_Error_UfCanNotBeNullOrEmpty,
    }
}