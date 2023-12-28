using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SewaKendaraan.Models
{
    public class PemesananViewModel
    {
        public Guid Id { get; set; }
        public Guid KendaraanId { get; set; }
        public Guid UserId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public Guid IdDriver { get; set; }
        public Guid IdPemesan { get; set; }
        public bool AdminShowDropdown { get; set; }
        public Guid userLogin { get;set; }
        public Guid LastApprovedBy { get; set; }
        public DateTime? LastApprovedDate { get; set; }

        public PemesananViewModel()
        {
            // Initialize KendaraanList to an empty list if needed
            KendaraanList = new List<SelectListItem>();
            UserList = new List<SelectListItem> ();
            DriverList = new List<SelectListItem>();
            UsersPemesan = new List<SelectListItem>();
        }
        public List<SelectListItem> KendaraanList { get; set; }
        public List<SelectListItem> DriverList { get; set; }
        public List<SelectListItem> UserList { get; set; }
        public List<SelectListItem> UsersPemesan { get; set; }
    }
}