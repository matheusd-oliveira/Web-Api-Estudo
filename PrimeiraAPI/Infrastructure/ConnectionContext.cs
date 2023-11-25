using Microsoft.EntityFrameworkCore;
using PrimeiraAPI.Domain.Model.CompanyAggregate;
using PrimeiraAPI.Domain.Model.EmployeeAggregate;

namespace PrimeiraAPI.Infrastructure
{
    // Realizando a conexão com o banco de dados.
    public class ConnectionContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Server=localhost;" +
            "Port=5432;Database=employee_sample;" +
            "User id=postgres;" +
            "Password=1234;"
            );
    }
}
