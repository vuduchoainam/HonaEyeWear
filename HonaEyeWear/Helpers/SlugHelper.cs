using System.Text.RegularExpressions;
using System.Text;

namespace HonaEyeWear.Helpers
{
    public class SlugHelper
    {
        public static string RemoveVietnameseDiacritics(string str)
        {
            // Các bước loại bỏ dấu tiếng Việt
            str = str.Replace("à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ", "a");
            str = str.Replace("è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ", "e");
            str = str.Replace("ì|í|ị|ỉ|ĩ|Ì|Í|Ị|Ỉ|Ĩ", "i");
            str = str.Replace("ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ", "o");
            str = str.Replace("ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ", "u");
            str = str.Replace("ỳ|ý|ỵ|ỷ|ỹ|Ỳ|Ý|Ỵ|Ỷ|Ỹ", "y");
            str = str.Replace(" ", "-");
            str = str.Replace("đ", "d").Replace("Đ", "D");
            str = Regex.Replace(str.Normalize(NormalizationForm.FormD), @"[\u0300-\u036f]", string.Empty);
            return str;
        }

        public static string GenerateSlug(string name)
        {
            // Chuyển đổi tất cả các ký tự thành chữ thường
            name = name.ToLower();
            // Xóa các dấu tiếng Việt và chuẩn hóa
            name = RemoveVietnameseDiacritics(name);
            // Loại bỏ khoảng trắng ở đầu và cuối chuỗi
            name = name.Trim('-');
            // Nếu tên sau các bước trên là rỗng, trả về một giá trị mặc định
            if (string.IsNullOrEmpty(name))
            {
                return "default-slug";
            }
            // Giới hạn độ dài của slug
            if (name.Length > 100)
            {
                name = name.Substring(0, 100);
            }
            return name;
        }
    }
}
