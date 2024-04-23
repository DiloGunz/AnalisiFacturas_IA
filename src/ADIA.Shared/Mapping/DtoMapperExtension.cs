using System.Text.Json;
using System.Text.Json.Serialization;

namespace ADIA.Shared.Mapping
{
    public static class DtoMapperExtension
    {
        public static T MapTo<T>(this object value) where T : new()
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                PropertyNameCaseInsensitive = true,
            };
            //#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            //return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, options), options);
            //#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo

            var result = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, options), options);

            if (result == null)
            {
                return new T();
            }
            else
            {
                return result;
            }
        }

        //public static T? MapTo<T>(this object value)
        //{
        //    var options = new JsonSerializerOptions
        //    {
        //        ReferenceHandler = ReferenceHandler.Preserve
        //    };
        //    //#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
        //    //return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, options), options);
        //    //#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo

        //    var result = JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, options), options);

        //    if (result == null)
        //    {
        //        return default(T);
        //    }
        //    else
        //    {
        //        return result;
        //    }
        //}
    }
}