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
        private LiquidacionTallerDAL _liquidTallerDal = new LiquidacionTallerDAL();
        //Solucion para no consultar dos veces el ultimo nro de control de LiquidacionTalleres
        //1-. Recibir como parametro el numero de liquidacion (periodo + id)
        //2-. Extraer del nro de liquidacion el id, que esta despues del periodo
        public int IngresarLiquidacionTaller(LiquidacionTallerDTO _liquidTaller) {
            string id = UltimoNroControlLiquidacion();
            int _nroControl = int.Parse(id == "" ? "0" : id) + 1;
            _liquidTaller.NroControl = _nroControl;
            return _liquidTallerDal.InsertLiquidacionTaller(_liquidTaller);
        }

        public string UltimoNroControlLiquidacion() {
            return _liquidTallerDal.SelectUltimoNroControlLiquidacionTaller();
        }

        public DataTable ListarLiquidaciones(string _codProv, int _anio) {
            return _liquidTallerDal.SelectLiquidacionTalleres(_codProv, _anio);
        }

        public DataTable ListarLiquidacionesLibres(string _codProv, string _anio, string mes)
        {
            return _liquidTallerDal.SelectLiquidacionLibreTalleres(_codProv, _anio, mes);
        }

        public DataTable ListarFacturacionesPorFecha(string rangoFechaDocumento)
        {
            string[] rangosFecha = rangoFechaDocumento.Split('-');
            string fechaInicial = rangosFecha[0].Trim();
            string fechaFinal = rangosFecha[1].Trim();
            return _liquidTallerDal.SelectLiquidacionTallerPorFecha(fechaInicial, fechaFinal);
        }

        public List<int> ListarYears() {
            List<int> years = new List<int>();
            int yearNow = DateTime.Now.Year;
            for (int y = 2015; y <= yearNow; y++)
                years.Add(y);
            return years.OrderByDescending(x=>x).ToList();
        }

        public List<int> ListarSemanas(int anio) {
            List<int> semanas = new List<int>();
            DateTime ultimoDia = (anio == DateTime.Now.Year) ? DateTime.Now : new DateTime(anio, 12, 31);
            Calendar c = CultureInfo.CurrentCulture.Calendar;
            int ultimaSemana = c.GetWeekOfYear(ultimoDia, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            for (int s = 1; s <= ultimaSemana; s++)
                semanas.Add(s);
            return semanas.OrderByDescending(x => x).ToList();
        }

        public DataTable ListarLiquidacionesPorSemana(int anio, int semana) {
            return _liquidTallerDal.SelectLiquidacionesPorSemana(anio, semana);
        }
    }
}
