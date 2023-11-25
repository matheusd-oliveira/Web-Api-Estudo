using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeiraAPI.Application.ViewModel;
using PrimeiraAPI.Domain.Model.EmployeeAggregate;

namespace PrimeiraAPI.Controllers
{
    [ApiController]
    [Route("api/v1/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger, IMapper mapper)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add([FromForm] EmployeeViewModel employeeView)
        {
            // Montando a URL do caminho aonde o arquivo vai ser salvo. Consequentemente vai ser o caminho que vai estar salvo no banco de dados.
            var filePath = Path.Combine("Storage", employeeView.Photo.FileName);

            // Salvando o arquivo na memória com a classe Stream.
            using Stream fileStream = new FileStream(filePath, FileMode.Create);

            // Copiando o arquivo que foi salvo em memória .
            employeeView.Photo.CopyTo(fileStream);

            var employee = new Employee(employeeView.Name, employeeView.Age, filePath);

            _employeeRepository.Add(employee);

            return Ok();
        }

        
        [HttpGet]
        public IActionResult Get(int pageNumber, int pageQuantity)
        {
            var listEmployees = _employeeRepository.Get(pageNumber, pageQuantity);

            // Adicionando o log.
            _logger.LogInformation("Lista retornada com sucesso!");

            return Ok(listEmployees);
        }

        [Authorize]
        [HttpPost]
        [Route("{id}/download")]
        public IActionResult DownloadPhoto(int id)
        {
            var employee = _employeeRepository.Get(id);

            var dataBytes = System.IO.File.ReadAllBytes(employee.photo);

            return File(dataBytes, "image/jpg");
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Search(int id)
        {
            var listEmployees = _employeeRepository.Get(id);

            var listEmployeesDTO = _mapper.Map<Employee>(listEmployees);

            return Ok(listEmployeesDTO);
        }
    }
}
