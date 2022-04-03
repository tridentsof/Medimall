using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Medimall.Helper
{
    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int maxContent = 1024 * 1024;
            string[] sAllowedExt = new string[] { ".jpg", ".png" };
            var file = value as HttpPostedFileBase;
            if (value == null)
            {
                return true;
            }
            else if (!sAllowedExt.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Tải lên ảnh có định dạng" + string.Join(", ", sAllowedExt);

                return false;
            }
            else if (file.ContentLength > maxContent)
            {
                ErrorMessage = "Ảnh của bạn có kích thước lớn quá, kích thước cho phép: " + (maxContent / 1024).ToString() + "MB";

                return false;
            }
            else
                return true;
        }
    }
}