using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using PSIAA.DataTransferObject;
using System.Data;
using System.Globalization;

namespace PSIAA.BusinessLogicLayer
{
    public class LiquidacionTallerBLL
    {
        /// <summary>
        /// Variable de instancia a la clase LiquidacionTallerDAL.
        /// </summary>
        public LiquidacionTallerDAL _liquidTallerDal = new LiquidacionTallerDAL();

        /// <summary>
        /// Genera un nuevo número de control de liquidación para modificar el campo: NroControl del objeto que viene como parámetro.
        /// Ejecuta un procedimiento DAL de Insert Liquidación Taller, enviando como parametro el objeto modificado.
        /// </summary>
        /// <param name="_liquidTaller">Objeto de tipo LiquidacionTallerDTO con los datos de liquidación.</param>
        /// <returns>Variable de tipo int con la cantidad de ingresos.</returns>
        public int IngresarLiquidacionTaller(LiquidacionTallerDTO _liquidTaller) {
            string id = UltimoNroControlLiquidacion();
            int _nroControl = int.Parse(id == "" ? "0" : id) + 1;
            _liquidTaller.NroControl = _nroControl;
            return _liquidTallerDal.InsertLiquidacionTaller(_liquidTaller);
        }

        /// <summary>
        /// Ejecua un procedimiento DAL de Ultimo Número de Control de Liquidación.
        /// </summary>
        /// <returns>Variable de tipo string  con el último número de control.</returns>
        public string UltimoNroControlLiquidacion() {
            return _liquidTallerDal.SelectUltimoNroControlLiquidacionTaller();
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Liquidación Talleres, y retorna el resultado.
        /// </summary>
        /// <param name="_codProv">Código de Proveedor</param>
        /// <param name="_anio">Año de Liquidación</param>
        /// <returns>Contenedor de tipo DataTable con las liquidaciones.</returns>
        public DataTable ListarLiquidaciones(string _codProv, int _anio) {
            return _liquidTallerDal.SelectLiquidacionTalleres(_codProv, _anio);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_codProv">Código de Proveedor</param>
        /// <param name="_anio">Año de Lqiuidación</param>
        /// <param name="mes">Mes de Liquidación</param>
        /// <returns></returns>
        public DataTable ListarLiquidacionesLibres(string _codProv, string _anio, string mes)
        {
            return _liquidTallerDal.SelectLiquidacionLibreTalleres(_codProv, _anio, mes);
        }

        /// <summary>
        /// Desfragmenta la cadena de entrada para obtener la fecha inicial y final, y con el resultado ejecuta un procedimiento DAL
        /// de Liquidaciones de Taller por Fecha de ingreso.
        /// </summary>
        /// <param name="rangoFechaDocumento">Cadena con la Fecha Inicial y Final de consulta.</param>
        /// <returns>Contenedor de tipo DataTable con las liquidaciones retornadas.</returns>
        public DataTable ListarFacturacionesPorFecha(string rangoFechaDocumento)
        {
            string[] rangosFecha = rangoFechaDocumento.Split('-');
            string fechaInicial = rangosFecha[0].Trim();
            string fechaFinal = rangosFecha[1].Trim();
            return _liquidTallerDal.SelectLiquidacionTallerPorFecha(fechaInicial, fechaFinal);
        }

        /// <summary>
        /// Genera un listado de años desde el 2015 hasta la actualidad.
        /// </summary>
        /// <returns>Lista genérica de tipo int con los años generados.</returns>
        public List<int> ListarYears() {
            List<int> years = new List<int>();
            int yearNow = DateTime.Now.Year;
            for (int y = 2015; y <= yearNow; y++)
                years.Add(y);
            return years.OrderByDescending(x=>x).ToList();
        }

        /// <summary>
        /// Genera un listado de números de semana por el año de entrada.
        /// </summary>
        /// <param name="anio">Año base</param>
        /// <returns>Lista genérica de tipo int con los números de semana generados.</returns>
        public List<int> ListarSemanas(int anio) {
            List<int> semanas = new List<int>();
            DateTime ultimoDia = (anio == DateTime.Now.Year) ? DateTime.Now : new DateTime(anio, 12, 31);
            Calendar c = CultureInfo.CurrentCulture.Calendar;
            int ultimaSemana = c.GetWeekOfYear(ultimoDia, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            for (int s = 1; s <= ultimaSemana; s++)
                semanas.Add(s);
            return semanas.OrderByDescending(x => x).ToList();
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Liquidaciones por Semana, y retorna el resultado.
        /// </summary>
        /// <param name="anio">Año de liquidación</param>
        /// <param name="semana">Número de semana de liquidación</param>
        /// <returns>Contenedor de tipo DataTable con las liquidaciones retornadas.</returns>
        public DataTable ListarLiquidacionesPorSemana(int anio, int semana) {
            return _liquidTallerDal.SelectLiquidacionesPorSemana(anio, semana);
        }
    }
}
