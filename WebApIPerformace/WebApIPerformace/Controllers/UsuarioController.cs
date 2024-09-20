using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using WebApIPerformace.Model;

namespace WebApIPerformace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsuario()
        {
       
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123;";
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();
            String query = "Select Id, Nome, Email from Usuarios;";
            var usuarios = await connection.QueryAsync<Usuario>(query);

            return Ok(usuarios);
        }
    }
}
