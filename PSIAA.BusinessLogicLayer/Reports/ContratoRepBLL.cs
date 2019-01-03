using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataTransferObject.Report;
using PSIAA.DataAccessLayer;
using System.Data;

namespace PSIAA.BusinessLogicLayer.Reports
{
    public class ContratoRepBLL
    {
        /// <summary>
        /// Variable de instancia a la clase ContratoDAL.
        /// </summary>
        public ContratoDAL _contratoDal = new ContratoDAL();

        /// <summary>
        /// Ejecuta procedimientos DAL de Reporte Contrato y obtiene si detalle y cabecera en contenedores de datos que son seteados
        /// en un objeto de tipo ContratoCabDTO, en el caso de que el contenedor este vacio devuelve el objeto tambien vacio.
        /// </summary>
        /// <param name="_numContrato">Número de Contrato</param>
        /// <returns>Objeto de tipo ContratoCabDTO poblado con todos sus datos y su detalle de tipo ContratoDetDTO.</returns>
        public ContratoCabDTO Cabecera(int _numContrato)
        {
            var primeraFila = string.Empty;

            DataTable dtCabecera = _contratoDal.SelectReporteContratoCab(_numContrato);
            DataTable dtDetalle = _contratoDal.SelectReporteContratoDet(_numContrato);

            if (dtCabecera.Rows.Count > 0)
            {
                var cabecera = new ContratoCabDTO
                {
                    NumContrato = dtCabecera.Rows[0]["Num Contrato"].ToString(),
                    TipoContrato = dtCabecera.Rows[0]["Tipo Contrato"].ToString(),
                    Fecha = dtCabecera.Rows[0]["Fecha"].ToString(),
                    Cliente = dtCabecera.Rows[0]["Cliente"].ToString(),
                    Orden = dtCabecera.Rows[0]["Orden"].ToString(),
                    FechaEnvio = dtCabecera.Rows[0]["Fecha Envio"].ToString(),
                    Tolerancia = dtCabecera.Rows[0]["Tolerancia"].ToString(),
                    ToleranciaTiempo = int.Parse(dtCabecera.Rows[0]["Tolerancia Tiempo"].ToString()),
                    HojaL = dtCabecera.Rows[0]["Hoja L"].ToString(),
                    Moneda = dtCabecera.Rows[0]["Moneda"].ToString(),
                    Destino = dtCabecera.Rows[0]["Destino"].ToString(),
                    Observaciones = dtCabecera.Rows[0]["Observaciones"].ToString()
                };
                foreach (DataRow item in dtDetalle.Rows)
                {
                    var detalle = new ContratoDetDTO
                    {
                        ModeloAA = item["Modelo AA"].ToString(),
                        ModeloSAP = item["Codigo SAP"].ToString(),
                        ModeloCliente = item["Modelo Cliente"].ToString(),
                        Descripcion = item["Descripcion"].ToString(),
                        Material = item["Material"].ToString(),
                        Titulo = item["Titulo"].ToString(),
                        Maquina = item["Maquina"].ToString(),
                        Galga = item["Galga"].ToString(),
                        CodColor = item["Cod Color"].ToString(),
                        Color = item["Color"].ToString(),
                        CTalla1 = int.Parse(item["CTalla1"].ToString()),
                        CTalla2 = int.Parse(item["CTalla2"].ToString()),
                        CTalla3 = int.Parse(item["CTalla3"].ToString()),
                        CTalla4 = int.Parse(item["CTalla4"].ToString()),
                        CTalla5 = int.Parse(item["CTalla5"].ToString()),
                        CTalla6 = int.Parse(item["CTalla6"].ToString()),
                        CTalla7 = int.Parse(item["CTalla7"].ToString()),
                        CTalla8 = int.Parse(item["CTalla8"].ToString()),
                        CTalla9 = int.Parse(item["CTalla9"].ToString()),
                        //CantMuestra = int.Parse(item["Cant Muestra"].ToString()),
                        TMuestra = item["TMuestra"].ToString(),
                        Total = int.Parse(item["Total"].ToString()),
                        NTalla1 = item["NTalla1"].ToString(),
                        NTalla2 = item["NTalla2"].ToString(),
                        NTalla3 = item["NTalla3"].ToString(),
                        NTalla4 = item["NTalla4"].ToString(),
                        NTalla5 = item["NTalla5"].ToString(),
                        NTalla6 = item["NTalla6"].ToString(),
                        NTalla7 = item["NTalla7"].ToString(),
                        NTalla8 = item["NTalla8"].ToString(),
                        NTalla9 = item["NTalla9"].ToString()
                    };
                    if (item["Cant Muestra"] == DBNull.Value)
                        detalle.CantMuestra = 0;
                    else
                        detalle.CantMuestra = int.Parse(item["Cant Muestra"].ToString());

                    cabecera.Detalle.Add(detalle);
                }
                return cabecera;
            }
            else
            {
                return new ContratoCabDTO();
            }
        }
    }
}
