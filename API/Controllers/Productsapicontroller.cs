using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace API.Controllers
{
    //[ApiController]
    [Route("api/[controller]")]
    public class Productscontroller : ControllerBase
    {
        public readonly IProductRepository _repo;

        public Productscontroller(IProductRepository repo)
        {
            _repo = repo;

        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Products()
        {
            var products = await _repo.GetProductsAsync();
            return Ok(products);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> ProductBrands()
        {
            var productBrands = await _repo.GetProductBrandsAsync();
            return Ok(productBrands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<List<Product>>> ProductTypes()
        {
            var productTypes = await _repo.GetProductTypesAsync();
            return Ok(productTypes);            
        }
        [HttpGet("{id}")]
        public async Task<Product> GetProduct(int id)
        {
            return await _repo.GetProductByIdAsync(id);
        }
    }
}