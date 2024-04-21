﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HonaEyeWear.Models
{
    [Table("PropertyProduct")]
    public class PropertyProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }
        public string Value { get; set; }
    }
}
