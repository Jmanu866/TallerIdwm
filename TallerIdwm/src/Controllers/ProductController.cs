using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TallerIdwm.src.models;
using TallerIdwm.src.data;
using Microsoft.EntityFrameworkCore;
using TallerIdwm.src.helpers;
using TallerIdwm.src.dtos;
using TallerIdwm.src.mappers;
using TallerIdwm.src.interfaces;
using TallerIdwm.src.services;



namespace TallerIdwm.src.controllers
{


    public class ProductController(ILogger<ProductController> logger, UnitOfWork unitOfWork, IPhotoService photoService) : BaseController
    {
        private readonly ILogger<ProductController> _logger = logger;
        private readonly UnitOfWork _context = unitOfWork;
        private readonly IPhotoService _photoService = photoService;


        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {

            var products = await _context.ProductRepository.GetProductsAsync();
            return Ok(products);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.ProductRepository.GetProductByIdAsync(id);
            return product == null ? (ActionResult<Product>)NotFound() : (ActionResult<Product>)Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            await _context.ProductRepository.AddProductAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<ApiResponse<Product>>> Create([FromForm] ProductDto dto)
        {
            var urls = new List<string>();
            string? publicId = null;

            foreach (var image in dto.Images)
            {
                var result = await _photoService.AddPhotoAsync(image);
                if (result.Error != null)
                {
                    return BadRequest(new ApiResponse<Product>(
                        false,
                        "Error uploading image",
                        null,
                        new List<string> { result.Error.Message }
                    ));
                }

                urls.Add(result.SecureUrl.AbsoluteUri);
                publicId ??= result.PublicId;
            }

            var product = ProductMapper.FromCreateDto(dto, urls, publicId);

            await _context.ProductRepository.AddProductAsync(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.Id },
                new ApiResponse<Product>(true, "Product created successfully", product)
            );
        }
    }
}