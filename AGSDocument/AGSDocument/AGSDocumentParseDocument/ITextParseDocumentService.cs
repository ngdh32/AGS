using System;
using System.IO;
using System.Text;
using AGSDocumentCore.Interfaces.Services;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace AGSDocumentParseDocument
{
    public class ITextParseDocumentService : IParseDocumentService
    {
        public ITextParseDocumentService()
        {
        }

        public string PrasePDFFile(byte[] fileContent)
        {
            MemoryStream stream = new MemoryStream(fileContent);

            var pdfDocument = new PdfDocument(new PdfReader(stream));
            var strategy = new SimpleTextExtractionStrategy();
            StringBuilder processed = new StringBuilder();

            for (int i = 1; i <= pdfDocument.GetNumberOfPages(); ++i)
            {
                var page = pdfDocument.GetPage(i);
                string text = PdfTextExtractor.GetTextFromPage(page, strategy);
                processed.Append(text);
            }

            return processed.ToString();
        }
    }
}
