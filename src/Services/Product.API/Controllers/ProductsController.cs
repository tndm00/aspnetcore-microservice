﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.API.Entities;
using Product.API.Repositories.Interfaces;
using Shared.DTOs.Product;
using System.ComponentModel.DataAnnotations;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;


        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        #region CRUD

        [HttpGet("get-all")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            var result = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetAllProduct([Required] long id)
        {
            var product = await _productRepository.GetProductById(id);
            if(product == null) 
                return NotFound();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var product = _mapper.Map<CatalogProduct>(productDto);
            await _productRepository.CreateProduct(product);
            await _productRepository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateProduct([Required] long id, [FromBody] UpdateProductDto productDto)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
                return NotFound();

            var updateProduct = _mapper.Map(productDto, product);
            await _productRepository.UpdateProduct(updateProduct);
            await _productRepository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProduct([Required] long id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null)
                return NotFound();

            await _productRepository.DeleteProduct(id);
            await _productRepository.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Additional Resources

        [HttpGet("get-product-by-no/{productNo}")]
        public async Task<IActionResult> GetProductByNo([Required] string productNo)
        {
            var product = await _productRepository.GetProductByNo(productNo);
            if (product == null)
                return NotFound();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        #endregion
    }
}
