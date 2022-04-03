﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Medimall.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class News
    {
        [Display(Name = "Mã tin tức")]
        public int NewsId { get; set; }
        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Không được để trống")]
        public string NewTiTle { get; set; }
        [Display(Name = "Nội dung")]
        public string Content { get; set; }
        public Nullable<int> NewsCategoryId { get; set; }

        public virtual NewsCategory NewsCategory { get; set; }
    }
}