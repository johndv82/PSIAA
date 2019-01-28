using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using NpgsqlTypes;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer
{
    public class HojaCombinacionesDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD).
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener los porcentajes, material, y titulos de cada color combinado.
        /// </summary>
        /// <param name="familia">Grupo Familiar del Modelo</param>
        /// <param name="correlativo">Correlativo del Modelo</param>
        /// <param name="combinacion">Código de Combinación</param>
        /// <returns>Contenedor de tipo DataTable con el resultado de la consulta.</returns>
        public DataTable SelectColoresPorcentajes(string familia, int correlativo, short combinacion) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                (Col.Famili + '-' + CONVERT(varchar, Col.Correlativo)) as Modelo, 
	                Col.Colores, 
	                Por.Porcentajes,
	                Mat.Materiales,
	                Tit.Titulos
                from (
	                select 
		                Famili, 
		                Correlativo, 
		                Colores,
		                case when CHARINDEX('_', Nombre) > 0 then Nombre 
			                else 'Color_12' 
		                end as Nombre
	                from Hoja_de_Combinaciones_2 
	                unpivot(
	                Colores for Nombre in(Color_1, Color_2, Color_3, Color_4, Color_5, 
						                Color_6, Color_7, Color_8, Color_9, Color_10,
						                Color_11, Color, Color_13, Color_14, Color_15)
	                ) as up
	                where Famili = @familia and Correlativo = @correlativo and Item = @item and Colores != ''
                ) Col inner join
                (
	                select 
		                Famili, 
		                Correlativo, 
		                Porcentajes, NombrePor
	                from Hoja_de_Combinaciones_porc_2
	                unpivot(
	                Porcentajes for NombrePor in(porc_Color_1, porc_Color_2, porc_Color_3, porc_Color_4, porc_Color_5, 
							                porc_Color_6, porc_Color_7, porc_Color_8, porc_Color_9, porc_Color_10,
							                porc_Color_11, porc_Color_12, porc_Color_13, porc_Color_14, porc_Color_15)
	                ) as up
	                where Famili = @familia and Correlativo = @correlativo and Porcentajes != 0
                ) Por on Por.Famili = Col.Famili and Por.Correlativo = Col.Correlativo 
                    and 
	                Col.Nombre = 
	                SUBSTRING(Por.NombrePor, CHARINDEX('_', Por.NombrePor)+1, len(Por.NombrePor) - CHARINDEX('_', Por.NombrePor))
                inner join(
	                select 
		                Famili, 
		                Correlativo, 
		                Materiales, NombreMat
	                from Hoja_de_Combinaciones_porc_2
	                unpivot(
	                Materiales for NombreMat in(Material_1, Material_2, Material_3, Material_4, Material_5, 
							                Material_6, Material_7, Material_8, Material_9, Material_10,
							                Material_11, Material_12, Material_13, Material_14, Material_15)
	                ) as up
	                where Famili = @familia and Correlativo = @correlativo and Materiales != ''
                ) Mat on Mat.Famili = Col.Famili and Mat.Correlativo = Col.Correlativo 
                    and 
		                SUBSTRING(Por.NombrePor, CHARINDEX('r_', Por.NombrePor)+2, len(Por.NombrePor) - CHARINDEX('r_', Por.NombrePor)) = 
		                SUBSTRING(Mat.NombreMat, CHARINDEX('_', Mat.NombreMat)+1, len(Mat.NombreMat) - CHARINDEX('_', Mat.NombreMat))
                inner join(
	                select 
		                Famili, 
		                Correlativo, 
		                Titulos, NombreTit
	                from Hoja_de_Combinaciones_porc_2
	                unpivot(
	                Titulos for NombreTit in(Titulo_1, Titulo_2, Titulo_3, Titulo_4, Titulo_5,
							                Titulo_6, Titulo_7, Titulo_8, Titulo_9, Titulo_10,
							                Titulo_11, Titulo_12, Titulo_13, Titulo_14, Titulo_15)
	                ) as up
	                where Famili = @familia and Correlativo = @correlativo
                ) Tit on Tit.Famili = Col.Famili and Tit.Correlativo = Col.Correlativo 
                    and 
		                SUBSTRING(Por.NombrePor, CHARINDEX('r_', Por.NombrePor)+2, len(Por.NombrePor) - CHARINDEX('r_', Por.NombrePor)) = 
		                SUBSTRING(Tit.NombreTit, CHARINDEX('_', Tit.NombreTit)+1, len(Tit.NombreTit) - CHARINDEX('_', Tit.NombreTit))";

            _sqlParam.Add(new SqlParameter("@familia", SqlDbType.VarChar) { Value = familia });
            _sqlParam.Add(new SqlParameter("@correlativo", SqlDbType.Int) { Value = correlativo });
            _sqlParam.Add(new SqlParameter("@item", SqlDbType.SmallInt) { Value = combinacion });

            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener el material, titulo y color de un modelo.
        /// </summary>
        /// <param name="familia">Grupo Familiar del Modelo</param>
        /// <param name="correlativo">Correlativo del Modelo</param>
        /// <returns>Contenedor de datos de tipo DataTable con el resultado de la consulta.</returns>
        public DataTable SelectMaterialUnSoloColor(string familia, int correlativo) {
            List<SqlParameter> _sqlParam = new List<SqlParameter>();

            string query = @"
                select 
	                Mat.Famili,
	                Mat.Correlativo,
	                Mat.Materiales,
	                Tit.Titulos
                from(
                select 
	                Famili, 
	                Correlativo, 
	                Materiales, NombreMat
                from Hoja_de_Combinaciones_porc_2
                unpivot(
                Materiales for NombreMat in(Material_1, Material_2, Material_3, Material_4, Material_5, 
						                Material_6, Material_7, Material_8, Material_9, Material_10,
						                Material_11, Material_12, Material_13, Material_14, Material_15)
                ) as up
                where Famili = @familia and Correlativo = @correlativo and Materiales != ''
                ) as Mat
                inner join(
	                select 
		                Famili, 
		                Correlativo, 
		                Titulos, NombreTit
	                from Hoja_de_Combinaciones_porc_2
	                unpivot(
	                Titulos for NombreTit in(Titulo_1, Titulo_2, Titulo_3, Titulo_4, Titulo_5,
							                Titulo_6, Titulo_7, Titulo_8, Titulo_9, Titulo_10,
							                Titulo_11, Titulo_12, Titulo_13, Titulo_14, Titulo_15)
	                ) as up
	                where Famili = @familia and Correlativo = @correlativo
                ) Tit on Tit.Famili = Mat.Famili and Tit.Correlativo = Mat.Correlativo 
                    and 
		                SUBSTRING(Mat.NombreMat, CHARINDEX('_', Mat.NombreMat)+1, len(Mat.NombreMat) - CHARINDEX('_', Mat.NombreMat)) = 
		                SUBSTRING(Tit.NombreTit, CHARINDEX('_', Tit.NombreTit)+1, len(Tit.NombreTit) - CHARINDEX('_', Tit.NombreTit))";

            _sqlParam.Add(new SqlParameter("@familia", SqlDbType.VarChar) { Value = familia });
            _sqlParam.Add(new SqlParameter("@correlativo", SqlDbType.Int) { Value = correlativo });
            return _trans.ReadingQuery(query, _sqlParam);
        }

        /// <summary>
        /// Ejecuta un procedimiento almacenado en la BD, para la corrección de colores SIAA.
        /// </summary>
        /// <param name="contrato">Número de Contrato</param>
        /// <returns>Contenedor de tipo DataTable con los colores corregidos.</returns>
        public DataTable CorreccionColoresSiaa(int contrato) {
            List<SqlParameter> _procedureParam = new List<SqlParameter>();
            _procedureParam.Add(new SqlParameter("@contrato", SqlDbType.Int) { Value = contrato });
            return _trans.ReadingProcedure("dbo.ITSM_CORRECCIONCOLORES", _procedureParam);
        }
    }
}