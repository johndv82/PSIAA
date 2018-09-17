using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PSIAA.DataAccessLayer.SAP.Reports
{
    public class PackingListDAL
    {
        private TransactionsSAP _trans = new TransactionsSAP();

        public DataTable SelectPackingListCabecera(int docKey) {
            List<SqlParameter> sqlParam = new List<SqlParameter>();
            string query = @"
                Declare @Talla nvarchar(10)
                declare @GrupoTalla nvarchar(1)

                declare @country nvarchar(100)
                set @country = (SELECT distinct T1.[Country]+' ' FROM [dbo].ODLN  T0 inner join CRD1 T1 on T0.[CardCode] = T1.[CardCode] 
                WHERE T0.docentry = @DocKey AND T1.[AdresType]  ='S' and T1.[Country] = (SELECT T0.[CountryS] FROM DLN12 T0 WHERE T0.[DocEntry] =@DocKey))

                declare @county nvarchar(100)
                set @county = (SELECT distinct T1.[County]+' ' FROM [dbo].ODLN  T0 inner join CRD1 T1 on T0.[CardCode] = T1.[CardCode] 
                WHERE T0.docentry = @DocKey AND T1.[AdresType]  ='S' and T1.[County] = (SELECT T0.[CountyS] FROM DLN12 T0 WHERE T0.[DocEntry] =@DocKey))

                declare @state nvarchar(10)
                set @state = (SELECT distinct T1.[State]+' ' FROM [dbo].ODLN  T0 inner join CRD1 T1 on T0.[CardCode] = T1.[CardCode] 
                WHERE T0.docentry = @DocKey AND T1.[AdresType]  ='S' and T1.[State] = (SELECT T0.[StateS] FROM DLN12 T0 WHERE T0.[DocEntry] =@DocKey))

                declare @direccion nvarchar(500)
                declare @Street nvarchar(200)
                set @Street = (SELECT distinct T1.Street+' ' FROM [dbo].ODLN  T0 inner join CRD1 T1 on T0.[CardCode] = T1.[CardCode] 
                WHERE T0.docentry = @DocKey AND T1.[AdresType]  ='S' and T1.Street = (SELECT T0.StreetS FROM DLN12 T0 WHERE T0.[DocEntry] =@DocKey))

                declare @block nvarchar(100)
                set @block = (SELECT distinct T1.[Block]+' ' FROM [dbo].ODLN  T0 inner join CRD1 T1 on T0.[CardCode] = T1.[CardCode] 
                WHERE T0.docentry = @DocKey AND T1.[AdresType]  ='S' and T1.[Block] = (SELECT T0.[BlockB] FROM DLN12 T0 WHERE T0.[DocEntry] =@DocKey))

                declare @city nvarchar(100)
                set @city = (SELECT distinct T1.[City]+' ' FROM [dbo].ODLN  T0 inner join CRD1 T1 on T0.[CardCode] = T1.[CardCode] 
                WHERE T0.docentry = @DocKey AND T1.[AdresType]  ='S' and T1.[City] = (SELECT T0.[CityS] FROM DLN12 T0 WHERE T0.[DocEntry] =@DocKey))

                declare @zipcode nvarchar(20)

                set @zipcode = (SELECT distinct T1.[ZipCode]+' ' FROM [dbo].ODLN  T0 inner join CRD1 T1 on T0.[CardCode] = T1.[CardCode] 
                WHERE T0.docentry = @DocKey AND T1.[AdresType]  ='S' and T1.[ZipCode] = (SELECT T0.[ZipCodeS] FROM DLN12 T0 WHERE T0.[DocEntry] =@DocKey))

                if (@Street is null)
                begin
	                set @Street= ('')
                end
                if (@block is null)
                begin
	                set @block= ('')
                end
                if (@city is null)
                begin
	                set @city= ('')
                end
                if (@zipcode is null)
                begin
	                set @zipcode= ('')
                end
                if (@county is null)
                begin
	                set @county= ('')
                end

                set @direccion = ( @block + @city + @zipcode + @county  + @state)

                select 
	                T0.U_SYP_PLNUM as NotaContenido, 
	                (select count(U_SYP_CAJA) from DLN7 where docentry = @DocKey) as TotalCajas,
	                T0.TaxDate as Fecha,
	                @direccion as Direccion,
	                T0.U_SYP_CONTRATO as Contrato,
	                T0.U_SYP_POCLIENTE as Po,
	                T0.CardName as Cliente,
	                (select distinct StreetS from DLN12 where docentry = @DocKey) as Calle,
	                (CASE WHEN @state is null THEN (select distinct CountryB from DLN12 where docentry = @DocKey) ELSE @state END) + ' - ' + 
	                (CASE WHEN @state is null THEN (select distinct CountryB from DLN12 where docentry = @DocKey) ELSE (select Name from OCST where Code = @state) END) as Destino,
	                'TotalPesoNeto' = (SELECT SUM(CAST(U_SYP_PNETO AS numeric(19, 6))) FROM dbo.DLN7 WHERE  (DocEntry = @DocKey)),
	                'TotalPesoBruto' = (SELECT SUM(CAST(U_SYP_PBRUTO AS numeric(19, 6))) FROM dbo.DLN7 WHERE  (DocEntry = @DocKey)),
	                'TotalCantidad' = (SELECT SUM(quantity) FROM dbo.DLN8 WHERE  (DocEntry = @DocKey))
                from ODLN T0 
                WHERE T0.DocEntry = @DocKey ";
            sqlParam.Add(new SqlParameter("@DocKey", SqlDbType.Int) { Value = docKey });
            return _trans.ReadingQuery(query, sqlParam);
        }

        public DataTable SelectPackingListDetalle(int docKey) {
            List<SqlParameter> sqlParam = new List<SqlParameter>();
            string query = @"
                select 
	                base.ItemCode,
	                base.TamañoCaja,
	                base.Paquete,
	                base.Caja,
	                base.PesoNeto,
	                base.CajaPesoNeto,
	                base.PesoBruto,
	                base.CajaPesoBruto,
	                base.CantidadPaquete,
	                base.CutTicket,
	                base.Modelo,
	                base.CodCliModelo,
	                base.CliModelo,
	                base.Color,
	                Tallas.Talla0, Tallas.Talla1, Tallas.Talla2, 
	                Tallas.Talla3, Tallas.Talla4, Tallas.Talla5, 
	                Tallas.Talla6, Tallas.Talla7, Tallas.Talla8,
	                case when base.Talla = Tallas.Talla0 then base.Cantidad end as Cantidad0,
	                case when base.Talla = Tallas.Talla1 then base.Cantidad end as Cantidad1,
	                case when base.Talla = Tallas.Talla2 then base.Cantidad end as Cantidad2,
	                case when base.Talla = Tallas.Talla3 then base.Cantidad end as Cantidad3,
	                case when base.Talla = Tallas.Talla4 then base.Cantidad end as Cantidad4,
	                case when base.Talla = Tallas.Talla5 then base.Cantidad end as Cantidad5,
	                case when base.Talla = Tallas.Talla6 then base.Cantidad end as Cantidad6,
	                case when base.Talla = Tallas.Talla7 then base.Cantidad end as Cantidad7,
	                case when base.Talla = Tallas.Talla8 then base.Cantidad end as Cantidad8,
	                base.Descripcion,
	                base.TiendaCli,
	                base.GrupoTalla,
	                base.CodigoTalla
                from(
                select
	                T2.ItemCode,
	                (select distinct PackageTyp from DLN7 where docentry = @DocKey) as TamañoCaja,
	                'Paquete'=T2.PackageNum,
	                'Caja'= (SELECT U_SYP_CAJA  from DLN7 where docentry = @DocKey and PackageNum=T2.PackageNum),
	                'PesoNeto'= (SELECT U_SYP_PNETO  from DLN7 where docentry = @DocKey and PackageNum=T2.PackageNum),
	                'CajaPesoNeto' = (SELECT SUM(CAST(U_SYP_PNETO AS numeric(19, 6))) FROM DLN7 WHERE (DocEntry = @DocKey) AND U_SYP_CAJA=(SELECT U_SYP_CAJA  from DLN7 where docentry = @DocKey and PackageNum=T2.PackageNum)),
	                'PesoBruto'= (SELECT U_SYP_PBRUTO  from DLN7 where docentry = @DocKey and PackageNum=T2.PackageNum),
	                'CajaPesoBruto' = (SELECT SUM(CAST(U_SYP_PBRUTO AS numeric(19, 6))) FROM DLN7 WHERE (DocEntry = @DocKey) AND U_SYP_CAJA = (SELECT U_SYP_CAJA  from DLN7 where docentry = @DocKey and PackageNum=T2.PackageNum) ),
	                'CantidadPaquete' = (SELECT SUM(dbo.DLN8.Quantity) AS Expr1 FROM dbo.DLN8 INNER JOIN dbo.DLN7 ON (dbo.DLN8.DocEntry = dbo.DLN7.DocEntry  AND dbo.DLN8.PackageNum = dbo.DLN7.PackageNum) WHERE (dbo.DLN8.DocEntry = @DocKey) AND (dbo.DLN8.PackageNum = T2.PackageNum)),
	                'CutTicket'=(SELECT U_SYP_CUTTICKET FROM dbo.DLN1 WHERE (DocEntry = @DocKey) AND (ItemCode = T2.ItemCode) Group by U_SYP_CUTTICKET),
	                'Modelo' = (SELECT U_SYP_PT_MODELO FROM  dbo.DLN1 INNER JOIN dbo.OITM ON dbo.DLN1.ItemCode = dbo.OITM.ItemCode WHERE (dbo.DLN1.DocEntry = @DocKey) AND (dbo.DLN1.ItemCode = T2.ItemCode) Group by U_SYP_PT_MODELO),
	                'CodCliModelo' = (SELECT CASE WHEN dbo.OCRD.GroupCode='102' THEN [dbo].[@SYP_PT_MODELO].U_SYP_CL_COMOD ELSE [dbo].[@SYP_PT_MODELO].U_SYP_AA_COMOD END FROM  dbo.DLN1 INNER JOIN dbo.OITM ON dbo.DLN1.ItemCode = dbo.OITM.ItemCode INNER JOIN [dbo].[@SYP_PT_MODELO] ON [dbo].[@SYP_PT_MODELO].U_SYP_AA_COMOD= dbo.OITM.U_SYP_PT_MODELO INNER JOIN dbo.OCRD ON dbo.OCRD.CardCode = T0.CardCode where (dbo.DLN1.DocEntry = @DocKey) AND (dbo.DLN1.ItemCode = T2.ItemCode) Group by [dbo].[@SYP_PT_MODELO].U_SYP_AA_COMOD, [dbo].[@SYP_PT_MODELO].U_SYP_CL_COMOD, dbo.OCRD.GroupCode),
	                'CliModelo' = (SELECT T0.[U_SYP_CL_DESCMOD] FROM [dbo].[@SYP_PT_MODELO]  T0 WHERE T0.[U_SYP_AA_COMOD] = (SELECT   U_SYP_PT_MODELO FROM  dbo.DLN1 INNER JOIN dbo.OITM ON dbo.DLN1.ItemCode = dbo.OITM.ItemCode WHERE (dbo.DLN1.DocEntry = @DocKey) AND (dbo.DLN1.ItemCode = T2.ItemCode) Group by U_SYP_PT_MODELO)),
	                'Color' = (SELECT U_SYP_COLCLIENTE FROM dbo.DLN1 WHERE (DocEntry = @DocKey) AND (ItemCode = T2.ItemCode) Group by U_SYP_COLCLIENTE),
	                'Talla' = (SELECT U_SYP_PT_TALLA FROM dbo.DLN1 WHERE (DocEntry = @DocKey) AND (ItemCode = T2.ItemCode) Group by U_SYP_PT_TALLA), 
	                'Cantidad'=T2.Quantity, 
	                'Descripcion' = (SELECT U_SYP_MOCOLOR FROM dbo.DLN1 WHERE (DocEntry = @DocKey) AND (ItemCode = T2.ItemCode)group by U_SYP_MOCOLOR),
	                'TiendaCli'=(SELECT U_SYP_TIENDA  from DLN7 where docentry = @DocKey and PackageNum=T2.PackageNum),
	                'GrupoTalla' = (SELECT   U_SYP_PT_GRUTALL from oitm where ItemCode= T2.ItemCode ),
	                'CodigoTalla' = (SELECT   TA.Name from [@SYP_PT_TALLAS] TA where U_SYP_PT_TALLA=(SELECT U_SYP_PT_TALLA FROM dbo.DLN1 WHERE (DocEntry = @DocKey) AND (ItemCode = T2.ItemCode) Group by U_SYP_PT_TALLA) and  U_SYP_PT_CODGRUPOTAL= (SELECT   U_SYP_PT_GRUTALL from oitm where ItemCode= T2.ItemCode ))
                FROM ODLN T0 
                INNER JOIN DLN12 T3 ON (T0.docEntry= T3.DocEntry )      
                INNER JOIN DLN8 T2 ON (T0.DocEntry = T2.DocEntry )
                WHERE (T2.DocEntry = @DocKey) 
                ) as base
                inner join (
	                select *
	                from(
		                SELECT 
			                DocEntry,
			                U_SYP_PT_TALLA as Talla, 
			                'Talla'+convert(varchar(10), ROW_NUMBER() OVER(PARTITION BY U_SYP_SALIDA_CLIENTE ORDER BY LineNum ASC) - 1) as Nombre,
			                U_SYP_SALIDA_CLIENTE as modeloGrupo
		                FROM dbo.DLN1 WHERE DocEntry = @DocKey 
	                ) as data
	                pivot(max(Talla) for Nombre in ([Talla0], [Talla1], [Talla2], [Talla3], [Talla4], [Talla5], [Talla6], [Talla7], [Talla8])) piv
                ) as Tallas on  Tallas.DocEntry = @DocKey and Tallas.modeloGrupo = REPLACE(base.CodCliModelo, ' ', '')
                order by base.Paquete";

            sqlParam.Add(new SqlParameter("@DocKey", SqlDbType.Int) { Value = docKey });
            return _trans.ReadingQuery(query, sqlParam);
        }
    }
}
