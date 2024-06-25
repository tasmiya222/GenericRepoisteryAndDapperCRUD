namespace CrudWithDapperGeneric.Models
{
    [TableName("tblEmployee")]
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Addrees { get; set; }
        public string Email { get; set; }
        public string PhoneNO { get; set; }
    }
}
