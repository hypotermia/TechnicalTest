//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SewaKendaraan.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pemesanan
    {
        public System.Guid id { get; set; }
        public Nullable<System.Guid> KendaraanId { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime createdDate { get; set; }
        public Nullable<System.Guid> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public Nullable<System.Guid> LastApprovedBy { get; set; }
        public Nullable<System.DateTime> LastApprovedDate { get; set; }
        public Nullable<System.Guid> idPemesan { get; set; }
        public Nullable<System.Guid> IdDriver { get; set; }
    
        public virtual Kendaraan Kendaraan { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
        public virtual User User3 { get; set; }
        public virtual User User4 { get; set; }
        public virtual User User5 { get; set; }
    }
}