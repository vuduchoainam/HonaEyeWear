using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HonaEyeWear.Models
{
    [Table("Role")]
    public class Role : IdentityRole
    {
    }
}
