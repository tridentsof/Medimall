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

    public partial class Billing
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Billing()
        {
            this.BillDetails = new HashSet<BillDetail>();
        }

        public int BillId { get; set; }
        [Display(Name = "Mã tài khoản")]
        public Nullable<int> AccountId { get; set; }
        [Display(Name = "Ngày tạo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> PurchaseDate { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Hình thức vận chuyển")]
        public Nullable<int> DeliveryId { get; set; }
        [Display(Name = "Tổng tiền")]
        public Nullable<decimal> Total { get; set; }
        [Display(Name = "Mã thanh toán")]
        public Nullable<int> PayId { get; set; }
        [Display(Name = "Trạng thái")]
        public Nullable<int> Status { get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Tên người dùng")]
        public string UserName { get; set; }
    
        public virtual Account Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillDetail> BillDetails { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
