using System.ComponentModel.DataAnnotations;

namespace PDA.Web.Common
{
    public enum GenderEnum
    {
        Male = 1,
        Female = 2,
        Others = 3
    }

    public enum PilotTypeEnum
    {
        [Display(Name = "Type 1")]
        Type1 = 1,
        [Display(Name = "Type 2")]
        Type2 = 2
    }
}
