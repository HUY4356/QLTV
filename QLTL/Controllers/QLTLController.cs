using DocumentManager.Models;
using QLTL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTL.Controllers
{
    public class QLTLController : Controller
    {
        private static readonly List<Document> documents = new List<Document>();

        // Trang danh sách tài liệu
        public ActionResult Index()
        {
            return View(documents);
        }

        // Form tải lên
        public ActionResult Create()
        {
            return View();
        }

        // Xử lý upload
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file, string title, string description)
        {
            if (file != null && file.ContentLength > 0)
            {
                string uploadsPath = Server.MapPath("~/Uploads");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(uploadsPath, fileName);
                file.SaveAs(path);

                documents.Add(new Document
                {
                    Id = documents.Count + 1,
                    Title = title,
                    Description = description,
                    FilePath = "/Uploads/" + fileName,
                    UploadDate = DateTime.Now
                });

                return RedirectToAction("Index");
            }

            ViewBag.Message = "Vui lòng chọn file hợp lệ!";
            return View();
        }

        // Trang thống kê
        public ActionResult Statistics()
        {
            var totalDocs = documents.Count;
            var latestDoc = documents.OrderByDescending(d => d.UploadDate).FirstOrDefault();

            ViewBag.TotalDocs = totalDocs;
            ViewBag.LatestDoc = latestDoc?.Title ?? "Chưa có tài liệu";

            return View();
        }
    }
}
