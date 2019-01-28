using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PSIAA.BusinessLogicLayer
{
    public static class Helper
    {
        /// <summary>
        /// Convierte una Lista Genérica a un contenedor de tipo DataTable.
        /// </summary>
        /// <typeparam name="T">Parametro de tipo genérico</typeparam>
        /// <param name="items">Lista de items de tipo genérico a convertir</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }
            return tb;
        }

        /// <summary>
        /// Crea una mascara de valores segun la cantidad de caracteres que contenga. Ejemplo:
        /// Valor: 1234
        /// Mascara: 000000
        /// Resultado:001234
        /// </summary>
        /// <param name="valor">Valor de mascara</param>
        /// <param name="mascara">Mascara que cubre al valor</param>
        /// <returns>Variable de tipo string con el resultado de la máscara.</returns>
        public static string Mascara(int valor, string mascara)
        {
            int largoId = valor.ToString().Trim().Length;
            return mascara.Substring(0, mascara.Length - largoId) + valor.ToString();
        }
    }
}
