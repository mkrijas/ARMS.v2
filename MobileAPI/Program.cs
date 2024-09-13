using ARMS.Extensions;
using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using Core.IDataServices.Operations;
using DAL.DataServices.Operations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MobileAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbService, DbService>();
builder.Services.AddJWTTokenServices(builder.Configuration);

//builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped<IDataAuthorizationSettingsService, DataAuthorizationSettingsService>();
//builder.Services.AddScoped<IDataAuthorizationService, DataAuthorizationService>();

#region--------Identity configure--------------
builder.Services.AddScoped<IUserService, UserStore>();
builder.Services.AddTransient<IRoleService<RoleModel>, RoleService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddTransient<IUserStore<UserModel>, UserStore>();
builder.Services.AddTransient<IRoleStore<RoleModel>, RoleService>();
builder.Services.AddScoped<ITruckService, TruckService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<IGcService, GcService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IBranchSettingsService, BranchSettingsService>();
builder.Services.AddScoped<ITariffService, TariffService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddScoped<IProjectTonnageService, ProjectTonnageService>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<ITruckService, TruckService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ITruckStatusUpdateService, TruckStatusUpdateService>();

builder.Services.AddIdentity<UserModel, RoleModel>(options => options.SignIn.RequireConfirmedAccount = true)
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
