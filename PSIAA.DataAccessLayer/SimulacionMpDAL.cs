using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PSIAA.DataTransferObject;

namespace PSIAA.DataAccessLayer
{
    public class SimulacionMpDAL
    {
        private Transactions _trans = new Transactions();

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

        public int InsertSimulacionDetalle(SimulacionDetDTO simulacionDetDto) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                insert into Simulacion_detalle
                values(
	                @numcontrato, @item, 
                    @maquina, @codproducto, @modelo, 0, 
                    @totalkilos, 0, @kilos, @usuario, 
	                convert(date, @fechaingreso), 
                    @horaingreso, '', null
                )";

            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = simulacionDetDto.NumContrato });
            _sqlParam.Add(new SqlParameter("@item", SqlDbType.Int) { Value = 0 });
            _sqlParam.Add(new SqlParameter("@maquina", SqlDbType.VarChar) { Value = string.Empty });
            _sqlParam.Add(new SqlParameter("@codproducto", SqlDbType.VarChar) { Value = simulacionDetDto.CodProducto });
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = simulacionDetDto.Modelo });
            //_sqlParam.Add(new SqlParameter("@kilos", SqlDbType.Float) { Value = simulacionDetDto.Kilos });
            _sqlParam.Add(new SqlParameter("@usuario", SqlDbType.VarChar) { Value = simulacionDetDto.Usuario });
            _sqlParam.Add(new SqlParameter("@fechaingreso", SqlDbType.DateTime) { Value = simulacionDetDto.FechaIngreso });
            _sqlParam.Add(new SqlParameter("@horaingreso", SqlDbType.VarChar) { Value = simulacionDetDto.HoraIngreso });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

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

        public int DeleteSimulacionDetalleAlternativo(int contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"delete from PSIAA.Simulacion_Detalle where nroContrato = @numcontrato";
            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = contrato });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

        public int DeleteSimulacionCabeceraAlternativo(int contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"delete from PSIAA.Simulacion_Cabecera where Contrato = @numcontrato";
            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = contrato });
            return _trans.ExecuteQuery(query, _sqlParam);
        }

        public int DeleteSimulacionDetalle(int contrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"delete from Simulacion_detalle where No_Contrato = @numcontrato";
            _sqlParam.Add(new SqlParameter("@numcontrato", SqlDbType.Int) { Value = contrato });
            return _trans.ExecuteQuery(query, _sqlParam);
        }
    }
}
