using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PSIAA.Presentation.Helpers
{
    public static class PdfMerger
    {
        public static void MergeFiles(List<string> sourceFiles, string rutaPdfJoin, bool addCopy)
        {
            Document document = new Document();
            using (MemoryStream ms = new MemoryStream())
            {
                FileStream fl = new FileStream(rutaPdfJoin, FileMode.Create, FileAccess.Write, FileShare.None);
                PdfCopy copy = new PdfCopy(document, fl);
                document.Open();

                // Iterar pdfs
                for (int fileCounter = 0; fileCounter < sourceFiles.Count; fileCounter++)
                {
                    PdfReader reader = new PdfReader(sourceFiles[fileCounter]);
                    int numberOfPages = reader.NumberOfPages;

                    // Iterar paginas
                    for (int currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                    {
                        PdfImportedPage importedPage = copy.GetImportedPage(reader, currentPageIndex);
                        PdfCopy.PageStamp pageStamp = copy.CreatePageStamp(importedPage);

                        pageStamp.AlterContents();
                        copy.AddPage(importedPage);
                        if (addCopy) {
                            copy.AddPage(importedPage);
                        }
                    }
                    copy.FreeReader(reader);
                    reader.Close();
                }
                document.Close();
            }
        }
    }
}