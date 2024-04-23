namespace ADIA.OpenAi.Proxy.Utils;

public record Prompts
{
    public const string PromptImagen = 
$@"analiza la imagen para clasificar como FACTURA o INFORMACION y devuelve los datos en un objeto json segun sea el caso, si es FACTURA devuelve los datos en un formato json como el siguiente:

        {{
            ""tipoArchivo"": 1, // es numero
            ""data"": 
                {{
                    ""nombreCliente"": """",
                    ""direccionCliente"": """",
                    ""nombreProveedor"": """",
                    ""direccionProveedor"": """",
                    ""nroFactura"": """",
                    ""fecha"": """",
                    ""moneda"": """", // formato de fecha para deserializar en c#
                    ""total"": 0, // solo numero, no incluir simbolo de moneda
                    ""items"": [
                        {{
                            ""descripcion"": """",
                            ""cantidad"": 0,
                            ""precioUnitario"": 0,
                            ""total"": 0 // solo numero, no incluir simbolo de moneda
                        }}
                    ]
                }}
        }}


        y si es INFORMACION devuelve los datos segun el siguiente formato json:

        {{
            ""tipoArchivo"": 2,
            ""data"": 
                {{
                    ""descripcion"": """",
                    ""resumen"": """",
                    ""sentimiento"": """"
                }}
        }}

        y si la imagen no es ni factura ni de texto devolver los datos segun el siguiente formato json:

       {{
            ""tipoArchivo"": 3,
            ""data"": 
                {{
                    ""mensage"": ""NO ES POSIBLE CLASIFICAR LA IMAGEN COMO FACTURA O INFORMACIÓN"",
                }}
        }}


        
    devuelve solo el objeto json, sin texto adicional ni comentarios
";


    public const string PromptPdf = $@"analiza el texto que proviene de un archivo pdf para clasificar como FACTURA o INFORMACION y devuelve los datos en un objeto json segun sea el caso, si es FACTURA devuelve los datos en un formato json como el siguiente:

        {{
            ""tipoArchivo"": 1, // es numero
            ""data"": 
                {{
                    ""nombreCliente"": """",
                    ""direccionCliente"": """",
                    ""nombreProveedor"": """",
                    ""direccionProveedor"": """",
                    ""nroFactura"": """",
                    ""fecha"": """",
                    ""moneda"": """", // formato de fecha para deserializar en c#
                    ""total"": 0, // solo numero, no incluir simbolo de moneda
                    ""items"": [
                        {{
                            ""descripcion"": """",
                            ""cantidad"": 0,
                            ""precioUnitario"": 0,
                            ""total"": 0 // solo numero, no incluir simbolo de moneda
                        }}
                    ]
                }}
        }}


        y si es INFORMACION devuelve los datos segun el siguiente formato json:

        {{
            ""tipoArchivo"": 2,
            ""data"": 
                {{
                    ""descripcion"": """",
                    ""resumen"": """",
                    ""sentimiento"": """"
                }}
        }}

        y si el texto no es ni factura ni de texto devolver los datos segun el siguiente formato json:

       {{
            ""tipoArchivo"": 3,
            ""data"": 
                {{
                    ""mensage"": ""NO ES POSIBLE CLASIFICAR LA IMAGEN COMO FACTURA O INFORMACIÓN"",
                }}
        }}


        
    devuelve solo el objeto json, sin tecto adicional ni comentarios
";

}