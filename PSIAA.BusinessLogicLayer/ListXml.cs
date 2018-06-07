using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSIAA.DataTransferObject;
using System.Xml.Linq;
using System.IO;

namespace PSIAA.BusinessLogicLayer
{
    public class ListXml
    {
        public void AgregarAlmacenToXML(AlmacenDTO alm, string _user)
        {
            XDocument miXML = new XDocument();
            XElement nuevoElemento = new XElement("AlmacenDTO",
                                               new XElement("CodAlmacen", alm.CodAlmacen),
                                               new XElement("TipoMovimiento", alm.TipoMovimiento),
                                               new XElement("IngresoSalida", alm.IngresoSalida),
                                               new XElement("AlmacenOrigenDestino", alm.AlmacenOrigenDestino),
                                               new XElement("CodProducto", alm.CodProducto),
                                               new XElement("Orden", alm.Orden),
                                               new XElement("NroLote", alm.NroLote),
                                               new XElement("Contrato", alm.Contrato),
                                               new XElement("Tallas",
                                                    new XElement("Talla1", alm.Tallas[0]),
                                                    new XElement("Talla2", alm.Tallas[1]),
                                                    new XElement("Talla3", alm.Tallas[2]),
                                                    new XElement("Talla4", alm.Tallas[3]),
                                                    new XElement("Talla5", alm.Tallas[4]),
                                                    new XElement("Talla6", alm.Tallas[5]),
                                                    new XElement("Talla7", alm.Tallas[6])),
                                               new XElement("Cantidad", alm.Cantidad),
                                               new XElement("PesoBruto", alm.PesoBruto),
                                               new XElement("PesoNeto", alm.PesoNeto)
                                               );

            string _ruta = @"C:\inetpub\wwwroot\PSIAA\ListadoAlmacenDTO_" + _user + ".xml";
            if (File.Exists(_ruta))
            {
                miXML = new XDocument();
                miXML = XDocument.Load(_ruta);
                miXML.Root.Add(nuevoElemento);
            }
            else {
                miXML = new XDocument(
                            new XDeclaration("1.0", "utf-8", null),
                                new XElement("ListadoAlmacenDTO", nuevoElemento));
            }
            miXML.Save(_ruta);
        }

        public void AgregarRecepcionControlToXML(RecepcionControlDTO rec, string _user)
        {
            XDocument miXML = new XDocument();
            XElement nuevoElemento = new XElement("RecepcionControlDTO",
                                               new XElement("Almacen", rec.Almacen),
                                               new XElement("Orden", rec.Orden),
                                               new XElement("Lote", rec.Lote),
                                               new XElement("Tallas",
                                                    new XElement("Talla1", rec.Tallas[0]),
                                                    new XElement("Talla2", rec.Tallas[1]),
                                                    new XElement("Talla3", rec.Tallas[2]),
                                                    new XElement("Talla4", rec.Tallas[3]),
                                                    new XElement("Talla5", rec.Tallas[4]),
                                                    new XElement("Talla6", rec.Tallas[5]),
                                                    new XElement("Talla7", rec.Tallas[6]),
                                                    new XElement("Talla8", rec.Tallas[7]),
                                                    new XElement("Talla9", rec.Tallas[8])),
                                               new XElement("Piezas",
                                                    new XElement("Piezas1", rec.Piezas[0]),
                                                    new XElement("Piezas2", rec.Piezas[1]),
                                                    new XElement("Piezas3", rec.Piezas[2]),
                                                    new XElement("Piezas4", rec.Piezas[3]),
                                                    new XElement("Piezas5", rec.Piezas[4]),
                                                    new XElement("Piezas6", rec.Piezas[5]),
                                                    new XElement("Piezas7", rec.Piezas[6]),
                                                    new XElement("Piezas8", rec.Piezas[7]),
                                                    new XElement("Piezas9", rec.Piezas[8])),
                                               new XElement("Completo", rec.Completo),
                                               new XElement("Peso", rec.Peso),
                                               new XElement("Usuario", rec.Usuario));


            string _ruta = @"C:\inetpub\wwwroot\PSIAA\ListadoRecepcionControlDTO_" + _user + ".xml";


            if (File.Exists(_ruta))
            {
                miXML = new XDocument();
                miXML = XDocument.Load(_ruta);
                miXML.Root.Add(nuevoElemento);
            } else {
                miXML = new XDocument(
                            new XDeclaration("1.0", "utf-8", null),
                                new XElement("ListadoRecepcionControlDTO", nuevoElemento));
            }
            miXML.Save(_ruta);
        }

        public List<AlmacenDTO> ConvertXmlToListAlmacen(string _user)
        {
            List<AlmacenDTO> items = new List<AlmacenDTO>();
            string _ruta = @"C:\inetpub\wwwroot\PSIAA\ListadoAlmacenDTO_" + _user + ".xml";

            try
            {
                XDocument doc = XDocument.Load(_ruta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("AlmacenDTO")
                             select new AlmacenDTO
                             {
                                 CodAlmacen = (int)r.Element("CodAlmacen"),
                                 TipoMovimiento = (string)r.Element("TipoMovimiento"),
                                 IngresoSalida = (int)r.Element("IngresoSalida"),
                                 AlmacenOrigenDestino = (int)r.Element("AlmacenOrigenDestino"),
                                 CodProducto = (string)r.Element("CodProducto"),
                                 Orden = (string)r.Element("Orden"),
                                 NroLote = (string)r.Element("NroLote"),
                                 Contrato = (string)r.Element("Contrato"),
                                 Tallas = ElementToArrayInt7(r.Element("Tallas")),
                                 Cantidad = (int)r.Element("Cantidad"),
                                 PesoBruto = (int)r.Element("PesoBruto"),
                                 PesoNeto = (int)r.Element("PesoNeto")
                             }).ToList();
                }
                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<AlmacenDTO>();
            }
        }

        public List<RecepcionControlDTO> ConvertXmlToListRecepcionControl(string _user)
        {
            List<RecepcionControlDTO> items = new List<RecepcionControlDTO>();
            string _ruta = @"C:\inetpub\wwwroot\PSIAA\ListadoRecepcionControlDTO_" + _user + ".xml";

            try
            {
                XDocument doc = XDocument.Load(_ruta);
                if (doc.Root != null)
                {
                    items = (from r in doc.Root.Elements("RecepcionControlDTO")
                             select new RecepcionControlDTO
                             {
                                 Almacen = (int)r.Element("Almacen"),
                                 Orden = (string)r.Element("Orden"),
                                 Lote = (int)r.Element("Lote"),
                                 Tallas = ElementToArrayString9(r.Element("Tallas")),
                                 Piezas = ElementToArrayInt9(r.Element("Piezas")),
                                 Completo = Convert.ToChar((string)r.Element("Completo")),
                                 Peso = (decimal)r.Element("Peso"),
                                 Usuario = (string)r.Element("Usuario")
                             }).ToList();
                }
                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<RecepcionControlDTO>();
            }
        }

        private int[] ElementToArrayInt7(XContainer _container)
        {
            int[] _tallas = new int[7];
            _tallas[0] = (int)_container.Element("Talla1");
            _tallas[1] = (int)_container.Element("Talla2");
            _tallas[2] = (int)_container.Element("Talla3");
            _tallas[3] = (int)_container.Element("Talla4");
            _tallas[4] = (int)_container.Element("Talla5");
            _tallas[5] = (int)_container.Element("Talla6");
            _tallas[6] = (int)_container.Element("Talla7");
            return _tallas;
        }

        private string[] ElementToArrayString9(XContainer _container)
        {
            string[] _tallas = new string[9];
            _tallas[0] = (string)_container.Element("Talla1");
            _tallas[1] = (string)_container.Element("Talla2");
            _tallas[2] = (string)_container.Element("Talla3");
            _tallas[3] = (string)_container.Element("Talla4");
            _tallas[4] = (string)_container.Element("Talla5");
            _tallas[5] = (string)_container.Element("Talla6");
            _tallas[6] = (string)_container.Element("Talla7");
            _tallas[7] = (string)_container.Element("Talla8");
            _tallas[8] = (string)_container.Element("Talla9");
            return _tallas;
        }

        private int[] ElementToArrayInt9(XContainer _container)
        {
            int[] _piezas = new int[9];
            _piezas[0] = (int)_container.Element("Piezas1");
            _piezas[1] = (int)_container.Element("Piezas2");
            _piezas[2] = (int)_container.Element("Piezas3");
            _piezas[3] = (int)_container.Element("Piezas4");
            _piezas[4] = (int)_container.Element("Piezas5");
            _piezas[5] = (int)_container.Element("Piezas6");
            _piezas[6] = (int)_container.Element("Piezas7");
            _piezas[7] = (int)_container.Element("Piezas8");
            _piezas[8] = (int)_container.Element("Piezas9");
            return _piezas;
        }

        public void LimpiarArchivosXml(string _user)
        {
            //LIMPIAR XMLS
            string _rutaAlmacen = @"C:\inetpub\wwwroot\PSIAA\ListadoAlmacenDTO_" + _user + ".xml";
            if (File.Exists(_rutaAlmacen))
            {
                File.Delete(_rutaAlmacen);
            }

            string _rutaRecepcion = @"C:\inetpub\wwwroot\PSIAA\ListadoRecepcionControlDTO_" + _user + ".xml";
            if (File.Exists(_rutaRecepcion))
            {
                File.Delete(_rutaRecepcion);
            }
        }

        public void EliminarXmlAlmacen(string _orden, string _lote, string _user) {
            string _rutaAlmacen = @"C:\inetpub\wwwroot\PSIAA\ListadoAlmacenDTO_" + _user + ".xml";

            XDocument xmlAlmacen= XDocument.Load(_rutaAlmacen);

            var consul = from almacen in xmlAlmacen.Elements("ListadoAlmacenDTO").Elements("AlmacenDTO")
                         where almacen.Element("Orden").Value == _orden
                         & almacen.Element("NroLote").Value == _lote
                         select almacen;

            consul.ToList().ForEach(x => x.Remove());
            xmlAlmacen.Save(_rutaAlmacen);
        }

        public void EliminarXmlRecepcion(string _orden, int _lote, string _user) {
            string _rutaRecepcion = @"C:\inetpub\wwwroot\PSIAA\ListadoRecepcionControlDTO_" + _user + ".xml";

            XDocument xmlRecepcion = XDocument.Load(_rutaRecepcion);

            var consul = from recep in xmlRecepcion.Elements("ListadoRecepcionControlDTO").Elements("RecepcionControlDTO")
                         where recep.Element("Orden").Value == _orden
                         & int.Parse(recep.Element("Lote").Value) == _lote
                         select recep;

            consul.ToList().ForEach(x => x.Remove());
            xmlRecepcion.Save(_rutaRecepcion);
        }
    }
}
