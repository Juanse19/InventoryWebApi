
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryWebApi.DataAccess;
using InventoryWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InventoryWebApi.DTO;
using Microsoft.EntityFrameworkCore;

namespace InventoryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : Controller
    {
        public readonly InventoryContext _context;

        public InventoryController(InventoryContext context)
        {
            _context = context;
        }
        
        //Get: api/Inventory 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventory()
        {
            return await _context.Inventory.ToListAsync();
        }
        
        // GET: api/Inventory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItem>> GetInventory(int id)
        {
            var Inventory = await _context.Inventory.FindAsync(id);
 
            if (Inventory == null)
            {
                return NotFound();
            }
 
            return Inventory;
        }
        
        // PUT: api/Inventory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventoryItem(int id, InventoryItem inventoryItem)
        {
            if (id != inventoryItem.Barcode)
            {
                return BadRequest();
            }
 
            _context.Entry(inventoryItem).State = EntityState.Modified;
 
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
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
 
        // POST: api/Inventory
        [HttpPost]
        public async Task<ActionResult<InventoryItem>> AddInventoryItem(InventoryItem inventoryItem)
        {
            _context.Inventory.Add(inventoryItem);
            await _context.SaveChangesAsync();
 
            return CreatedAtAction("GetInventory", new { id = inventoryItem.Barcode }, inventoryItem);
        }
 
        // DELETE: api/Inventory/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InventoryItem>> DeleteInventoryItem(int id)
        {
            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
 
            _context.Inventory.Remove(inventory);
            await _context.SaveChangesAsync();
 
            return inventory;
        }
 
        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.Barcode == id);
        }
    }
    
}

