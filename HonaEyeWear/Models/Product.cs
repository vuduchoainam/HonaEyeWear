﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace HonaEyeWear.Models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(500, ErrorMessage = "Không vượt quá 500 ký tự")]
        public string Name { get; set; }

        public string Slug { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        [StringLength(2000, ErrorMessage = "Không vượt quá 2000 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        public decimal Price { get; set; }

        [NotMapped]
        public List<IFormFile>? ImageFile { get; set; }
        public virtual ICollection<ProductImage>? Images { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

         
        // Initialize the collection to avoid null reference issues
        public virtual ICollection<PropertyProduct> PropertyProducts { get; set; }        
    }
}
