using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Npgsql;

namespace PSIAA.DataAccessLayer.TuartDB
{
    public class ModeloComponenteDAL
    {
        /// <summary>
        /// Variable de instancia a la clase Transactions (Conexión BD)
        /// </summary>
        public Transactions _trans = new Transactions();

        /// <summary>
        /// Ejecuta una consulta de selección a la base de datos para obtener todos los componentes de una prenda.
        /// </summary>
        /// <param name="modelo">Modelo de prenda</param>
        /// <returns>Contenedor de tipo DataTable con los datos de la consulta</returns>
        public DataTable SelectComponentesModelo(string modelo) {
            List<NpgsqlParameter> _sqlParam = new List<NpgsqlParameter>();

            string query = @"
                select 
                    mc.c_codcom, 
                    mc.c_codmod, 
                    c.c_dencom 
                from modelocomponente mc 
                inner join componente c 
                    on c.c_codcom = mc.c_codcom
                where 
                    mc.c_codmod = @modelo 
                    and c.i_est = 1
                limit 10";

            _sqlParam.Add(new NpgsqlParameter("@modelo", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = modelo });
            return _trans.ReadingQuery(query, _sqlParam);
        }
    }
}
