using Blazorise;

namespace ADIA.Web.Utils;

/// <summary>
/// Proporciona métodos de extensión para trabajar con archivos, incluyendo la determinación del tipo de archivo y la conversión a byte array.
/// </summary>
public static class FileExtensions
{
    /// <summary>
    /// Determina si un archivo es una imagen basándose en la extensión del archivo.
    /// </summary>
    /// <param name="file">El archivo a verificar.</param>
    /// <returns>Verdadero si el archivo es una imagen (.jpg o .png); de lo contrario, falso.</returns>
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

    /// <summary>
    /// Determina si un archivo es un documento PDF basándose en la extensión del archivo.
    /// </summary>
    /// <param name="file">El archivo a verificar.</param>
    /// <returns>Verdadero si el archivo es un documento PDF; de lo contrario, falso.</returns>
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

    /// <summary>
    /// Convierte un archivo a un array de bytes de manera asíncrona.
    /// </summary>
    /// <param name="file">El archivo a convertir.</param>
    /// <returns>Una tarea que retorna un array de bytes representando el contenido del archivo.</returns>
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