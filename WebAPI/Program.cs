using BLL.ServiceInterfaces;
using BLL_EF.Services;
using BLL_DB.Services;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WebstoreContext>();

//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<IBasketService, BasketService>();
//builder.Services.AddScoped<IOrderService, OrderService>();
//builder.Services.AddScoped<IUserService, UserService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IProductService>(provider => new ProductServiceDb(connectionString));
builder.Services.AddScoped<IBasketService>(provider => new BasketServiceDb(connectionString));
builder.Services.AddScoped<IOrderService>(provider => new OrderServiceDb(connectionString));
builder.Services.AddScoped<IUserService>(provider => new UserServiceDb(connectionString));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
