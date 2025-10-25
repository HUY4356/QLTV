using System;
using System.IO;
using Microsoft.AspNetCore.Mvc; // Thêm using
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QLThuvien.Areas.Identity.Pages.Account.Manage
{
    // Fix lỗi 'ManageNavPages' does not exist
    public static class ManageNavPages
    {
        public static string Index => "Index";
        public static string Email => "Email";
        public static string ChangePassword => "ChangePassword";
        public static string TwoFactorAuthentication => "TwoFactorAuthentication";
        public static string PersonalData => "PersonalData";
        public static string MyViolations => "MyViolations"; // Tên Action

        // Sửa lỗi null: Thêm '?'
        public static string? IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);
        public static string? EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);
        public static string? ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);
        public static string? TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);
        public static string? PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        // Hàm mới
        public static string? MyViolationsNavClass(ViewContext viewContext)
        {
            // Sửa lỗi null: Thêm '?'
            string? controllerName = viewContext.RouteData.Values["controller"]?.ToString();
            string? actionName = viewContext.RouteData.Values["action"]?.ToString();

            if (string.Equals(controllerName, "Profile", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(actionName, MyViolations, StringComparison.OrdinalIgnoreCase))
            {
                return "active";
            }
            return null;
        }

        private static string? PageNavClass(ViewContext viewContext, string page)
        {
            // Sửa lỗi null: Thêm '?'
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);

            if (activePage == null) return null; // Kiểm tra null

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}

