
using BancoSimulacion.Models;

namespace BancoSimulacion.Services
{
    public class BankSimulationService
    {
        private readonly Random _random = new Random();

        private double GenerarExponencial(double media) => -media * Math.Log(1.0 - _random.NextDouble());

        public ReporteEscenario EjecutarSimulacion(int numCajeros, double tiempoLlegadas, double tiempoServicio, int duracionTotal)
        {
            double tiempoActual = 0;
            var cola = new Queue<Cliente>();
            var servidores = new double[numCajeros];
            var atendidos = new List<Cliente>();
            int rechazados = 0;
            double tiempoTotalOcupado = 0;

            var reporte = new ReporteEscenario { Cajeros = numCajeros };
            double proximaLlegada = GenerarExponencial(tiempoLlegadas);

            while (tiempoActual < duracionTotal)
            {
                reporte.Historico.Add(new PuntoHistorico { Minuto = tiempoActual, LongitudCola = cola.Count });
                double proximoFinServicio = servidores.Min();

                if (proximaLlegada <= proximoFinServicio)
                {
                    tiempoActual = proximaLlegada;
                    if (tiempoActual > duracionTotal) break;

                    if (cola.Count < 10)
                        cola.Enqueue(new Cliente { Id = atendidos.Count + 1, TiempoLlegada = tiempoActual });
                    else
                        rechazados++;

                    proximaLlegada += GenerarExponencial(tiempoLlegadas);
                }
                else
                {
                    tiempoActual = Math.Max(tiempoActual, proximoFinServicio);
                    if (tiempoActual > duracionTotal) break;

                    if (cola.Count > 0)
                    {
                        int idCajero = Array.IndexOf(servidores, proximoFinServicio);
                        var cliente = cola.Dequeue();
                        cliente.TiempoInicioServicio = tiempoActual;
                        double duracionAtencion = GenerarExponencial(tiempoServicio);
                        cliente.TiempoFinServicio = tiempoActual + duracionAtencion;
                        servidores[idCajero] = cliente.TiempoFinServicio;
                        tiempoTotalOcupado += duracionAtencion;
                        atendidos.Add(cliente);
                    }
                    else
                    {
                        int idCajero = Array.IndexOf(servidores, proximoFinServicio);
                        servidores[idCajero] = proximaLlegada;
                    }
                }
            }

            reporte.ClientesAtendidos = atendidos.Count;
            reporte.ClientesRechazados = rechazados;
            reporte.TiempoEsperaPromedio = atendidos.Any() ? atendidos.Average(c => c.TiempoEspera) : 0;
            reporte.PorcentajeOcupacion = (tiempoTotalOcupado / (numCajeros * duracionTotal)) * 100;
            return reporte;
        }
    }

}
