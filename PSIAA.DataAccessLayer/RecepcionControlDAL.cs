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
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión a la BD).
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la BD, para obtener los ingresos en Recepción por Punto de Control.
        /// </summary>
        /// <param name="_almacen">Código de Almacén</param>
        /// <returns>Contenedor de tipo DataTable con el resultado de la consulta.</returns>
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
                where almacen = @almacen and convert(date, fecha_ingreso)  = convert(date, getdate())
                order by hora_ingreso desc";

            _sqlParam.Add(new SqlParameter("@almacen", SqlDbType.Int) { Value = _almacen });

            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la BD, para obteener el Seguimiento de una Orden de Producción en cada punto
        /// de Recepción, desde lanzamiento hasta almacén de productos terminados.
        /// </summary>
        /// <param name="_orden">Orden de Producción</param>
        /// <returns>Contenedor de tipo DataTable con los datos del procedimiento.</returns>
        public DataTable SelectSeguimientoRecepcionControl(string _orden) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _orden });

            return _trans.ReadingProcedure("PSIAA.SeguimientoRecepcionControl", _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de inserción en la tabla Recepcion_Pto_Control a la BD.
        /// </summary>
        /// <param name="_controlFinal">Objeto de tipo RecepcionControlDTO</param>
        /// <returns>Variable de tipo int con la cantidad de registros ingresados.</returns>
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

        /// <summary>
        /// Ejecuta un procedimiento almacenado en la BD, para obtener la Recepcion Punto de Control, por Contrasto.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectRecepcionPorContrato(string _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.VarChar) { Value =  _contrato});
            //Cambiar nombre a Procedure a: RecepcionPorContrato
            return _trans.ReadingProcedure("PSIAA.RecepcionPorModeloColor", _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la BD, para obtener la cantidad de recepción en un punto de control,
        /// por orden de producción y lote.
        /// </summary>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <param name="_punto">Punto de Control</param>
        /// <returns>Variable de tipo int con la cantidad recepcionada.</returns>
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

        /// <summary>
        /// Ejecuta un procedimiento almacenado en la BD, para obtener el avance detallado por tallas de un contrato.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con los datos del procedimiento.</returns>
        public DataTable SelectAvanceDetalladoPorTallas(int _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingProcedure("PSIAA.AvanceDetalladoPorTallas", _sqlParam);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado en la BD, para obtener los Ingresos Faltantes a Almacén.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <param name="_modelo">Modelo de Prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos del procedimiento.</returns>
        public DataTable SelectIngresosFaltantesAlmacen(int _contrato, string _modelo) {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            _procedureParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value =  _modelo});

            return _trans.ReadingProcedure("PSIAA.IngresosFaltantesAlmacen", _procedureParam);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado en la BD, para obtener todos los ingresos en recepdión por punto de control.
        /// </summary>
        /// <param name="_fechaIni">Fecha Inicial</param>
        /// <param name="_fechaFin">Fecha Final</param>
        /// <returns>Contenedor de tipo DataTable con los datos del procedimiento.</returns>
        public DataTable SelectIngresosPuntoControl(string _fechaIni, string _fechaFin) {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();

            _procedureParam.Add(new SqlParameter("@fechaini", SqlDbType.VarChar) { Value = _fechaIni });
            _procedureParam.Add(new SqlParameter("@fechafin", SqlDbType.VarChar) { Value = _fechaFin });

            return _trans.ReadingProcedure("PSIAA.IngresosPuntoControl", _procedureParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la BD, para validar la existencia de una Orden de Producción y Lote.
        /// </summary>
        /// <param name="_orden">Orden de Producción</param>
        /// <param name="_lote">Número de Lote</param>
        /// <returns>Variable de tipo string con el valor de la órden.</returns>
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
