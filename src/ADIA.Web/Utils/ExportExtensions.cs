using ADIA.Model.DataTransfer.Dtos.AnalysisDtos;
using ADIA.Shared.Extensions;
using ClosedXML.Excel;
using System.Data;

namespace ADIA.Web.Utils;

/// <summary>
/// Proporciona métodos de extensión para exportar datos de análisis a formatos específicos como Excel.
/// </summary>
public static class ExportExtensions
{
    /// <summary>
    /// Convierte una lista de DTOs de análisis en un archivo de Excel, creando un documento que resume los resultados de análisis.
    /// </summary>
    /// <param name="list">Colección enumerable de DTOs de análisis que se convertirán en Excel.</param>
    /// <returns>Un MemoryStream que contiene el archivo Excel generado.</returns>
    public static MemoryStream AnalysisToExcel(this IEnumerable<AnalysisListDto> list)
    {
        DataTable dt = new DataTable("Analysis");
        dt.Columns.AddRange(new DataColumn[]
        {
            new DataColumn("Nombre Archivo"),
            new DataColumn("Tipo Archivo"),
            new DataColumn("Fecha Análisis"),
            new DataColumn("Estado Análisis"),
            new DataColumn("Tiempo Respuesta (ms)"),
            new DataColumn("Tipo Doc Respuesta")
        });

        foreach (var item in list)
        {
            if (item.AnalysisResponseListDto != null)
            {
                dt.Rows.Add(
                    item.FileName, 
                    item.FileType.GetDescription(), 
                    item.AnalysisDate, 
                    item.AnalysisResponseListDto.IsSuccess ? "Correcto" : "Incorrecto", 
                    item.AnalysisResponseListDto.ResponseTime, 
                    item.AnalysisResponseListDto.DocumentType.GetDescription());
            }
            else
            {
                dt.Rows.Add(
                    item.FileName,
                    item.FileType.GetDescription(),
                    item.AnalysisDate,
                    "No Completado",
                    0,
                    "-");
            }
        }

        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add(dt, "Hoja1", "Análisis de Documentos");

            ws.Columns().AdjustToContents();

            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return ms;
        }
    }
}