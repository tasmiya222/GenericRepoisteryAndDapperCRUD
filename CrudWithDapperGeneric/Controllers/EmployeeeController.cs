using Dapper;
using Microsoft.AspNetCore.Mvc;
using static CrudWithDapperGeneric.Repoistery.Interface.IGenericRepoistery;
using System.Data;
using CrudWithDapperGeneric.Models;

namespace CrudWithDapperGeneric.Controllers
{
    public class EmployeeeController : Controller
    {
        private readonly IGenericRepository<Employee> _EmployeeRepository;

        public EmployeeeController(IGenericRepository<Employee> employeeRepoistery)
        {
            _EmployeeRepository = employeeRepoistery;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _EmployeeRepository.GetAllAsync();
            return View(list);
        }
        public async Task<IActionResult> AddEditMember(int Id)
        {
            Employee employee = new Employee();
            if (Id > 0)
            {
                var Parameter = new DynamicParameters();
                Parameter.Add("@EmployeeID", Id);

                var ListById = await _EmployeeRepository.QueryStoredProcedureAsync("BrowseEmployeeByID_Sp", Parameter);
                employee = ListById.FirstOrDefault();

            }
            else
            {
                employee = new Employee();
            }
            return View(employee);
        }
        [HttpPost]
        public async Task<IActionResult> AddEditMembers(Employee employee)
        {

            try
            {

                if (employee.EmployeeId != 0)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Name", employee.Name);
                    parameters.Add("@Addrees", employee.Addrees);
                    parameters.Add("@Email", employee.Email);
                    parameters.Add("@PhoneNO", employee.PhoneNO);
                    await _EmployeeRepository.ExecuteStoredProcedureAsync("UpdateEmployee_SP", parameters);

                    return Json(new { success = 1, message = "Data Updated successfully." });

                }
                else
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Name", employee.Name);
                    parameters.Add("@Addrees", employee.Addrees);
                    parameters.Add("@Email", employee.Email);
                    parameters.Add("@PhoneNO", employee.PhoneNO);
        
                    parameters.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await _EmployeeRepository.ExecuteStoredProcedureAsync("InsertEmployee_sp", parameters);

                    var newId = parameters.Get<int>("@NewId");
                    if (newId != 0)
                    {
                        return Json(new { success = 1, message = "Data inserted successfully.", newId });
                    }
                    else
                    {
                        return Json(new { success = 0, message = "Data insertion failed." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = 0, message = "Error" + ex.Message });
            }
        }



        [HttpGet]
        public async Task<IActionResult> DeleteMember(int id)
        {
            try
            {
                var Parameter = new DynamicParameters();
                Parameter.Add("@EmployeID", id);
                var list = await _EmployeeRepository.QueryStoredProcedureAsync("DeleteEmployee_Sp");

                if (list.Any())
                {
                    return Json(new { data = list, message = "Record Found!" });
                }
                else
                {
                    return Json(new { data = new List<string>(), message = "No Record Found!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }
    }
}
}
