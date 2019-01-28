using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer.SAP;
using System.Data;

namespace PSIAA.BusinessLogicLayer.SAP
{
    public class PackingListBLL
    {
        /// <summary>
        /// Variable de instancia a la clase PackingListDAL.
        /// </summary>
        public readonly PackingListDAL _packingListDal = new PackingListDAL();

        /// <summary>
        /// Ejecuta un procedimiento DAL de cabecera de Packing List, y evalúa la existencia de datos en su contenedor,
        /// para para retornar la primera Fila de Datos, en caso contrario se retorna nulo.
        /// </summary>
        /// <param name="docEntry">Documento de Entrada</param>
        /// <returns>Fila de datos de tipo DataRow, con la cabecera.</returns>
        public DataRow PackingListCabecera(int docEntry) {
            DataTable dtCab = _packingListDal.SelectPackingListCabecera(docEntry);
            if (dtCab.Rows.Count > 0) {
                return dtCab.Rows[0];
            } else
                return null;
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de detalle de Packing List, y retorna el resultado.
        /// </summary>
        /// <param name="docEntry">Documento de Entrada</param>
        /// <returns>Contenedor de tipo DataTable con el detalle.</returns>
        public DataTable PackingListDetalle(int docEntry) {
            return _packingListDal.SelectPackingListDetalle(docEntry);
        }

        /// <summary>
        /// Ejecuta una consulta DAL para buscar el documento de entrada, y retorna el resultado.
        /// </summary>
        /// <param name="tipo">Tipo de Documento</param>
        /// <param name="serie">Serie de Documento</param>
        /// <param name="correlativo">Correlativo</param>
        /// <returns>Variable de tipo int con el número de documento</returns>
        public int BuscarDocumentoEntry(string tipo, string serie, string correlativo) {
            return _packingListDal.SelectDocumentoEntry(tipo, serie, correlativo);
        }
    }
}
