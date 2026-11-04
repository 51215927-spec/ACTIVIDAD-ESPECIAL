# Pruebas de StreamORT

Esta carpeta contiene las comprobaciones de persistencia, los scripts Python de pruebas de consola y las sondas auxiliares. Permiten verificar datos y contrastar el comportamiento con el ejecutable original.

- [Guía de ejecución y resultados](../docs/Pruebas.md).
- [Resultados conservados](resultados/): salidas de texto, entradas JSON y capturas; las nuevas ejecuciones locales quedan en `resultados/local/`.
- [Pruebas de persistencia en C#](VerificacionGuardado/).
- [Sondas de consola](sondas/).
- [Análisis funcional](../IA/Analisis.md) y [casos observados](../IA/Pruebas-StreamORT.md).
- [Informes de investigación](../IA/Log.md#archivo-comparacion-interfaz-20261103), integrados en el registro de IA.

Los scripts Python están en esta carpeta. Son herramientas auxiliares; las pruebas unitarias de la etapa 6 siguen pendientes.
