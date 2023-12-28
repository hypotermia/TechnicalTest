using SewaKendaraan.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace SewaKendaraan.Controllers
{
     public class HomeController : Controller
    {
        private SewaKendaraanEntities dbContext;

        public HomeController() {
            dbContext = new SewaKendaraanEntities();
        }

        public ActionResult Index(string roles,Guid id)
        {
            ViewBag.Roles = roles;
            ViewBag.id = id;
            if (roles != null)
            {
                ViewBag.IsAuthenticated = "true";
            }
            List<Pemesanan> pemesananData = dbContext.Pemesanans.ToList();
            Dictionary<Guid, string> userIdToNameMapping = dbContext.Users.ToDictionary(u => u.Id, u => u.username);

            // Transform data
            List<PemesananTableModel> combinedData = pemesananData
                        .Where(p => p.UserId == id && roles != "Admin")
                        .Select(p => new PemesananTableModel
            {
                Id = p.id,
                KendaraanName = p.Kendaraan != null ? p.Kendaraan.names : string.Empty,
                ApprovedBy = p.ApprovedBy.HasValue && userIdToNameMapping.ContainsKey(p.ApprovedBy.Value) ? userIdToNameMapping[p.ApprovedBy.Value] : "N/A",
                CreatedBy = userIdToNameMapping.ContainsKey(p.CreatedBy) ? userIdToNameMapping[p.CreatedBy] : string.Empty,
                DriverName = p.IdDriver.HasValue && userIdToNameMapping.ContainsKey(p.IdDriver.Value) ? userIdToNameMapping[p.IdDriver.Value] : string.Empty,
                CreatedDate = p.createdDate,
                ApprovedDate = p.ApprovedDate,
                LastApprovedBy = p.LastApprovedBy.HasValue && userIdToNameMapping.ContainsKey(p.LastApprovedBy.Value) ? userIdToNameMapping[p.LastApprovedBy.Value] : string.Empty,
                LastApprovedDate = p.LastApprovedDate,
                UserApprover = p.UserId.HasValue && userIdToNameMapping.ContainsKey(p.UserId.Value) ? userIdToNameMapping[p.UserId.Value] : string.Empty,
                UserPemesan = p.idPemesan.HasValue && userIdToNameMapping.ContainsKey(p.idPemesan.Value) ? userIdToNameMapping[p.idPemesan.Value] : string.Empty

            }).ToList();
            return View(combinedData);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Approve(string roles,Guid id,Guid usersId)
        {
            ViewBag.Roles = roles;
            ViewBag.id = usersId;
            var existingData = dbContext.Pemesanans.Find(id);
            if(existingData.ApprovedBy == null) {
                existingData.ApprovedBy = usersId;
            }

            else {
                existingData.LastApprovedBy = usersId;
            }
                
            bool IsAdmin = false;
            if (roles == "Admin")
            {
                IsAdmin = true;
            }
            else
            {
                IsAdmin = false;
            }

            if (existingData == null)
            {
                return HttpNotFound();
            }

            var kendaraanList = dbContext.Kendaraans
                .Select(k => new SelectListItem { Value = k.id.ToString(), Text = k.names })
                .ToList();

            var driverList = dbContext.Users
                .Where(u => u.Roles == "driver")
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.username })
                .ToList();

            var userList = dbContext.Users
                .Where(u => u.Roles == "Atasan")
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.username })
                .ToList();

            var userPemesan = dbContext.Users
                .Where(u => u.Roles == "pegawai")
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.username })
                .ToList();


            var viewModel = new PemesananViewModel
            {
                Id = existingData.id,
                KendaraanId = (Guid)existingData.KendaraanId, 
                KendaraanList = kendaraanList,
                AdminShowDropdown = IsAdmin,
                DriverList = driverList,
                UserList = userList,
                UsersPemesan = userPemesan,
                UserId = (Guid)existingData.UserId,
                IdPemesan = (Guid) existingData.idPemesan,
                IdDriver = (Guid) existingData.IdDriver,
                ApprovedBy = (Guid)existingData.ApprovedBy

            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Approve(PemesananViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var existingData = dbContext.Pemesanans.Find(viewModel.Id);
                var existingDataLog = dbContext.LogAplikasis.Find(viewModel.Id);
                if (existingData == null || existingDataLog == null)
                {
                    return HttpNotFound();
                }
                if (existingData.ApprovedBy == null)
                {
                    existingData.ApprovedBy = viewModel.ApprovedBy;
                    existingData.ApprovedDate = DateTime.Now;

                    existingDataLog.ApprovedBy = viewModel.ApprovedBy;
                    existingDataLog.ApprovedDate = DateTime.Now;
                }
                else
                {
                    existingData.LastApprovedBy = viewModel.LastApprovedBy;
                    existingData.LastApprovedDate = DateTime.Now;

                    existingDataLog.LastApprovedBy = viewModel.LastApprovedBy;
                    existingDataLog.LastApprovedDate = DateTime.Now;
                }
                dbContext.SaveChanges();
                User u = dbContext.Users.FirstOrDefault(x => x.Id == viewModel.UserId);
                return RedirectToAction("Index", "Home", new { roles = u.Roles, id = u.Id });
            }

            viewModel.KendaraanList = dbContext.Kendaraans
                .Select(k => new SelectListItem { Value = k.id.ToString(), Text = k.names })
                .ToList();

            viewModel.DriverList = dbContext.Users
                .Where(u => u.Roles == "driver")
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.username })
                .ToList();

            viewModel.UserList = dbContext.Users
                .Where(u => u.Roles == "Atasan")
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.username })
                .ToList();

            viewModel.UsersPemesan = dbContext.Users
                .Where(u => u.Roles == "pegawai")
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.username })
                .ToList();

            return View(viewModel);
        }
        public ActionResult Contact(string roles, Guid id)
        {
            ViewBag.Roles = roles;
            ViewBag.id = id;
            if (roles != null)
            {
                ViewBag.IsAuthenticated = "true";
            }
            List<Pemesanan> pemesananData = dbContext.Pemesanans.ToList();
            Dictionary<Guid, string> userIdToNameMapping = dbContext.Users.ToDictionary(u => u.Id, u => u.username);

            // Transform data
            List<PemesananTableModel> combinedData = pemesananData.Select(p => new PemesananTableModel
            {
                Id = p.id,
                KendaraanName = p.Kendaraan != null ? p.Kendaraan.names : string.Empty,
                ApprovedBy = p.ApprovedBy.HasValue && userIdToNameMapping.ContainsKey(p.ApprovedBy.Value) ? userIdToNameMapping[p.ApprovedBy.Value] : "N/A",
                CreatedBy = userIdToNameMapping.ContainsKey(p.CreatedBy) ? userIdToNameMapping[p.CreatedBy] : string.Empty,
                DriverName = p.IdDriver.HasValue && userIdToNameMapping.ContainsKey(p.IdDriver.Value) ? userIdToNameMapping[p.IdDriver.Value] : string.Empty,
                CreatedDate = p.createdDate,
                ApprovedDate = p.ApprovedDate,
                LastApprovedBy = p.LastApprovedBy.HasValue && userIdToNameMapping.ContainsKey(p.LastApprovedBy.Value) ? userIdToNameMapping[p.LastApprovedBy.Value] : string.Empty,
                LastApprovedDate = p.LastApprovedDate,
                UserApprover = p.UserId.HasValue && userIdToNameMapping.ContainsKey(p.UserId.Value) ? userIdToNameMapping[p.UserId.Value] : string.Empty,
                UserPemesan = p.idPemesan.HasValue && userIdToNameMapping.ContainsKey(p.idPemesan.Value) ? userIdToNameMapping[p.idPemesan.Value] : string.Empty

            }).ToList();
            return View(combinedData);
        }
        public ActionResult Create(string roles,Guid id)
        {
            ViewBag.Roles = roles;
            ViewBag.id = id;
            if(roles!= null)
            {
                ViewBag.IsAuthenticated = "true";
                Session["roles"] = roles;

            }
            bool AdminShowDropdown = false;
            if (roles == "Admin")
            {
                AdminShowDropdown = true;
            }
            else
            {
                AdminShowDropdown = false;
            }
            ViewBag.Message = "Your Create Page.";
            var kendaraanList = dbContext.Kendaraans
                .Select(k => new SelectListItem { Value = k.id.ToString(), Text = k.names })
                .ToList();
            var driverList = dbContext.Users
       .Where(u => u.Roles == "driver")
       .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.username })
       .ToList();

            var userList = dbContext.Users
                .Where(u=>u.Roles=="Atasan")
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.username })
                .ToList();
            var userPemesan = dbContext.Users
                .Where(u => u.Roles == "pegawai")
                .Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.username })
                .ToList();


            var viewModel = new PemesananViewModel
            {
                CreatedBy = id,
                KendaraanId = Guid.Empty,
                AdminShowDropdown = AdminShowDropdown,
                KendaraanList = kendaraanList,
                DriverList = driverList,
                UserList = userList,
                UsersPemesan = userPemesan
                
            };



            return View(viewModel) ;
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(PemesananViewModel model) 
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Pemesanan pemesanan = new Pemesanan
                    {
                        id = Guid.NewGuid(),
                        KendaraanId = model.KendaraanId,
                        UserId = model.UserId,
                        CreatedBy = model.CreatedBy,
                        createdDate = DateTime.Now,
                        IdDriver = model.IdDriver,
                        idPemesan = model.IdPemesan
                    };

                    dbContext.Pemesanans.Add(pemesanan);
                    dbContext.SaveChanges();

                    LogAplikasi log = new LogAplikasi
                    {
                        id = Guid.NewGuid(),
                        KendaraanId = model.KendaraanId,
                        UserId = model.UserId,
                        CreatedBy = model.CreatedBy,
                        createdDate = DateTime.Now,
                        IdDriver = model.IdDriver,
                        idPemesan = model.IdPemesan
                    };
                    dbContext.LogAplikasis.Add(log);
                    dbContext.SaveChanges();
                    User u = dbContext.Users.FirstOrDefault(x => x.Id == model.CreatedBy);

                    return RedirectToAction("Index", "Home", new { roles=u.Roles, id=model.CreatedBy });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the data.");
                    
                }
            }

            
            return View(model);
        }
        public ActionResult ExportToExcel(string roles, Guid id)
        {
            List<Pemesanan> pemesananData = dbContext.Pemesanans.ToList();
            Dictionary<Guid, string> userIdToNameMapping = dbContext.Users.ToDictionary(u => u.Id, u => u.username);

            // Transform data
            List<PemesananTableModel> combinedData = pemesananData
                .Where(p => p.UserId == id && roles != "Admin") 
                .Select(p => new PemesananTableModel
                {
                    Id = p.id,
                    KendaraanName = p.Kendaraan != null ? p.Kendaraan.names : string.Empty,
                    ApprovedBy = p.ApprovedBy.HasValue && userIdToNameMapping.ContainsKey(p.ApprovedBy.Value) ? userIdToNameMapping[p.ApprovedBy.Value] : "N/A",
                    CreatedBy = userIdToNameMapping.ContainsKey(p.CreatedBy) ? userIdToNameMapping[p.CreatedBy] : string.Empty,
                    DriverName = p.IdDriver.HasValue && userIdToNameMapping.ContainsKey(p.IdDriver.Value) ? userIdToNameMapping[p.IdDriver.Value] : string.Empty,
                    CreatedDate = p.createdDate,
                    ApprovedDate = p.ApprovedDate,
                    LastApprovedBy = p.LastApprovedBy.HasValue && userIdToNameMapping.ContainsKey(p.LastApprovedBy.Value) ? userIdToNameMapping[p.LastApprovedBy.Value] : string.Empty,
                    LastApprovedDate = p.LastApprovedDate,
                    UserApprover = p.UserId.HasValue && userIdToNameMapping.ContainsKey(p.UserId.Value) ? userIdToNameMapping[p.UserId.Value] : string.Empty,
                    UserPemesan = p.idPemesan.HasValue && userIdToNameMapping.ContainsKey(p.idPemesan.Value) ? userIdToNameMapping[p.idPemesan.Value] : string.Empty
                })
                .ToList();

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
             
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("PemesananData");

                // Add headers
                var properties = typeof(PemesananTableModel).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i].Name;
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                // Add data rows
                for (int i = 0; i < combinedData.Count; i++)
                {
                    var rowData = combinedData[i];
                    for (int j = 0; j < properties.Length; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = properties[j].GetValue(rowData, null);
                    }
                }

                // Auto fit columns for better readability
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Set content type and return the Excel file
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PemesananData.xlsx");
            }
        }
        public ActionResult Dashboard(string roles, Guid id)
        {
            ViewBag.Roles = roles;
            ViewBag.id = id;
            if (roles != null)
            {
                ViewBag.IsAuthenticated = "true";
            }

            

            var dataGrafik = dbContext.Pemesanans
                .Where(p => p.UserId == id && roles != "Admin")
                .GroupBy(p => new { Bulan = p.createdDate.Month, Tahun = p.createdDate.Year })
                .Select(g => new
                {
                    Bulan = g.Key.Bulan,
                    Tahun = g.Key.Tahun,
                    TotalPemakaian = g.Count()
                })
                .OrderBy(g => g.Tahun)
                .ThenBy(g => g.Bulan)
                .ToList();

            ViewBag.GrafikData = dataGrafik;

            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}