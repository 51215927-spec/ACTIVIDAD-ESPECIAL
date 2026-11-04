# Confirmación de lectura en consola — 04/11/2026

Se ejecutó la aplicación productiva con pywinpty/ConPTY, reutilizando Capture de la investigación previa. El script `Investigacion/verificar_confirmacion_lectura.py` recorre las opciones reales 1, 2, 11, 4 y 12. No sustituye métodos del programa ni simula la decisión mediante llamadas al dominio.

Cada sesión usa una copia independiente con una competencia guardada y cuatro equipos. En cinco sesiones se crea previamente Memoria, Sponsor anterior y el equipo Anterior mediante el menú. Después de leer se consulta la información de competencia para verificar el reemplazo o la conservación de esos datos.

| Entrada | Resultado esperado y observado |
| --- | --- |
| Sin competencia previa | Recupera cuatro equipos sin pedir confirmación |
| s | Recupera la competencia guardada y sustituye los datos anteriores |
| S | Recupera la competencia guardada y sustituye los datos anteriores |
| n | Cancela; conserva Memoria, Sponsor anterior y Anterior |
| x | Cancela; conserva Memoria, Sponsor anterior y Anterior |
| Enter | Cancela; conserva Memoria, Sponsor anterior y Anterior |

Seis casos correctos, sin cambios al código de producción ni a bibliotecas. No se compararon todas las celdas de la interfaz con el ejecutable original: esa verificación visual sigue pendiente en el plan.

Comandos:

```powershell
dotnet build "ACTIVIDAD ESPECIAL.sln" --artifacts-path IA/obj/Confirmacion
python IA/Investigacion/verificar_confirmacion_lectura.py
```

Compilación: cero errores, advertencias previas CA1416 y CS0219. Script: salida cero y seis resultados correctos. Requiere la sonda VerificacionGuardado previamente compilada y las dependencias locales de la investigación (pywinpty, pyte, Pillow). Las dependencias no se instalan ni modifican en esta tarea.

Evidencia local: `IA/Verificacion/Confirmacion-slfnaonx/`: seis sesiones ANSI, entradas, 17 capturas de pantalla PNG/texto y resultados.json. Los CSV de prueba y capturas completas permanecen ignorados por Git. El informe y el script sí se publican.

Se cierra la tarea de opción 11 y la etapa 5 para el alcance registrado. Permanecen pendientes las pruebas unitarias formales, recorrido integral y entrega de etapa 6, y el contraste visual integral de etapa 4.
