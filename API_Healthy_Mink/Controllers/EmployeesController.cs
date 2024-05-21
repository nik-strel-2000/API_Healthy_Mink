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
        //экземпляр для обращение к БД
        private readonly HealthyMInk_BaseContext _context;

        //конструктор класса
        public EmployeesController(HealthyMInk_BaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод вывода всех сотрудников
        /// </summary>
        /// <returns></returns>
        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }
        /// <summary>
        /// Метод вывода всех должностей
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/Roles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }
        /// <summary>
        /// Вспомогательный метод для возврата json
        /// </summary>
        /// <param name="id">id сотрудника</param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод вывод всех пользователей с определенной должностью
        /// </summary>
        /// <param name="Role">Название должности</param>
        /// <returns></returns>
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

        /// <summary>
        /// Перегрузка метода обновление данных сотрудника с использованием id должности
        /// </summary>
        /// <param name="id">id Сотрудника</param>
        /// <param name="employee">обновленная модель данных сотрудника</param>
        /// <returns></returns>
        [HttpPut("RoleId/{id}")]
        public async Task<ActionResult<Employee>> PutEmployee(int id, Employee employee)
        {
            return await EditEmployee(id, employee);
        }
        /// <summary>
        /// Перегрузка метода обновление данных сотрудника с использованием названия должности
        /// </summary>
        /// <param name="id">id Сотрудника</param>
        /// <param name="employeeValid">обновленная модель данных сотрудника</param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод обновления данных ползователя
        /// </summary>
        /// <param name="id">id Сотрудника</param>
        /// <param name="employee">обновленная модель данных сотрудника</param>
        /// <returns></returns>
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
        /// <summary>
        /// Перегрузка метода создания пользователя с использованием id Должности
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost("/api/Employees/RoleId")]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            return await SaveEmployee(employee);
        }
        /// <summary>
        /// Перегрузка метода создания пользователя с использованием названия Должности
        /// </summary>
        /// <param name="employeeValid"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод создания сотрудника
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private async Task<ActionResult<Employee>> SaveEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
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
        
        /// <summary>
        /// Метод даления сотрудника
        /// </summary>
        /// <param name="id">id сотрудника</param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод проверки на существования сотрудника
        /// </summary>
        /// <param name="id">id сотрудника</param>
        /// <returns></returns>
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
