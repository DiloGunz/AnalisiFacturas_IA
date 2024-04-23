using System.ComponentModel;

namespace ADIA.Shared.Enums
{
    public class GeneralEnums
    {
        public enum MedioPago
        {
            Efectivo,
            [Description("Tarjeta de Crédito")]
            TarjetaCredito,
            [Description("Tarjeta de Débito")]
            TarjetaDebito
        }

        public enum TipoTarjetaPago
        {
            Ninguno,
            Visa,
            MasterCard,
            AmericanExpress
        }
    }
}
