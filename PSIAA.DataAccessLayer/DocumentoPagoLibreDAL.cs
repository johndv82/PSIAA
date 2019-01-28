using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using PSIAA.DataTransferObject;

namespace PSIAA.DataAccessLayer
{
    public class DocumentoPagoLibreDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD).
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener todas las Operaciónes Libres SIAA.
        /// </summary>
        /// <returns>Contenedor de tipo DataTable con el resultado de la consulta.</returns>
        public DataTable SelectOperacionesLibres() {
            string query = @"
                select
	                Cod_operacion,
	                Denominacion
                from Operaciones_Libres
                where Estado is null
                order by Cod_operacion";
            return _trans.ReadingQuery(query, null);
        }

        /// <summary>
        /// Ejecuta una consulta de inserción a la tabla Doc_pago_taller_libre.
        /// </summary>
        /// <param name="_docLibre">Objeto de tipo DocumentoPagoLibreDTO</param>
        /// <returns>Variable de tipo int con la cantidad de registros ingresados.</returns>
        public int InsertDocumentoPagoLibre(DocumentoPagoLibreDTO _docLibre) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                insert into Doc_pago_taller_libre(
	                cod_proveedor, 
	                tipo_movimiento, 
	                serie_documento, 
	                nro_documento, 
	                Cod_Cat_Oper, 
	                Nro_Ord_Asig, 
	                Orden, Lote, 
	                Categoria, 
	                Cod_Operacion_Libre, 
	                Talla, Prendas, 
	                Tiempo, Moneda, 
	                Precio, Total, 
	                Observaciones
                )
                values(
	                @codproveedor, @tipomov, 0, 
	                @nrodocumento, 0, 0, 
	                @orden, @lote, 0, 
	                @codoperacion, @talla, 
	                @prendas, @tiempo, @moneda, @precio, @total, 
	                @observaciones
                )";

            _sqlParam.Add(new SqlParameter("@codproveedor", SqlDbType.VarChar) { Value = _docLibre.CodProveedor });
            _sqlParam.Add(new SqlParameter("@tipomov", SqlDbType.VarChar) { Value = _docLibre.TipoMovimiento });
            _sqlParam.Add(new SqlParameter("@nrodocumento", SqlDbType.Int) { Value = _docLibre.NroDocumento });
            _sqlParam.Add(new SqlParameter("@orden", SqlDbType.VarChar) { Value = _docLibre.Orden });
            _sqlParam.Add(new SqlParameter("@lote", SqlDbType.Int) { Value = _docLibre.Lote });
            _sqlParam.Add(new SqlParameter("@codoperacion", SqlDbType.VarChar) { Value = _docLibre.CodOperacion });
            _sqlParam.Add(new SqlParameter("@talla", SqlDbType.VarChar) { Value = _docLibre.Talla });
            _sqlParam.Add(new SqlParameter("@prendas", SqlDbType.Int) { Value = _docLibre.Prendas });
            _sqlParam.Add(new SqlParameter("@tiempo", SqlDbType.Decimal) { Value = _docLibre.Tiempo });
            _sqlParam.Add(new SqlParameter("@moneda", SqlDbType.VarChar) { Value = _docLibre.Moneda });
            _sqlParam.Add(new SqlParameter("@precio", SqlDbType.Decimal) { Value = _docLibre.Precio });
            _sqlParam.Add(new SqlParameter("@total", SqlDbType.Decimal) { Value = _docLibre.Total });
            _sqlParam.Add(new SqlParameter("@observaciones", SqlDbType.VarChar) { Value = _docLibre.Observaciones });
            return _trans.ExecuteQuery(query, _sqlParam);
        }
    }
}
