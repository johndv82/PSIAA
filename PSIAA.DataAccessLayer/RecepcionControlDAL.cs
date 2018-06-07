using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PSIAA.DataTransferObject;

namespace PSIAA.DataAccessLayer
{
    public class RecepcionControlDAL
    {
        private Transactions _trans = new Transactions();

        public DataTable SelectRecepcionControl(int _almacen) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select
	                Orden, Lote,
	                talla1, talla2, talla3,
	                talla4, talla5, talla6,
	                talla7, talla8, talla9,
	                piezas1, piezas2, piezas3,
                    piezas4, piezas5, piezas6,
                    piezas7, piezas8, piezas9,
	                hora_ingreso as 'Hora',
	                usuario,
	                Completo
                from Recepcion_Pto_Control
                where almacen = 550 and convert(date, fecha_ingreso)  = convert(date, getdate())
                order by hora_ingreso desc";

            _sqlParam.Add(new SqlParameter("@almacen", SqlDbType.Int) { Value = _almacen });

            return _trans.ReadingQuery(query, _sqlParam);
        }

        //Seguimiento Por Orden --> Almacen/Ctrl Final
        public DataTable SelectSeguimientoRecepcionControl(string _orden) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _orden });

            return _trans.ReadingProcedure("PSIAA.SeguimientoRecepcionControl", _sqlParam);
        }

        public int InsertRecepcionControl(RecepcionControlDTO _controlFinal) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                insert into Recepcion_Pto_Control(
	                almacen, 
	                Orden, 
	                Lote, 
	                talla1, talla2, talla3, 
	                talla4, talla5, talla6, 
	                talla7, talla8, talla9, 
	                piezas1, piezas2, piezas3, 
	                piezas4, piezas5, piezas6, 
	                piezas7, piezas8, piezas9, 
	                usuario, 
	                fecha_ingreso, 
	                hora_ingreso, 
	                catoper, 
	                Peso, 
	                observaciones, 
	                Completo
                ) values(
	                @almacen, @orden, @lote, @talla1, @talla2, @talla3, @talla4, @talla5, @talla6, @talla7, 
                    @talla8, @talla9, @piezas1, @piezas2, @piezas3, @piezas4, @piezas5, @piezas6, @piezas7, 
                    @piezas8, @piezas9,  @usuario, convert(date, @fechaingreso), @hora, @catoper, @peso, @obs, @completo)";

            _sqlParam.Add(new SqlParameter("@almacen", SqlDbType.Int) { Value = _controlFinal.Almacen });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _controlFinal.Orden.ToUpper() });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.SmallInt) { Value = _controlFinal.Lote });

            _sqlParam.Add(new SqlParameter("@talla1", SqlDbType.VarChar) { Value = _controlFinal.Tallas[0] });
            _sqlParam.Add(new SqlParameter("@talla2", SqlDbType.VarChar) { Value = _controlFinal.Tallas[1] });
            _sqlParam.Add(new SqlParameter("@talla3", SqlDbType.VarChar) { Value = _controlFinal.Tallas[2] });
            _sqlParam.Add(new SqlParameter("@talla4", SqlDbType.VarChar) { Value = _controlFinal.Tallas[3] });
            _sqlParam.Add(new SqlParameter("@talla5", SqlDbType.VarChar) { Value = _controlFinal.Tallas[4] });
            _sqlParam.Add(new SqlParameter("@talla6", SqlDbType.VarChar) { Value = _controlFinal.Tallas[5] });
            _sqlParam.Add(new SqlParameter("@talla7", SqlDbType.VarChar) { Value = _controlFinal.Tallas[6] });
            _sqlParam.Add(new SqlParameter("@talla8", SqlDbType.VarChar) { Value = _controlFinal.Tallas[7] });
            _sqlParam.Add(new SqlParameter("@talla9", SqlDbType.VarChar) { Value = _controlFinal.Tallas[8] });

            _sqlParam.Add(new SqlParameter("@piezas1", SqlDbType.Int) { Value = _controlFinal.Piezas[0] });
            _sqlParam.Add(new SqlParameter("@piezas2", SqlDbType.Int) { Value = _controlFinal.Piezas[1] });
            _sqlParam.Add(new SqlParameter("@piezas3", SqlDbType.Int) { Value = _controlFinal.Piezas[2] });
            _sqlParam.Add(new SqlParameter("@piezas4", SqlDbType.Int) { Value = _controlFinal.Piezas[3] });
            _sqlParam.Add(new SqlParameter("@piezas5", SqlDbType.Int) { Value = _controlFinal.Piezas[4] });
            _sqlParam.Add(new SqlParameter("@piezas6", SqlDbType.Int) { Value = _controlFinal.Piezas[5] });
            _sqlParam.Add(new SqlParameter("@piezas7", SqlDbType.Int) { Value = _controlFinal.Piezas[6] });
            _sqlParam.Add(new SqlParameter("@piezas8", SqlDbType.Int) { Value = _controlFinal.Piezas[7] });
            _sqlParam.Add(new SqlParameter("@piezas9", SqlDbType.Int) { Value = _controlFinal.Piezas[8] });

            _sqlParam.Add(new SqlParameter("@usuario", SqlDbType.VarChar) { Value = _controlFinal.Usuario });
            _sqlParam.Add(new SqlParameter("@fechaingreso", SqlDbType.DateTime) { Value = _controlFinal.FechaIngreso });
            _sqlParam.Add(new SqlParameter("@hora", SqlDbType.VarChar) { Value = _controlFinal.HoraIngreso });
            _sqlParam.Add(new SqlParameter("@catoper", SqlDbType.Int) { Value = _controlFinal.Almacen });
            _sqlParam.Add(new SqlParameter("@peso", SqlDbType.Float) { Value = _controlFinal.Peso });
            _sqlParam.Add(new SqlParameter("@obs", SqlDbType.VarChar) { Value = _controlFinal.Observaciones });
            _sqlParam.Add(new SqlParameter("@completo", SqlDbType.VarChar) { Value = _controlFinal.Completo });

            return _trans.ExecuteQuery(query, _sqlParam);
        }

        //Seguimiento al punto de control -> Avance Contrato Principal
        public DataTable SelectRecepcionPorModeloColor(string _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.VarChar) { Value =  _contrato});
            return _trans.ReadingProcedure("PSIAA.RecepcionPorModeloColor", _sqlParam);
        }

        public int SelectCantidadRecepcion(string _orden, int _lote, int _punto) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                sum(case 
		                when piezas1 != 0 then piezas1
		                when piezas2 != 0 then piezas2
		                when piezas3 != 0 then piezas3
		                when piezas4 != 0 then piezas4
		                when piezas5 != 0 then piezas5
		                when piezas6 != 0 then piezas6
		                when piezas7 != 0 then piezas7
		                when piezas8 != 0 then piezas8
		                when piezas9 != 0 then piezas9
	                end) as Cantidad
                from Recepcion_Pto_Control where almacen = @punto and Orden = @orden and Lote = @lote
                group by 
	                Orden, Lote";

            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _lote });
            _sqlParam.Add(new SqlParameter("@punto", SqlDbType.Int) { Value = _punto });

            string _result = _trans.ReadingEscalarQuery(query, _sqlParam);

            return _result == "" ? 0: int.Parse(_result);
        }

        //Avance detallado del contrato
        public DataTable SelectAvanceDetalladoPorTallas(int _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingProcedure("PSIAA.AvanceDetalladoPorTallas", _sqlParam);
        }

        public DataTable SelectIngresosFaltantesAlmacen(int _contrato, string _modelo) {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            _procedureParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value =  _modelo});

            return _trans.ReadingProcedure("PSIAA.IngresosFaltantesAlmacen", _procedureParam);
        }

        public DataTable SelectIngresosPuntoControl(string _fechaIni, string _fechaFin) {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@fechaini", SqlDbType.VarChar) { Value = _fechaIni });
            _procedureParam.Add(new SqlParameter("@fechafin", SqlDbType.VarChar) { Value = _fechaFin });

            return _trans.ReadingProcedure("PSIAA.IngresosPuntoControl", _procedureParam);
        }

        public string SelectExistenciaOrdenLote(string _orden, int _lote) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                Orden 
                from Recepcion_Pto_Control where Orden = @orden and Lote = @lote
                group by Orden";

            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _lote });

            return _trans.ReadingEscalarQuery(query, _sqlParam);
        }
    }
}
