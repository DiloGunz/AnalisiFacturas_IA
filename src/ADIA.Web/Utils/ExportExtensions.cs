using ADIA.Model.DataTransfer.Dtos.AnalysisDtos;
using ADIA.Shared.Extensions;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;

namespace ADIA.Web.Utils;

public static class ExportExtensions
{
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

            //// Agregar título
            //var titleCell = ws.Cell(1, 1); // La celda A1
            //titleCell.Value = "Análisis de Documentos";
            //titleCell.Style.Font.Bold = true;
            //titleCell.Style.Font.FontSize = 20;
            //titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //// Combinar celdas para el título
            //ws.Range(1, 1, 2, dt.Columns.Count).Merge();

            //ws.Row(1).AdjustToContents();

            //// Insertar DataTable en la hoja de Excel
            //var range = ws.Cell(3, 1).InsertData(dt.AsEnumerable()); // Comienza desde la fila 2

            ws.Columns().AdjustToContents();

            var ms = new MemoryStream();
            wb.SaveAs(ms);
            ms.Position = 0;
            return ms;
        }
    }
}