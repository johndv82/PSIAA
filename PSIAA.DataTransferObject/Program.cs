using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace PSIAA.DataTransferObject
{
    public class Rate
    {
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }

        public static void met()
        {
            
        }

        /*public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int page, int pageSize)
        {
            return source.Take((page - 1) * pageSize).Skip(pageSize);
        }*/


    }
}
