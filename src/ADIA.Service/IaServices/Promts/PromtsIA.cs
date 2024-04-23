using ADIA.Model.DataTransfer.IaResponses;
using System.Text.Json;

namespace ADIA.Service.IaServices.Promts;

public record PromtsIA
{
    public const string PromtSystem = "Eres experto identificando facturas o cualquier comprobante de pago, y texto general desde imagenes o texto. Devuelve sólo el objeto json, sin texto adicional ni comentarios. Todo en español.";

    public static string PromptImagen = $@"analiza la imagen para clasificar como 'FACTURA' o 'TEXTO GENERAL' y devuelve los datos en un objeto json segun sea el caso, si es un comprobante de pago como boleta, recibo o factura clasificarlo como 'FACTURA' y devuelve los datos en un formato json como el siguiente:

        {JsonSerializer.Serialize(AnalysisIAResultDto.CreateDefaultInvoice())}

        y si se clasficia como 'TEXTO GENERAL' obtén breve descripcion, resumen corto y sentimiento (si no puedes determinar el sentimiento asignarlo como neutral) y devuelvelos en un formato json como el siguiente:

        {JsonSerializer.Serialize(AnalysisIAResultDto.CreateDefaultGeneralText())}

            y si la imagen no se clasifica como 'FACTURA' y tampoco como 'TEXTO GENERAL' devolver los datos en un formato json como el siguiente:

        {JsonSerializer.Serialize(AnalysisIAResultDto.CreateDefaultUndefined())}";


    public static string PromptPdf = $@"analiza el texto que proviene de un archivo pdf para clasificar como 'FACTURA' o 'TEXTO GENERAL' y devuelve los datos en un objeto json segun sea el caso, si es un comprobante de pago como boleta, recibo o factura clasificarlo como 'FACTURA' y devuelve los datos en un formato json como el siguiente:

        {JsonSerializer.Serialize(AnalysisIAResultDto.CreateDefaultInvoice())}

        y si se clasficia como 'TEXTO GENERAL' obtén breve descripcion, resumen corto y sentimiento (si no puedes determinar el sentimiento asignarlo como neutral) y devuelvelos en un formato json como el siguiente:

        {JsonSerializer.Serialize(AnalysisIAResultDto.CreateDefaultGeneralText())}

        y si el texto no es ni factura ni de texto devolver datos en un formato json como el siguiente:

       {JsonSerializer.Serialize(AnalysisIAResultDto.CreateDefaultUndefined())}";

}