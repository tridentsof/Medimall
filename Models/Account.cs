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
    using Medimall.Helper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            this.HealthBooks = new HashSet<HealthBook>();
            this.ListDrugs = new HashSet<ListDrug>();
            this.Billings = new HashSet<Billing>();
            this.Vouchers = new HashSet<Voucher>();
        }

        public int AccountId { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Mật khẩu")]
        [MinLength(6, ErrorMessage = "{0} phải chứa ít nhất 6 kí tự")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Không được để trống")]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Không được để trống")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BirthDay { get; set; }
        [Display(Name = "Trạng thái")]
        public Nullable<int> Status { get; set; }
        [Display(Name = "Mã xác nhận")]
        public string ActiveCode { get; set; }
        [Display(Name = "Điểm thưởng")]
        public Nullable<int> PowerPoint { get; set; }

        [Display(Name = "Avatar")]
        [NotMapped]
        [ValidateFile]
        public HttpPostedFileBase Photo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HealthBook> HealthBooks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ListDrug> ListDrugs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Billing> Billings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
