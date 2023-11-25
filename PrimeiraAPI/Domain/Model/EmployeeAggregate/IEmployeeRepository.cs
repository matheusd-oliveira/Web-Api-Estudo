using PrimeiraAPI.Domain.DTOs;

namespace PrimeiraAPI.Domain.Model.EmployeeAggregate
{
    public interface IEmployeeRepository
    {
        // Apenas a minha regra de negócio.

        void Add(Employee employee);

        List<EmployeeDTO> Get(int pageNumber, int pageQuantity);

        Employee? Get(int id);
    }
}
