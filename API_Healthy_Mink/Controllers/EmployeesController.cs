using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Healthy_Mink.Models;
using API_Healthy_Mink.Models.ValidModel;
using System.Xml.Linq;

namespace API_Healthy_Mink.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly HealthyMInk_BaseContext _context;

        public EmployeesController(HealthyMInk_BaseContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }
        
        [HttpGet("/api/Roles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        [HttpGet("/api/Employee/{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // GET: api/Employees/Менеджер
        [HttpGet("{Role}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeeRole(string Role)
        {
            if (_context.Roles.Where(p => p.Name == Role).FirstOrDefault() != null)
            {
                int IdRole = _context.Roles.Where(p => p.Name == Role).FirstOrDefault().Id;
                var employee = await _context.Employees.Where(p => p.RoleId == IdRole).ToListAsync();

                if (employee == null)
                {
                    return NotFound();
                }

                return employee;
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There is no such role.");
            }   
        }

        [HttpPut("RoleId/{id}")]
        public async Task<ActionResult<Employee>> PutEmployee(int id, Employee employee)
        {
            return await EditEmployee(id, employee);
        }

        [HttpPut("RoleName/{id}")]
        public async Task<ActionResult<Employee>> PutEmployee(int id, EmployeeValind employeeValid)
        {
            if (_context.Roles.Where(p => p.Name == employeeValid.RoleName).FirstOrDefault() != null)
            {
                int IdRole = _context.Roles.Where(p => p.Name == employeeValid.RoleName).FirstOrDefault().Id;
                Employee employee = new Employee
                {
                    Id = employeeValid.id,
                    FirstName = employeeValid.FirstName,
                    LastName = employeeValid.LastName,
                    MiddleName = employeeValid.MiddleName,
                    RoleId = IdRole,
                };
                return await EditEmployee(id, employee);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There is no such role.");
            }
        }
        
        private async Task<ActionResult<Employee>> EditEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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
        
        //2 Метода создания пользователя с полем ввиде id и Названия должности
        [HttpPost("/api/Employees/RoleId")]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            return await SaveEmployee(employee);
        }

        [HttpPost("/api/Employees/RoleName")]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeValind employeeValid)
        {

            if (_context.Roles.Where(p => p.Name == employeeValid.RoleName).FirstOrDefault() != null)
            {
                int IdRole = _context.Roles.Where(p => p.Name == employeeValid.RoleName).FirstOrDefault().Id;
                Employee employee = new Employee
                {
                    FirstName = employeeValid.FirstName,
                    LastName = employeeValid.LastName,
                    MiddleName = employeeValid.MiddleName,
                    RoleId = IdRole,
                };
                return await SaveEmployee(employee);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"There is no such role.");
            }
        }

        private async Task<ActionResult<Employee>> SaveEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            _context.Employees.Add(employee);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
            }
            catch (DbUpdateException ex)
            {
                if (EmployeeExists(employee.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
        }
        
        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
