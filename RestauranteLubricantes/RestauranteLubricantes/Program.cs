using Microsoft.AspNetCore.Authentication.JwtBearer; //Permite usar autenticación con JWT tokens.
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;//: Se usa para validar y manejar los tokens JWT.
using System.Text; //Necesario para codificar la clave secreta (Encoding.UTF8).
using RestauranteLubricantes.Models;
using RestauranteLubricantes.Custom;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();  //Habilita la documentación automática OpenAPI/Swagger, para probar tus endpoints.

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestauranteLubricantes API", Version = "v1" });
});




//agregamos esto ---Conexión a la Base de Datos
builder.Services.AddDbContext<PolleriaLubricantesContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cn"));
});
//Esto le dice a .NET:
//“Cada vez que necesite acceder a la BD, usa este contexto con esta conexión SQL Server.”






//agregamos esto    --- útil para clases de ayuda que no dependen del contexto o del usuario.
builder.Services.AddSingleton<Utilidades>();

//agregamos esto  
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>//Aquí defines cómo tu API validará los tokens que recibe.
{
    config.RequireHttpsMetadata = false; //Desactiva la validación HTTPS (Por defecto, JWT exige HTTPS.Esta línea desactiva ese requisito).
    config.SaveToken = true; //Guarda el token validado.
    config.TokenValidationParameters = new TokenValidationParameters//Aquí defines cómo se validará el token recibido.
    {
        ValidateIssuerSigningKey = true,//Verifica la firma del token con tu clave secreta
        ValidateIssuer = false,  //: No se validan esos campos (opcional).
        ValidateAudience = false,//: No se validan esos campos (opcional).
        ValidateLifetime = true,//Rechaza tokens expirados.
        ClockSkew = TimeSpan.Zero,//Evita tiempo de tolerancia para expiración.
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))//lee la clave del archivo appsettings y lo convierte a bytes
    };
});//Esto asegura que solo los tokens generados con tu clave secreta sean válidos.




//agregamos esto ---parte del cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("NewPolicy", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});






var app = builder.Build();//Crea la aplicación web (usa todo lo que configuraste arriba).

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestauranteLubricantes API v1");
        c.RoutePrefix = string.Empty; // 👈 Esto hace que se abra directamente en https://localhost:7291/
    });


   app.MapOpenApi();//Activa la documentación OpenAPI solo en entorno Development.
}
app.UseHttpsRedirection();



//agregamos esto cors
app.UseCors("NewPolicy");
//agregamos esto
app.UseAuthentication();//Valida el token JWT y autentica al usuario.




app.UseAuthorization();//Verifica si el usuario tiene permisos (roles, políticas).
app.MapControllers();//Activa los endpoints definidos en tus controladores.
app.Run();
