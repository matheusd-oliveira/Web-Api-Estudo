using Microsoft.AspNetCore.Connections;
using PrimeiraAPI.Domain.DTOs;
using PrimeiraAPI.Domain.Model.EmployeeAggregate;

namespace PrimeiraAPI.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ConnectionContext _context = new ConnectionContext();
        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public List<EmployeeDTO> Get(int pageNumber, int pageQuantity)
        {
            // Skip -> Pega a posição da onde eu quero o registro.
            // Take -> Pega a quantidade de dados que eu quero retornar.
            return _context.Employees
                .Skip(pageNumber * pageQuantity)
                .Take(pageQuantity)
                .Select(b =>
                    new EmployeeDTO
                    {
                        Id = b.id,
                        EmployeeName = b.name,
                        EmployeeAge = b.age,
                        Photo = b.photo
                    }
                ).ToList();

        }

        public Employee? Get(int id)
        {
            return _context.Employees.Find(id);
        }

    }
}
