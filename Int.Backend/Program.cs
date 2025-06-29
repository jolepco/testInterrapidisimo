using Int.Backend.Data;
using Int.Backend.Negocio.Implementaciones;
using Int.Backend.Negocio.Interfaces;
using Int.Backend.Servicios.Especificos.Implementaciones;
using Int.Backend.Servicios.Especificos.Interfaces;
using Int.Backend.Servicios.Genericos.Implementaciones;
using Int.Backend.Servicios.Genericos.Interfaces;
using Microsoft.EntityFrameworkCore;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyHeader()
                          .AllowAnyOrigin().
                          AllowAnyMethod()
                          ;
                      });
});

builder.
    Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services
    .AddTransient<IEstudianteRepositorio, EstudianteRepositorio>()
    .AddTransient<IEstudianteMateriaRepositorio, EstudianteMateriaRepositorio>()
    .AddTransient<IMateriaRepositorio, MateriaRepositorio>()
    .AddTransient<IProfesorMateriaRepositorio, ProfesorMateriaRepositorio>()
    .AddTransient<IProfesorRepositorio, ProfesorRepositorio>();

builder.Services.AddTransient<IEstudianteNegocio, EstudianteNegocio>();

builder.Services.AddTransient<SeedDb>();
var app = builder.Build();
SeedData(app);
void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory!.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<SeedDb>();
        service!.SeedAsync().Wait();
    }

}
// Configure the HTTP request pipeline.
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

    if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
           
        }



app.Run();
