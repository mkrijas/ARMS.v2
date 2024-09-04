using ARMS.Extensions;
using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbService, DbService>();
builder.Services.AddJWTTokenServices(builder.Configuration);

#region--------Identity configure--------------
builder.Services.AddScoped<IUserService, UserStore>();
//builder.Services.AddScoped<IRoleService<RoleModel>, RoleStore>();
builder.Services.AddTransient<IUserStore<UserModel>, UserStore>();
//builder.Services.AddTransient<IRoleStore<RoleModel>, RoleStore>();
builder.Services.AddIdentityCore<UserModel>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders();
builder.Services.AddTransient<IClaimsTransformation, AddUserClaimsTransformation>();
#endregion

// Add services to the container.

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
}); 

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
