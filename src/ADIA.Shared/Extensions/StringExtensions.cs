using ADIA.Shared.Enums;

namespace ADIA.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string TrimNotNull(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            return value.Trim();
        }

        public static string? ToUpperTrimOrNull(this string? value)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToUpper().Trim();
        }

        public static string ToUpperTrim(this string? value)
        {
            if (value == null)
            {
                return "";
            }

            return value.ToUpper().Trim();
        }

        public static string ToUpperTrim(this string? value, string replace)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (string.IsNullOrWhiteSpace(replace))
                {
                    return "";
                }
                return replace;
            }
            return value.ToUpper().Trim();
        }

        public static string? ToLowerTrimOrNull(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value.ToLower().Trim();
        }

        public static string ToLowerTrim(this string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            return value.ToLower().Trim();
        }

        public static string ToLowerTrim(this string? value, string replace)
        {
            if (value == null)
            {
                if (string.IsNullOrWhiteSpace(replace))
                {
                    return "";
                }
                return replace;
            }

            return value.ToLower().Trim();
        }

        public static EntityEnums.FileType GetFileType(this string? value)
        {
            if(!string.IsNullOrWhiteSpace(value))
            {
                switch (value.ToLower())
                {
                    case ".jpg":
                    case ".png": return EntityEnums.FileType.Image;
                    case ".pdf": return EntityEnums.FileType.Pdf;
                }
            }

            return EntityEnums.FileType.None;
        }
    }
}
