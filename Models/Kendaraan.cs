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
    
    public partial class Kendaraan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kendaraan()
        {
            this.LogAplikasis = new HashSet<LogAplikasi>();
            this.Pemesanans = new HashSet<Pemesanan>();
        }
    
        public System.Guid id { get; set; }
        public string names { get; set; }
        public string nopol { get; set; }
        public string konsumsiBBM { get; set; }
        public Nullable<System.DateTime> jadwalServis { get; set; }
        public Nullable<bool> onLoan { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogAplikasi> LogAplikasis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pemesanan> Pemesanans { get; set; }
    }
}