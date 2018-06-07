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

        public static string Mascara(int valor, string mascara)
        {
            int largoId = valor.ToString().Length;
            return mascara.Substring(0, mascara.Length - largoId) + valor.ToString();
        }
    }
}
