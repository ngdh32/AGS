using System;
using AGSDocumentCore.Models.Enums;

namespace AGSDocumentCore.Interfaces.Services
{
    public interface IParseDocumentService
    {
        public string PrasePDFFile(byte[] fileContent, FileExtensionType fileExtensionType);
    }
}
