using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
using StackExchange.Redis;
using WebApIPerformace.Model;

namespace WebApIPerformace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private static ConnectionMultiplexer redis;
        [HttpGet]
        public async Task<IActionResult> GetUsuario()
        {
            //Criação de cache get usuarios
            string key = "getUsuarios";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase(); 
            await db.KeyExpireAsync(key, TimeSpan.FromSeconds(10)); //Determina 2 minutos de cache
            String user = await db.StringGetAsync(key);

            if (!string.IsNullOrEmpty(user)) //se não for nulo
                //se encontrar algo no cache, retorne ele
            {
                return Ok(user);
            }

            //caso n encontre nd no cache faz a conexão/chamado no banco
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123;";
            using var connection = new MySqlConnection(connectionString); // conectar no banco
            await connection.OpenAsync();
            String query = "Select Id, Nome, Email from Usuarios;";
            var usuarios = await connection.QueryAsync<Usuario>(query); //executar a query

            // Como ele teve que bater no banco para recuperar a lista de usurarios, agr pega essa info e joga pro redis(cache)
            //e quando a info for jogada pro cache, ela durará o tempo setado e quando fizer outra requisição o acesso será pelo cache e n no banco
            //quando acabar o tempo, o cache apaga automaticamente
            //Para salvar a lista de usuarios necessitamos converter a variavel para string

            string usuariosJson = JsonConvert.SerializeObject(usuarios);
            await db.StringSetAsync(key, usuariosJson);

            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123;";
            using var connection = new MySqlConnection(connectionString); // conectar no banco
            await connection.OpenAsync();

            string sql = "Insert into Usuarios(Nome, Email) value(@nome, @email);"; //@nome e @email por conta de que em Usuario aqui tem esses parametros
            await connection.ExecuteAsync(sql, usuario);

            //Apaga o cache
            string key = "getUsuarios";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            return Ok(usuario);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Usuario usuario)
        {
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123;";
            using var connection = new MySqlConnection(connectionString); // conectar no banco
            await connection.OpenAsync();

            string sql = "update Usuarios set Nome = @nome, Email = @email where Id = @id;"; 
            await connection.ExecuteAsync(sql, usuario);

            //Apaga o cache
            string key = "getUsuarios";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123;";
            using var connection = new MySqlConnection(connectionString); // conectar no banco
            await connection.OpenAsync();

            string sql = "delete from Usuarios where Id = @id";
            await connection.ExecuteAsync(sql, new {id});

            //Apaga o cache
            string key = "getUsuarios";
            redis = ConnectionMultiplexer.Connect("localhost:6379");
            IDatabase db = redis.GetDatabase();
            await db.KeyDeleteAsync(key);

            return Ok();
        }
    }
}
