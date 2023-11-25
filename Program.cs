using CourtBooker;
using CourtBooker.Middleware;
using CourtBooker.Repositories.Interfaces;
using CourtBooker.Repositories.Postgres;
using CourtBooker.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUsuarioService, PostgresUsuarioRepository>();
builder.Services.AddScoped<IBlocoService, PostgresBlocoRepository>();
builder.Services.AddScoped<IEsporteService, PostgresEsporteRepository>();
builder.Services.AddScoped<IQuadraService, PostgresQuadraRepository>();
builder.Services.AddScoped<IAgendamentoService, PostgresAgendamentoRepository>();


builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<BlocoService>();
builder.Services.AddScoped<EsporteService>();
builder.Services.AddScoped<QuadraService>();
builder.Services.AddScoped<AgendamentoService>();
//builder.Services.AddScoped<IUsuarioService, MongoUsuarioService>();

var app = builder.Build();

app.UseErrorHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyAllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();