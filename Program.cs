using api.Data;
using api.Interfaces;
// using api.Interfaces;
using api.Models;
using api.Repository;
// using api.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;



//program utama untuk aplikasi ASP.NET Core
//di gambarkan seperti belakangan yang mengatur alur eksekusi aplikasi

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers() // ini fungsinya untuk menambahkan layanan kontroler ke dalam aplikasi
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true; //agar json terlihat lebih rapi
    });

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); //Ini adalah baris yang membuat Swagger bekerja.




builder.Services.AddControllers().AddNewtonsoftJson(options =>
    {
        // untuk menghindari masalah referensi loop saat serialisasi JSON
        // Contoh kasus: comments -> di dalam stock -> comments (akan looping saat get data di stock), bisa menyebabkan "Object cycle detected"
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; 
    });


builder.Services.AddDbContext<ApplicationDBContext>(options => // menambahkan DbContext ke dalam layanan (untuk database)
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//ditambahkan jika menambah repository
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build(); 
app.MapControllers(); //Mendaftarkan semua endpoint di controller kamu ke routing system
//dia akan mencari ini: [ApiController]
// [Route("...")]


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) //untuk menjalankan swagger; tampilan ui swagger
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection(); // untuk mengalihkan permintaan HTTP ke HTTPS

// ============================
// Bagian weatherforecast remark
// ============================

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

// ============================

app.MapControllers();
app.Run();
