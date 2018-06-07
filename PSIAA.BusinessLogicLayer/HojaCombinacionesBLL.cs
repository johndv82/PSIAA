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
        private HojaCombinacionesDAL _hojaCombinacionesDal = new HojaCombinacionesDAL();
        private CombinacionColorDAL _combinacionColorDal = new CombinacionColorDAL();
        private CombMaterialDAL _combMaterial = new CombMaterialDAL();

        public List<CombinacionDTO> ListarColoresCombinacion(string modelo, string correlativoColor, decimal kilosNecesarios)
        {
            string[] partes = modelo.Split('-');
            List<CombinacionDTO> _listCombinacionColores = new List<CombinacionDTO>();
            if (partes.Length > 0)
            {
                DataTable dtColorMaterialOdoo = _combinacionColorDal.SelectColorMateriales(modelo, MascaraColor(correlativoColor));

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
                    if (_listCombinacionColores.Count != dtColores.Rows.Count)
                        _listCombinacionColores.Clear();
                }
                return _listCombinacionColores;
            }
            else {
                return new List<CombinacionDTO>();
            }      
        }

        private string MascaraColor(string correlativoColor)
        {
            string cadena = "C000";
            int largoId = correlativoColor.Trim().Length;
            return cadena.Substring(0, cadena.Length - largoId) + correlativoColor.ToString();
        }

        public bool CorregirColores(int _contrato)
        {
            return (_hojaCombinacionesDal.CorreccionColoresSiaa(_contrato).Rows.Count > 0);
        }

        public DataTable MaterialPorColor(string famili, int correlativo) {
            return _hojaCombinacionesDal.SelectMaterialUnSoloColor(famili, correlativo);
        }
    }
}
