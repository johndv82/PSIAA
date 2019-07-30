using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace PSIAA.DataAccessLayer
{
    public class ContratoDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD).
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta un procedimiento almacenado en la base de datos par aobtener el Avance General de Contrato.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con los datos del procedimiento.</returns>
        public DataTable SelectAvancePorContrato(string _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingProcedure("PSIAA.AvancePorContrato", _sqlParam);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado en la base de dastos para obtener el Detalle de Contrato Solicitado.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con los datos del procedimiento.</returns>
        public DataTable SelectDetalleContrato(int _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingProcedure("PSIAA.DetalleContrato", _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener todos los modelos de un Contrato.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectGrupoModelos(int _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                Cod_Modelo_AA 
                from contrato_detalle 
                where numero_contrato = @contrato
                group by Cod_Modelo_AA 
                order by Cod_Modelo_AA";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener el cliente de un determinado contrato.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public string SelectCliente(int _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                c.nombre 
                from contrato_cabecera cc inner join clientes c
	                on c.cod_cliente = cc.Cod_Cliente
                where cc.numero_contrato = @contrato
                and cc.cod_cliente !=0";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingEscalarQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener el estado si el contrato esta cerrado.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <returns>Varibale de Tipo string con el estado Si/No</returns>
        public string SelectVerificaContratoCerrado(int _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select
                 case 
	                when numero_contrato in(select distinct Nro_Contrato from Contratos_Cerrados where Nro_Contrato != 0) 
	                then 'Si' 
	                else 'No' 
                end as cerrado
                from contrato_cabecera 
                where numero_contrato = @contrato";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingEscalarQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener detalles secundarios (material, maquina, titulo, po),
        /// del contrato solicitado.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectDetalleModeloContrato(int contrato, string modelo) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                cd.Material_AA, 
	                (mb.ccodigo+'-'+mb.codigo+'-'+mb.diseno) as ModeloSap,
	                maq.c_denmaq as Maquina,
	                cd.Titulo,
	                cc.numero_p_o,
	                (maq.c_abr +' ' + maq.c_denmaq) as Linea
                from contrato_detalle cd 
                left join contrato_cabecera cc
                    on cc.numero_contrato = cd.numero_contrato
                left join modelo_bac mb
                    on cd.Cod_Modelo_AA = mb.c_codmod 
                    collate German_PhoneBook_CI_AS
                inner join maquina_bac maq
                    on maq.c_codmaq = cd.Linea
                    collate German_PhoneBook_CI_AS
                where cd.numero_contrato = @contrato and cd.Cod_Modelo_AA = @modelo
                group by
	                cd.Material_AA, 
	                (mb.ccodigo+'-'+mb.codigo+'-'+mb.diseno),
	                maq.c_denmaq,
	                cd.Titulo,
	                cc.numero_p_o,
	                (maq.c_abr +' ' + maq.c_denmaq)";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = contrato });
            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = modelo });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado en la base de datos para obtener el detalle del Reporte de Contrato.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con los datos del procedimiento.</returns>
        public DataTable SelectReporteContratoDet(int _contrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@ncontrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingProcedure("getContratoDetalle", _sqlParam);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado en la base de datos para obtener la cabecera del Reporte de Contrato.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con los datos del procedimiento.</returns>
        public DataTable SelectReporteContratoCab(int _contrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@ncontrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingProcedure("getContratoCabecera", _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener el tipo de un contrato.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <returns>Variable de tipo string con el tipo de contrato</returns>
        public DataRow SelectTipoContrato(int contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                cc.Tipo_Contrato,
	                tc.Descripcion
                from contrato_cabecera cc inner join Tipo_Contrato tc on tc.Tipo_Contrato = cc.Tipo_Contrato
                collate Modern_Spanish_100_CI_AS
                where cc.numero_contrato = @contrato";

            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = contrato });
            DataTable dtTC = _trans.ReadingQuery(query, _sqlParam);
            if (dtTC.Rows.Count > 0)
                return dtTC.Rows[0];
            else return null;
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener todos los contratos de un determinado modelo.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta</returns>
        public DataTable SelectContratosPorModelo(string modelo) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                numero_contrato 
                from contrato_detalle
                where Cod_Modelo_AA like '%' + @modelo + '%'
                group by numero_contrato";

            _sqlParam.Add(new SqlParameter("@modelo", SqlDbType.VarChar) { Value = modelo });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener todos los contrato de un determinado cliente.
        /// </summary>
        /// <param name="idCliente">Código de Cliente</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta.</returns>
        public DataTable SelectContratosPorCliente(int idCliente)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                numero_contrato 
                from contrato_cabecera
                where Cod_Cliente =  @idCliente
                group by numero_contrato";

            _sqlParam.Add(new SqlParameter("@idCliente", SqlDbType.Int) { Value = idCliente });
            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}

