using System;
using System.Collections.Generic;

namespace BancoSimulacion.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public double TiempoLlegada { get; set; }
        public double TiempoInicioServicio { get; set; }
        public double TiempoFinServicio { get; set; }
        public double TiempoEspera => TiempoInicioServicio - TiempoLlegada;
    }

    public class PuntoHistorico
    {
        public double Minuto { get; set; }
        public int LongitudCola { get; set; }
    }

    public class ReporteEscenario
    {
        public int Cajeros { get; set; }
        public double TiempoEsperaPromedio { get; set; }
        public double PorcentajeOcupacion { get; set; }
        public int ClientesAtendidos { get; set; }
        public int ClientesRechazados { get; set; }
        public List<PuntoHistorico> Historico { get; set; } = new();
    }
}