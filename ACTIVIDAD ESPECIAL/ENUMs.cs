using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTIVIDAD_ESPECIAL
{
    internal enum Categoria
    {
        Gaming,
        Humor,
        Musica,
        Entretenimiento
    }

    internal enum Especialidad
    {
        Streamer,
        Youtuber,
        Tiktoker,
        Influencer
    }

    internal enum ResultadoComparacion
    {
        PrimerEquipoInexistente,
        SegundoEquipoInexistente,
        MismoEquipo,
        EquipoSinCreadores,
        Empate,
        GanaPrimero,
        GanaSegundo
    }
}
