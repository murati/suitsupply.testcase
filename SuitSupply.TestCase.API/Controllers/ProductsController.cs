using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SuitSupply.TestCase.Business.Helpers;
using SuitSupply.TestCase.Data.Database;
using SuitSupply.TestCase.Data.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuitSupply.TestCase.API.Controllers
{
    [Route("api/[controller]"), EnableCors("AllowSpecificOrigin")]
    public class ProductsController : Controller
    {
        ProductHelper productHelper;
        IMapper mapper;
        public ProductsController(dbTestCase db, IMapper mapper, IMemoryCache cache)
        {
            productHelper = new ProductHelper(db, mapper, cache);
            this.mapper = mapper;
        }

        // GET: api/<controller>
        [HttpGet, Route("get")]
        public IActionResult Get()
        {
            IEnumerable<ProductDTO> products = productHelper.Get();
            return new JsonResult(productHelper.Get());
        }

        [HttpGet, Route("get/{name:alpha}")]
        public IActionResult Get(string name)
        {

            return new JsonResult(productHelper.Filter(name));
        }

        [HttpGet, Route("get/{id:int}")]
        public JsonResult Get(int id)
        {
            return new JsonResult(productHelper.GetById(id));
        }

        // POST api/<controller>
        [HttpPost, Route("create")]
        public void Post([FromForm]ProductDTO product)
        {
            productHelper.SaveForm(product);
        }

        // PUT api/<controller>/5
        [HttpPut, Route("update")]
        public void Put([FromForm]ProductDTO product)
        {
            productHelper.Update(product);
        }

        // DELETE api/<controller>/5
        [HttpDelete, Route("delete/{id}")]
        public void Delete(int id)
        {
            productHelper.Delete(id);
        }

        [HttpGet]
        [Route("export"), Route("export/{filter}")]
        public IActionResult Export(string filter = "")
        {
            byte[] fileContents = productHelper.ExportToExcel(filter);
            return new FileContentResult(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}
