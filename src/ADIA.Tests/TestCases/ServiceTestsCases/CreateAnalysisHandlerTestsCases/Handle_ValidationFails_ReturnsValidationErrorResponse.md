## Casos de Prueba para el M�todo `Handle_ValidationFails_ReturnsValidationErrorResponse`

### 1. Caso de prueba: Entrada de archivo inv�lido
- **Precondiciones**: Ninguna.
- **Entradas**: `CreateAnalysisCommand` con un archivo inv�lido.
- **Pasos de Ejecuci�n**: Ejecutar el m�todo `Handle`.
- **Resultados Esperados**: `IsSuccess` debe ser `false` y el mensaje debe contener "Invalid file".

### 2. Caso de prueba: Campo obligatorio faltante
- **Precondiciones**: Ninguna.
- **Entradas**: `CreateAnalysisCommand` sin un campo obligatorio (e.g., `name`).
- **Pasos de Ejecuci�n**: Ejecutar el m�todo `Handle`.
- **Resultados Esperados**: `IsSuccess` debe ser `false` y el mensaje debe indicar que falta un campo obligatorio.

### 3. Caso de prueba: Formato de fecha incorrecto
- **Precondiciones**: Ninguna.
- **Entradas**: `CreateAnalysisCommand` con un campo de fecha en formato incorrecto.
- **Pasos de Ejecuci�n**: Ejecutar el m�todo `Handle`.
- **Resultados Esperados**: `IsSuccess` debe ser `false` y el mensaje debe indicar "Formato de fecha incorrecto".

### 4. Caso de prueba: Datos num�ricos fuera de rango
- **Precondiciones**: Ninguna.
- **Entradas**: `CreateAnalysisCommand` con un campo num�rico que excede el rango permitido.
- **Pasos de Ejecuci�n**: Ejecutar el m�todo `Handle`.
- **Resultados Esperados**: `IsSuccess` debe ser `false` y el mensaje debe indicar "Dato num�rico fuera de rango".

### 5. Caso de prueba: Longitud de cadena excesiva
- **Precondiciones**: Ninguna.
- **Entradas**: `CreateAnalysisCommand` con un campo de texto demasiado largo.
- **Pasos de Ejecuci�n**: Ejecutar el m�todo `Handle`.
- **Resultados Esperados**: `IsSuccess` debe ser `false` y el mensaje debe indicar "Longitud de cadena excesiva".

### 6. Caso de prueba: Caracteres no permitidos en un campo
- **Precondiciones**: Ninguna.
- **Entradas**: `CreateAnalysisCommand` con caracteres especiales no permitidos.
- **Pasos de Ejecuci�n**: Ejecutar el m�todo `Handle`.
- **Resultados Esperados**: `IsSuccess` debe ser `false` y el mensaje debe indicar "Caracteres no permitidos".

### 7. Caso de prueba: Entrada de tipo de an�lisis desconocido
- **Precondiciones**: Ninguna.
- **Entradas**: `CreateAnalysisCommand` con un tipo de an�lisis no reconocido por el sistema.
- **Pasos de Ejecuci�n**: Ejecutar el m�todo `Handle`.
- **Resultados Esperados**: `IsSuccess` debe ser `false` y el mensaje debe indicar "Tipo de an�lisis desconocido".

### 8. Caso de prueba: Referencia a entidad no existente
- **Precondiciones**: Ninguna.
- **Entradas**: `CreateAnalysisCommand` con una referencia a una entidad que no existe (e.g., `projectId` inexistente).
- **Pasos de Ejecuci�n**: Ejecutar el m�todo `Handle`.
- **Resultados Esperados**: `IsSuccess` debe ser `false` y el mensaje debe indicar "Referencia a entidad no existente".

### 9. Caso de prueba: Entrada duplicada no permitida
- **Precondiciones**: Ninguna.
- **Entradas**: `CreateAnalysisCommand` que intenta duplicar una entrada ya existente.
- **Pasos de Ejecuci�n**: Ejecutar el m�todo `Handle`.
- **Resultados Esperados**: `IsSuccess` debe ser `false` y el mensaje debe indicar "Entrada duplicada no permitida".

### 10. Caso de prueba: Interrupci�n del proceso por timeout
- **Precondiciones**: Ninguna.
- **Entradas**: `CreateAnalysisCommand` durante una alta carga de sistema que podr�a causar un timeout.
- **Pasos de Ejecuci�n**: Ejecutar el m�todo `Handle`.
- **Resultados Esperados**: `IsSuccess` debe ser `false` y el mensaje debe indicar "Error de timeout en el proceso".
