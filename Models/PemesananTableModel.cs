using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SewaKendaraan.Models
{
    public class PemesananTableModel
    {
        public Guid Id { get; set; }
        public string KendaraanName { get; set; }
        public string UserApprover { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string LastApprovedBy { get; set; }
        public DateTime? LastApprovedDate { get; set; }
        public string DriverName { get; set; }
        public string UserPemesan { get; set; }
     
    }
}