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

namespace TallerIdwm.src.controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly StoreContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger,StoreContext context)
        {
            _context = context;
            _logger = logger;
        }
        

       
        [HttpGet]
        public ActionResult <List<Product>> GetAll()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
       
    }
}