using SuitSupply.TestCase.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuitSupply.TestCase.Business.Implementations
{
    public interface IProductHelper
    {
        string UploadImage(ProductDTO product);
        bool SaveForm(ProductDTO product);
        IEnumerable<ProductDTO> Get();
        IEnumerable<ProductDTO> Filter(string name);
        bool Delete(int id);
        byte[] ExportToExcel(string filter);
    }
}
