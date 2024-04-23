using System.ComponentModel;

namespace ADIA.Shared.Enums
{
    public class EntityEnums
    {
        public enum Status
        {
            [Description("Disponible")]
            Enable,
            [Description("No Disponible")]
            Disable
        }

        public enum FileType
        {
            [Description("Ninguno")]
            None,
            [Description("PDF")]
            Pdf,
            [Description("Imagen")]
            Image
        }

        public enum DocumentType
        {
            [Description("No Definidio")]
            Undefined,
            [Description("Factura")]
            Invoice,
            [Description("Texto General")]
            GeneralText
        }

        public enum Ia
        {
            [Description("Open AI")]
            OpenIa,
            [Description("Azure AI")]
            AzureCognitiveService
        }
    }
}
