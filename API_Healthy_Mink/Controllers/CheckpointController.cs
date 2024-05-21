﻿using API_Healthy_Mink.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Healthy_Mink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckpointController : ControllerBase
    {
        private readonly HealthyMInk_BaseContext _context;

        public CheckpointController(HealthyMInk_BaseContext context) 
        { 
            _context = context; 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShift(int id)
        {
            if (_context.Shifts == null)
            {
                return NotFound();
            }
            var shift = await _context.Shifts.FindAsync(id);

            if (shift == null)
            {
                return NotFound();
            }

            return shift;
        }

        [HttpPost]
        public async Task<ActionResult<Shift>> StartShift(int id, DateTime StartShift)
        {
            try
            {
                List<Shift> shifts = await _context.Shifts.Where(p => p.EmployeeId == id).ToListAsync();
                if (shifts != null)
                {
                    foreach (Shift currentShift in shifts)
                    {
                        if (currentShift.NumberHours == null)
                        {
                            return BadRequest("Shift's not closed");
                        }
                    }
                }

                Shift shift = new Shift()
                {
                    EmployeeId = id,
                    StartShift = StartShift,
                    EndShift = StartShift,
                    NumberHours = null
                };

                await _context.Shifts.AddAsync(shift);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetShift", new { id = shift.Id }, shift);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error {ex.Message}");

            }
        }

        [HttpPut]
        public async Task<ActionResult<Shift>> EndShift(int id, DateTime EndShift)
        {
            try
            {
                List<Shift> shifts = await _context.Shifts.Where(p => p.EmployeeId == id).ToListAsync();
                if (shifts != null)
                {
                    foreach (Shift shift in shifts)
                    {
                        if (shift.NumberHours == null)
                        {
                            shift.EndShift = EndShift;
                            shift.NumberHours = EndShift.Subtract(shift.StartShift).TotalHours;
                            _context.Shifts.Update(shift);
                            await _context.SaveChangesAsync();
                            return CreatedAtAction("GetShift", new { id = shift.Id }, shift);
                        }
                    }
                }
                return BadRequest("Shift's not open.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error {ex.Message}");
            }
            
        }
    }
}