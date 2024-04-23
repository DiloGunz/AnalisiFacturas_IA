using Blazorise;

namespace ADIA.Web.Utils;

public static class FileExtensions
{
    public static bool EsImagen(this IFileEntry file)
    {
        string nombreArchivo = file.Name;
        if (!string.IsNullOrWhiteSpace(nombreArchivo))
        {
            if (nombreArchivo.EndsWith(".jpg") || nombreArchivo.EndsWith(".png"))
            {
                return true;
            }
        }
        return false;
    }

    public static bool EsPdf(this IFileEntry file)
    {
        string nombreArchivo = file.Name;
        if (!string.IsNullOrWhiteSpace(nombreArchivo))
        {
            if (nombreArchivo.EndsWith(".pdf"))
            {
                return true;
            }
        }
        return false;
    }

    public static async Task<byte[]> ConvertirByteArrayAsync(this IFileEntry file)
    {
        try
        {
            byte[] imagenArray;
            using (var stream = new MemoryStream())
            {
                await file.WriteToStreamAsync(stream);
                imagenArray = stream.ToArray();
            }
            return imagenArray;
        }
        catch 
        {
        }
        
        return null!;
    }
}