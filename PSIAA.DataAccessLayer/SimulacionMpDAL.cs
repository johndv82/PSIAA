using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PSIAA.DataTransferObject;

namespace PSIAA.DataAccessLayer
{
    public class SimulacionMpDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión a la BD).
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la BD, para obtener el último correlativo de Simulación.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <returns>Variable de tipo string con el valor del correlativo. En el caso de que el valor sea vacío o nulo, se retornará 0.</returns>
        public string CorrelativoSimulacion(int contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
                    top 1 Correlativo 
                from PSIAA.Simulacion_Cabecera
                where Contrato = @numcontrato
                order by Correlativo desc";

            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = contrato });
            string rpta = _trans.ReadingEscalarQuery(query, _sqlParam);
            return rpta == "" ? "0" : rpta;
        }

        /// <summary>
        /// Ejecuta una consulta de inserción en la tabla PSIAA.Simulacion_Cabecera.
        /// </summary>
        /// <param name="numContrato">Número de Contrato</param>
        /// <param name="correlativo">Correlativo de Simulación</param>
        /// <param name="fecha">Fecha de Simulación</param>
        /// <param name="usuario">Nombre de Usuario</param>
        /// <returns>Variable de tipo int con la cantidad de registros ingresados.</returns>
        public int InsertSimulacionCabecera(int numContrato, int correlativo, DateTime fecha, string usuario) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                insert into PSIAA.Simulacion_Cabecera 
                values(
	                @numcontrato, @correlativo, @fecha, @usuario
                )";

            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = numContrato });
            _sqlParam.Add(new SqlParameter("@correlativo", SqlDbType.Int) { Value = correlativo });
            _sqlParam.Add(new SqlParameter("@fecha", SqlDbType.DateTime) { Value = fecha });
            _sqlParam.Add(new SqlParameter("@usuario", SqlDbType.VarChar) { Value = usuario });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la BD, para obtener las Simulaciones por Contrato y Modelos.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="modelos">Lista tipo string de Modelos de prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectSimulacionDetalleAlternativo(int contrato, List<string> modelos) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string whereModelos = "";

            for (int x = 0; x < modelos.Count; x++)
                modelos[x] = "'" + modelos[x] + "'";

            whereModelos = string.Join(",", modelos);

            string query = @"
                select 
                    Correlativo,
	                codSimulacion,
	                Maquina,
	                colorBase,
	                codProducto,
	                Material,
	                Modelo,
	                kilosSimulado,
	                porAdicional,
	                kilosTotales,
	                fechaIngreso,
	                horaIngreso,
	                Usuario,
                    nroContrato
                from PSIAA.Simulacion_Detalle
                where nroContrato = @contrato
                and Modelo in (" + whereModelos + ") order by codProducto";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = contrato });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de inserción en la tabla PSIAA.Simulacion_Detalle.
        /// </summary>
        /// <param name="simulacionDetDto">Objeto de tipo SimulacionDetDTO</param>
        /// <returns>Variable de tipo int con la cantidad de registros ingresados.</returns>
        public int InsertSimulacionDetalleAlternativo(SimulacionDetDTO simulacionDetDto) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                insert into PSIAA.Simulacion_Detalle values(
                    @correlativo,
	                @numcontrato, @numsimulacion, 
                    @maquina, @colorbase, @codproducto, 
	                @material, @modelo, @kilossimulado, @poradicional,
                    @kilostotales, @fechaingreso, @horaingreso, @usuario 
                )";

            _sqlParam.Add(new SqlParameter("@correlativo", SqlDbType.Int) { Value = simulacionDetDto.Correlativo });
            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = simulacionDetDto.NumContrato });
            _sqlParam.Add(new SqlParameter("@numsimulacion", SqlDbType.VarChar) { Value = simulacionDetDto.NroSimulacion });
            _sqlParam.Add(new SqlParameter("@maquina", SqlDbType.VarChar) { Value = simulacionDetDto.NombreMaquina });
            _sqlParam.Add(new SqlParameter("@colorbase", SqlDbType.VarChar) { Value = simulacionDetDto.Color });
            _sqlParam.Add(new SqlParameter("@codproducto", SqlDbType.VarChar) { Value = simulacionDetDto.CodProducto });
            _sqlParam.Add(new SqlParameter("@material", SqlDbType.VarChar) { Value = simulacionDetDto.DescProducto });
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = simulacionDetDto.Modelo });
            _sqlParam.Add(new SqlParameter("@kilossimulado", SqlDbType.Decimal) { Value = simulacionDetDto.KilosSimulado });
            _sqlParam.Add(new SqlParameter("@poradicional", SqlDbType.Decimal) { Value = simulacionDetDto.PorSeguridad });
            _sqlParam.Add(new SqlParameter("@kilostotales", SqlDbType.Decimal) { Value = simulacionDetDto.TotalKilos });
            _sqlParam.Add(new SqlParameter("@fechaingreso", SqlDbType.Date) { Value = simulacionDetDto.FechaIngreso });
            _sqlParam.Add(new SqlParameter("@horaingreso", SqlDbType.VarChar) { Value = simulacionDetDto.HoraIngreso });
            _sqlParam.Add(new SqlParameter("@usuario", SqlDbType.VarChar) { Value = simulacionDetDto.Usuario });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

        private int DeleteSimulacionDetalleAlternativo(int contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"delete from PSIAA.Simulacion_Detalle where nroContrato = @numcontrato";
            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = contrato });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

        private int DeleteSimulacionCabeceraAlternativo(int contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"delete from PSIAA.Simulacion_Cabecera where Contrato = @numcontrato";
            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = contrato });
            return _trans.ExecuteQuery(query, _sqlParam);
        }
    }
}
