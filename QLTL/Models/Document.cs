using System;
using System.ComponentModel.DataAnnotations;

namespace QLTL.Models
{
    public class Document
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tài liệu")]
        [Display(Name = "Tên tài liệu")]
        public string Title { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Đường dẫn file")]
        public string FilePath { get; set; }

        [Display(Name = "Ngày tải lên")]
        public DateTime UploadDate { get; set; }
    }
}
