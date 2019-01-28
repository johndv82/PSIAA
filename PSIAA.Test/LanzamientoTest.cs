using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PSIAA.DataTransferObject;
using PSIAA.BusinessLogicLayer;
using System.Collections.Generic;
using System.Linq;

namespace PSIAA.Test
{
    [TestClass]
    public class LanzamientoTest
    {
        [TestMethod]
        public void CalculoKilosNecesariosTest()
        {
            List<decimal> listKilos = new List<decimal>();
            ContratoBLL _contratoBll = new ContratoBLL();
            LanzamientoBLL _lanzamientoBll = new LanzamientoBLL();
            List<ContratoDetalleDTO> _listContratoDet = _contratoBll.ListarDetalleContrato(18321, true);
            _listContratoDet = (from cont in _listContratoDet.AsEnumerable()
                               where cont.ModeloAA.Trim().Equals("C471-102")
                               select cont).ToList();

            foreach (var contrato in _listContratoDet) {
                listKilos.Add(_lanzamientoBll.CalcularKilosPorContrato(contrato));
            }
        }

        [TestMethod]
        public void CalculoPesosBaseTest() {
            List<Dictionary<string, decimal>> dicpesos = new List<Dictionary<string, decimal>>();
            ContratoBLL _contratoBll = new ContratoBLL();
            LanzamientoBLL _lanzamientoBll = new LanzamientoBLL();
            List<ContratoDetalleDTO> _listContratoDet = _contratoBll.ListarDetalleContrato(18321, true);
            _listContratoDet = (from cont in _listContratoDet.AsEnumerable()
                                where cont.ModeloAA.Trim().Equals("C471-102")
                                select cont).ToList();

            foreach (var contrato in _listContratoDet)
            {
                dicpesos.Add(_lanzamientoBll.CalcularPesosBasePorContratoTalla(contrato));
            }
        }
    }
}
