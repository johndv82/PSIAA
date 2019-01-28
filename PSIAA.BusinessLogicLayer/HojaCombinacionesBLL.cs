using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataAccessLayer;
using PSIAA.DataTransferObject;
using PSIAA.DataAccessLayer.ODOO;
using PSIAA.DataAccessLayer.TuartDB;
using System.Data;

namespace PSIAA.BusinessLogicLayer
{
    public class HojaCombinacionesBLL
    {
        /// <summary>
        /// Variable de instancia a la clase HojaCombinacionesDAL.
        /// </summary>
        public HojaCombinacionesDAL _hojaCombinacionesDal = new HojaCombinacionesDAL();
        /// <summary>
        /// Variable de instancia a la clase CombinacionColorDAL.
        /// </summary>
        public CombinacionColorDAL _combinacionColorDal = new CombinacionColorDAL();
        /// <summary>
        /// Variable de instancia a la clase CombMaterialDAL.
        /// </summary>
        public CombMaterialDAL _combMaterial = new CombMaterialDAL();

        /// <summary>
        /// Ejecuta procedimientos DAL de Colores/Porcentajes y Materiales por Modelo, con ambos resultados se hace un proceso de 
        /// matching en base a su código de material, y el resultado se lista.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <param name="correlativoColor">Correlativo de la Combinación</param>
        /// <param name="kilosNecesarios">Cantidad en kilos, en el caso se quiera calcular materia prima por porcentaje de combinación.</param>
        /// <returns>Lista Genérica de tipo CombinacionDTO con el resultado del matching.</returns>
        public List<CombinacionDTO> ListarColoresCombinacion(string modelo, string correlativoColor, decimal kilosNecesarios)
        {
            string[] partes = modelo.Split('-');
            List<CombinacionDTO> _listCombinacionColores = new List<CombinacionDTO>();
            if (partes.Length > 0)
            {
                //DataTable dtColorMaterialOdoo = _combinacionColorDal.SelectColorMateriales(modelo, Helper.Mascara(Convert.ToInt32(correlativoColor), "C000"));
                //Ya no consultar al ODOO
                DataTable dtColorMaterialOdoo = new DataTable();

                if (dtColorMaterialOdoo.Rows.Count > 0)
                {
                    foreach (DataRow row in dtColorMaterialOdoo.Rows)
                    {
                        CombinacionDTO comb = new CombinacionDTO()
                        {
                            Modelo = row["c_codmod"].ToString(),
                            Color = row["value_color"].ToString(),
                            Porcentaje = decimal.Parse(row["percent"].ToString()),
                            Kilos = (kilosNecesarios / 100) * decimal.Parse(row["percent"].ToString()),
                            Producto = row["tipo_material"].ToString() + "-" + row["Calidad"].ToString()
                                                            + "-HC-" + row["titulo"].ToString() + "-" +
                                                            row["value_color"].ToString(),
                            Titulo = row["titulo"].ToString(),
                            DescripcionMaterial = row["denominacion_material"].ToString()
                        };
                        _listCombinacionColores.Add(comb);
                    };
                }
                else
                {
                    string _familia = partes[0].Substring(0, 1);
                    int _correlativoModelo = int.Parse(partes[0].Substring(1, partes[0].Length - 1));
                    DataTable dtColores = _hojaCombinacionesDal.SelectColoresPorcentajes(_familia, _correlativoModelo, short.Parse(correlativoColor));
                    DataTable dtMateriales = _combMaterial.SelectCombinacionMaterial(modelo);

                    _listCombinacionColores = (from col in dtColores.AsEnumerable()
                                               join mat in dtMateriales.AsEnumerable()
                                                 on col.Field<string>("Materiales").ToString().Trim()
                                                 equals mat.Field<string>("c_codmat")
                                                into outer
                                               from mat in outer
                                               select new CombinacionDTO
                                               {
                                                   Modelo = col.Field<string>("Modelo"),
                                                   Color = col.Field<string>("Colores"),
                                                   Porcentaje = decimal.Parse(col.Field<float>("Porcentajes").ToString()),
                                                   Kilos = (kilosNecesarios / 100) * decimal.Parse(col.Field<float>("Porcentajes").ToString()),
                                                   Producto = mat.Field<string>("tipo") + "-" + col.Field<string>("Materiales").ToString().Trim()
                                                                + "-HC-" + col.Field<string>("Titulos") + "-" + 
                                                                col.Field<string>("Colores").ToString().Trim(),
                                                   Titulo = col.Field<string>("Titulos"),
                                                   DescripcionMaterial = mat.Field<string>("c_denmat")
                                               }).ToList();
                }
                return _listCombinacionColores;
            }
            else {
                return new List<CombinacionDTO>();
            }
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Corrección de colores.
        /// </summary>
        /// <param name="_contrato">Número de Contrato</param>
        /// <returns>Variable booleana en el caso que el conteo de filas sea mayor a 0.</returns>
        public bool CorregirColores(int _contrato)
        {
            return (_hojaCombinacionesDal.CorreccionColoresSiaa(_contrato).Rows.Count > 0);
        }

        /// <summary>
        /// Ejecuta un procedimiento DAL de Materiales por Color Entero de un Modelo.
        /// </summary>
        /// <param name="famili">Grupo Familiar de Modelo</param>
        /// <param name="correlativo">Correletivo de Modelo</param>
        /// <returns>Contenedor de tipo DataTable con los materiales.</returns>
        public DataTable MaterialPorColor(string famili, int correlativo) {
            return _hojaCombinacionesDal.SelectMaterialUnSoloColor(famili, correlativo);
        }
    }
}
