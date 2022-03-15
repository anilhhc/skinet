using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;

namespace API.Controllers
{
    //[ApiController]
    [Route("api/[controller]")]
    public class Productscontroller : ControllerBase
    {       
        
        private readonly IGenericRepository<ProductBrand> _productBrand;
        // public IGenericRepository<Product> _productsRepo { get; }
        // public IGenericRepository<ProductType> _productType { get; }
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductType> _productType;
        private readonly IMapper _mapper;

        public Productscontroller(IGenericRepository<Product> productsRepo,IGenericRepository<ProductBrand> productBrand,
        IGenericRepository<ProductType> productType,IMapper mapper)
        {
            _mapper = mapper;
            _productType = productType;
            _productsRepo = productsRepo;
            _productBrand = productBrand;         
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec=new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);//.ListAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products));
        }
         [HttpGet("{id}")]
        public async Task<ProductToReturnDto> GetProduct(int id)
        {
            var spec=new ProductsWithTypesAndBrandsSpecification(id);
            var product= await _productsRepo.GetEntityWithSpec(spec);//.GetByIdAsync(id);//_repo.GetProductByIdAsync(id);
            return _mapper.Map<Product,ProductToReturnDto>(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            var productBrands = await _productBrand.ListAllAsync();//_repo.GetProductBrandsAsync();
            return Ok(productBrands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<List<Product>>> GetProductTypes()
        {
            var productTypes = await _productType.ListAllAsync();//_repo.GetProductTypesAsync();
            return Ok(productTypes);            
        }
       
    }
}