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

using TallerIdwm.src.RequestHelpers;

using Microsoft.AspNetCore.Authorization;
using TallerIdwm.src.Helpers;



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
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
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
                        "Error al agregar la imagen",
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
                new ApiResponse<Product>(true, "Producto agregado correctamente", product)
            );
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Product>>> Update(int id, [FromForm] ProductDto dto)
        {
            var product = await _context.ProductRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new ApiResponse<Product>(false, "Producto no encontrado"));


            if (dto.Images.Any())
            {
                // Eliminar TODAS las imágenes anteriores usando las URLs
                if (product.Urls != null && product.Urls.Any())
                {
                    foreach (var url in product.Urls)
                    {
                        var oldPublicId = CloudinaryHelper.ExtractPublicIdFromUrl(url);
                        if (!string.IsNullOrEmpty(oldPublicId))
                            await _photoService.DeletePhotoAsync(oldPublicId);
                    }
                }

                // Subir la nueva imagen (puedes extender a varias si lo permites)
                var result = await _photoService.AddPhotoAsync(dto.Images.First());
                if (result.Error != null)
                {
                    return BadRequest(new ApiResponse<Product>(
                        false,
                        "Error al subir nueva imagen",
                        null,
                        new List<string> { result.Error.Message }
                    ));
                }

                product.Urls = new List<string> { result.SecureUrl.AbsoluteUri };
                product.PublicId = result.PublicId;
            }

            // Actualizar datos básicos
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Category = dto.Category;
            product.Brand = dto.Brand;
            product.Stock = dto.Stock;

            await _context.ProductRepository.UpdateProductAsync(product);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Product>(true, "Producto actualizado correctamente", product));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var product = await _context.ProductRepository.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new ApiResponse<string>(false, "Producto no encontrado"));

            bool isInOrders = await _context.ProductRepository.IsProductInOrdersAsync(id);
            if (isInOrders)
            {
                product.IsActive = false;
                await _context.ProductRepository.UpdateProductAsync(product);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<string>(true, "Producto desactivado correctamente (asociado a pedidos)"));
            }

            if (product.Urls != null && product.Urls.Any())
            {
                foreach (var url in product.Urls)
                {
                    var publicId = CloudinaryHelper.ExtractPublicIdFromUrl(url);
                    if (!string.IsNullOrEmpty(publicId))
                        await _photoService.DeletePhotoAsync(publicId);
                }
            }

            await _context.ProductRepository.DeleteProductAsync(product);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string>(true, "Producto eliminado correctamente"));
        }
    }
}