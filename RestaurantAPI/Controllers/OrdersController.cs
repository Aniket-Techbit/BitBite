using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly RestaurantDBContext _context;

        public OrdersController(RestaurantDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderMaster>>> GetOrderMasters()
        {
            return await _context.OrderMasters.Include(x => x.Customer).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderMaster>> GetOrderMaster(long id)
        {
            //var orderMaster = await _context.OrderMasters.FindAsync(id);
            var orderDetails = await (from om in _context.Set<OrderMaster>()
                                      join od in _context.Set<OrderDetail>()
                                      on om.OrderMasterId equals od.OrderMasterId
                                      join fi in _context.Set<FoodItem>()
                                      on od.FoodItemId equals fi.FoodItemId
                                      where om.OrderMasterId == id
                                      select new
                                      {
                                          od.OrderDetailId,
                                          od.FoodItemPrice,
                                          od.Quantity,
                                          od.FoodItemId,
                                          fi.FoodItemName,
                                          om.OrderMasterId
                                      }).ToListAsync();

            var orderMaster = await (from om in _context.Set<OrderMaster>()
                                     where om.OrderMasterId == id
                                     select new
                                     {
                                         om.OrderMasterId,
                                         om.OrderNumber,
                                         om.PaymentMethod,
                                         om.GrandTotal,
                                         om.CustomerId,
                                         deletedOrderItemIds = "",
                                         orderDetails
                                     }).FirstOrDefaultAsync();

            if (orderMaster == null)
            {
                return NotFound();
            }

            return Ok(orderMaster);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderMaster(long id, OrderMaster orderMaster)
        {
            if (id != orderMaster.OrderMasterId)
            {
                return BadRequest();
            }

            _context.Entry(orderMaster).State = EntityState.Modified;

            // Existing food item & newly added food items
            foreach (OrderDetail item in orderMaster.OrderDetails!)
            {
                if (item.OrderDetailId == 0)
                {
                    _context.OrderDetails.Add(item);
                }
                else
                {
                    _context.Entry(item).State = EntityState.Modified;
                }
            }

            // Delete food items
            foreach (var itemId in orderMaster.DeletedOrderItemIds!.Split(',').Where(x => x != ""))
            {
                OrderDetail orderDetail = _context.OrderDetails.Find(Convert.ToInt64(itemId))!;
                _context.OrderDetails.Remove(orderDetail);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderMasterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<OrderMaster>> PostOrderMaster(OrderMaster orderMaster)
        {
            _context.OrderMasters.Add(orderMaster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderMaster", new { id = orderMaster.OrderMasterId }, orderMaster);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderMaster(long id)
        {
            var orderMaster = await _context.OrderMasters.FindAsync(id);
            if (orderMaster == null)
            {
                return NotFound();
            }

            _context.OrderMasters.Remove(orderMaster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderMasterExists(long id)
        {
            return _context.OrderMasters.Any(e => e.OrderMasterId == id);
        }
    }
}
