using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HonaEyeWear.Models
{
    [Table("PropertyProduct")]
    public class PropertyProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int PropertyValueId { get; set; }
        public virtual PropertyValue PropertyValue { get; set; }

        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }
    }
}
