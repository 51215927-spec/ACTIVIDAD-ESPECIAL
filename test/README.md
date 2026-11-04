# Pruebas de StreamORT

Esta carpeta contiene las comprobaciones de persistencia, los scripts Python de pruebas de consola y las sondas auxiliares. Permiten verificar datos y contrastar el comportamiento con el ejecutable original.

- [Guía de ejecución y resultados](../docs/Pruebas.md).
- [Recorrido final de las 12 opciones y capturas](../docs/RecorridoFinal.md).
- [Resultados conservados](resultados/): salidas de texto, entradas JSON y capturas; las nuevas ejecuciones locales quedan en `resultados/local/`.
- [Pruebas de persistencia en C#](VerificacionGuardado/).
- [Proyecto de pruebas unitarias con xUnit](StreamORT.Tests/).
- [Sondas de consola](sondas/).
- [Análisis funcional](../docs/Analisis.md) y [casos observados](../docs/Pruebas-StreamORT.md).
- [Informes de investigación](../IA/Log.md#archivo-comparacion-interfaz-20261103), integrados en el registro de IA.

Los scripts Python están en esta carpeta. Son herramientas auxiliares. El proyecto xUnit está incorporado a la solución con 119 casos correctos: 86 de dominio y 33 de persistencia en directorios temporales.
