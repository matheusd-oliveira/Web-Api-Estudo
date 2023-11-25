using Microsoft.IdentityModel.Tokens;
using PrimeiraAPI.Domain.Model.EmployeeAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrimeiraAPI.Application.Services
{
    public class TokenService
    {
        public static object GenerateToken(Employee employee)
        {
            // Chamando a chave privada.
            var key = Encoding.ASCII.GetBytes(Key.Secret);

            // Configurando o token.
            var tokenConfig = new SecurityTokenDescriptor
            {
                // Payloading armazenado . Ou seja, o conteúdo armazenado.
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("employeeId", employee.id.ToString()),
                }),
                // Definindo tempo de expiração do token.
                Expires = DateTime.UtcNow.AddHours(3),
                // Tipo de assinatura, com o tipo de criptografia usado.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };


            // Gerando o token.
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfig);
            var tokenString = tokenHandler.WriteToken(token);


            return new { token = tokenString };
        }

    }
}
