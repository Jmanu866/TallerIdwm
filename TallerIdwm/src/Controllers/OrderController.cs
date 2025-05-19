using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

using TallerIdwm.src.models;
using TallerIdwm.src.data;
using TallerIdwm.src.helpers;
using TallerIdwm.src.mappers;
using TallerIdwm.src.dtos;
using TallerIdwm.src.dtos.Order;
using TallerIdwm.src.controllers;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;




namespace TallerIdwm.src.Controllers;

public class OrderController(ILogger<OrderController> logger, UnitOfWork unitOfWork) : BaseController
{
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<OrderController> _logger = logger;


    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrderAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

        var address = await _unitOfWork.ShippingAddressRepository.GetByUserIdAsync(userId);
        if (address == null)
            return BadRequest(new ApiResponse<string>(false, "No tienes una dirección registrada. Por favor agrégala antes de comprar."));

        var basketId = Request.Cookies["basketId"];
        if (string.IsNullOrEmpty(basketId))
            return BadRequest(new ApiResponse<string>(false, "No se encontró el carrito"));

        var basket = await _unitOfWork.BasketRepository.GetBasketByIdAsync(basketId);
        if (basket == null || !basket.Items.Any())
            return BadRequest(new ApiResponse<string>(false, "El carrito está vacío"));

        var order = OrderMapper.FromBasket(basket, userId, address.Id);

        // Reducir el stock
        foreach (var item in order.Items)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock -= item.Quantity;

                if (product.Stock < 0)
                    return BadRequest(new ApiResponse<string>(false, $"No hay suficiente stock para el producto {product.Name}"));
            }
        }

        await _unitOfWork.OrderRepository.CreateOrderAsync(order);
        _unitOfWork.BasketRepository.DeleteBasket(basket);
        await _unitOfWork.SaveChangesAsync();


        return Ok(new ApiResponse<OrderDto>(true, "Pedido realizado correctamente", OrderMapper.ToOrderDto(order)));
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderSummaryDto>>>> GetAllOrders()
    {
        var orders = await _unitOfWork.OrderRepository.GetAllOrdersAsync();
        var mapped = orders.Select(OrderMapper.ToSummaryDto).ToList();

        return Ok(new ApiResponse<IEnumerable<OrderSummaryDto>>(true, "Todas las órdenes obtenidas", mapped));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

        var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(id, userId);
        if (order == null)
            return NotFound(new ApiResponse<OrderDto>(false, "Pedido no encontrado"));

        return Ok(new ApiResponse<OrderDto>(true, "Pedido encontrado", OrderMapper.ToOrderDto(order)));
    }
}

