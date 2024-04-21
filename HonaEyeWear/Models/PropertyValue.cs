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
        public string Value { get; set; }

        [Display(Name = "Property")]
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
    }
}
