using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TallerIdwm.src.models;
using TallerIdwm.src.interfaces;
using TallerIdwm.src.data;
using TallerIdwm.src.dtos;
using TallerIdwm.src.mappers;
using TallerIdwm.src.helpers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;

namespace TallerIdwm.src.controllers
{
    public class BasketController(ILogger<ProductController> logger, UnitOfWork unitOfWork) : BaseController
    {
        private readonly ILogger<ProductController> _logger = logger;
        private readonly UnitOfWork _unitOfWork = unitOfWork;
        [HttpGet]
        public async Task<ActionResult<ApiResponse<BasketDto>>> GetBasket()
        {
            var basket = await RetrieveBasket();
            if (basket == null)
                return NoContent();

            return Ok(new ApiResponse<BasketDto>(
                true,
                "Carrito obtenido correctamente",
                basket.ToDto()
            ));
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<BasketDto>>> AddItemToBasket(int productId, int quantity)
        {
            _logger.LogWarning("Entrando a AddItemToBasket con productId: {ProductId}, quantity: {Quantity}", productId, quantity);

            if (quantity <= 0)
            {
                return BadRequest(new ApiResponse<string>(false, "La cantidad debe ser mayor a 0."));
            }

            var basket = await RetrieveBasket();

            if (basket == null)
            {
                basket = CreateBasket();
                await _unitOfWork.SaveChangesAsync();
            }

            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (product == null)
                return BadRequest(new ApiResponse<string>(false, "Producto no encontrado"));

            if (product.Stock == 0)
                return BadRequest(new ApiResponse<string>(false, $"El producto '{product.Name}' no tiene stock disponible."));

            if (product.Stock < quantity)
                return BadRequest(new ApiResponse<string>(false, $"Solo hay {product.Stock} unidades disponibles de '{product.Name}'"));

            basket.AddItem(product, quantity);

            var changes = await _unitOfWork.SaveChangesAsync();
            var success = changes > 0;

            return success
                ? CreatedAtAction(nameof(GetBasket), new ApiResponse<BasketDto>(true, "Producto añadido al carrito", basket.ToDto()))
                : BadRequest(new ApiResponse<string>(false, "Ocurrió un problema al actualizar el carrito"));
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<BasketDto>>> RemoveItemFromBasket(int productId, int quantity)
        {
            var basket = await RetrieveBasket();
            if (basket == null)
                return BadRequest(new ApiResponse<string>(false, "Carrito no encontrado"));

            basket.RemoveItem(productId, quantity);

            var success = await _unitOfWork.SaveChangesAsync() > 0;

            return success
                ? Ok(new ApiResponse<BasketDto>(
                    true,
                    "Producto eliminado del carrito",
                    basket.ToDto()
                ))
                : BadRequest(new ApiResponse<string>(false, "Error al actualizar el carrito"));
        }

        [Authorize]
        [HttpPut("update-quantity")]
        public async Task<ActionResult<ApiResponse<BasketDto>>> UpdateItemQuantity(int productId, int quantity)
        {
            _logger.LogWarning("Entrando a UpdateItemQuantity con productId: {ProductId}, nueva cantidad: {Quantity}", productId, quantity);

            if (quantity < 0)
                return BadRequest(new ApiResponse<string>(false, "La cantidad no puede ser negativa. Usa DELETE si deseas eliminar."));

            var basket = await RetrieveBasket();
            if (basket == null)
                return BadRequest(new ApiResponse<string>(false, "Carrito no encontrado"));

            var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return NotFound(new ApiResponse<string>(false, "El producto no está en el carrito"));

            if (quantity == 0)
            {
                basket.RemoveItem(productId, item.Quantity); // Elimina completamente el producto del carrito
            }
            else
            {
                item.Quantity = quantity;
            }

            var success = await _unitOfWork.SaveChangesAsync() > 0;

            return success
                ? Ok(new ApiResponse<BasketDto>(true, "Cantidad actualizada correctamente", basket.ToDto()))
                : BadRequest(new ApiResponse<string>(false, "Error al actualizar la cantidad"));
        }

        [Authorize]
        [HttpGet("empty")]
        public async Task<ActionResult<ApiResponse<object>>> GetBasketStatus()
        {
            var basket = await RetrieveBasket();

            if (basket == null || !basket.Items.Any())
            {
                return Ok(new ApiResponse<object>(true, "El carrito está vacío", new
                {
                    isEmpty = true,
                    itemCount = 0,
                    total = 0
                }));
            }

            var itemCount = basket.Items.Sum(i => i.Quantity);
            var total = basket.Items.Sum(i => i.Quantity * i.Product.Price);

            return Ok(new ApiResponse<object>(true, "El carrito tiene productos", new
            {
                isEmpty = false,
                itemCount,
                total
            }));
        }

        private async Task<Basket?> RetrieveBasket()
        {
            var basketId = Request.Cookies["basketId"];
            _logger.LogWarning("BasketId recibido desde cookie: {BasketId}", basketId);

            return string.IsNullOrEmpty(basketId)
                ? null
                : await _unitOfWork.BasketRepository.GetBasketByIdAsync(basketId);
        }

        private Basket CreateBasket()
        {
            var basketId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(30),
            };

            Response.Cookies.Append("basketId", basketId, cookieOptions);
            _logger.LogWarning("Nuevo basket creado con ID: {BasketId}", basketId);

            return _unitOfWork.BasketRepository.CreateBasket(basketId);
        }
    }
}