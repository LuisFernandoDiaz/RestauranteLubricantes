using Microsoft.IdentityModel.Tokens;
using RestauranteLubricantes.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RestauranteLubricantes.Custom
{

    //crear encriptador SHA256 y JWT en Utilidades.cs - carpeta Custom

    public class Utilidades
    {


        //Aquí estás inyectando la configuración del proyecto, es decir, lo que tienes en tu appsettings.json.
        //Gracias a esto, puedes leer cosas como:   osea el "JWT":{"KEY:"}
        private readonly IConfiguration _config;

        public Utilidades(IConfiguration config)
        {
            _config = config;
        }


        //Encriptar contraseñas antes de guardarlas en la base de datos.
        public string encriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())  //Crea una instancia del algoritmo SHA256 (hash seguro de 256 bits).
            {
                //computar el hash - Convierte el texto (por ejemplo, la contraseña) en un arreglo de bytes.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                //convertir el array de bytes a string
                StringBuilder builder1 = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder1.Append(bytes[i].ToString("x2"));
                }

                return builder1.ToString();

            }
        }



        //Generar tokens JWT para autenticar usuarios cuando inician sesión
        public string generarJWT(Usuario modelo)
        {
            //creamos la informacion del usuario para token
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, modelo.Id.ToString()),//Son los datos que el token guardará dentro, como el Id y Correo.
                new Claim(ClaimTypes.Email, modelo.Correo!)                 //Estos datos se pueden leer luego en los controladores para saber qué usuario hizo la petición.
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));  //verificar que nadie lo haya manipulado
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature); //Esto hace que el token sea seguro y verificable.

            //crear detalle del token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims, //los datos del usuario.
                expires: DateTime.UtcNow.AddMinutes(15), //esto es para definir el tiempo del token
                signingCredentials: credentials);//la firma que valida que el token es auténtico.

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);


        }




    }
}
