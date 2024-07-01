using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HonaEyeWear.Models
{
    [Table("PropertyValue")]
    public class PropertyValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Giá trị không được để trống")]
        public string Name { get; set; }
        public ICollection<PropertyProduct>? PropertyProducts { get; set; }
    }
}
