using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Productscontroller : ControllerBase
    {
        private readonly StoreContext _context;
        public Productscontroller(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Products()
        {
            var products=await _context.products.ToListAsync();
            return Ok(products);
        }
    [HttpGet("{id}")]
    public async Task<Product> GetProduct(int id)
    {
        return await _context.products.FindAsync(id);
    }
    }
}