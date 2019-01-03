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
        private Transactions _trans = new Transactions();

        //Avance Contrato Principal
        public DataTable SelectAvancePorContrato(string _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingProcedure("PSIAA.AvancePorContrato", _sqlParam);
        }

        //Para detalle solicitado y lanzamiento
        public DataTable SelectDetalleContrato(int _contrato) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingProcedure("PSIAA.DetalleContrato", _sqlParam);
        }

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

        //Metodos de acceso a procedure para Reporte de Contrato
        public DataTable SelectReporteContratoDet(int _contrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@ncontrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingProcedure("getContratoDetalle", _sqlParam);
        }

        public DataTable SelectReporteContratoCab(int _contrato)
        {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            _sqlParam.Add(new SqlParameter("@ncontrato", SqlDbType.Int) { Value = _contrato });
            return _trans.ReadingProcedure("getContratoCabecera", _sqlParam);
        }

        public string SelectTipoContrato(string orden) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                cc.Tipo_Contrato 
                from lanzamiento_detalle ld
                inner join contrato_cabecera cc on cc.numero_contrato = ld.numero_documento
                where Orden = @orden
                group by cc.Tipo_Contrato";

            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = orden });
            return _trans.ReadingEscalarQuery(query, _sqlParam).ToString();
        }

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

