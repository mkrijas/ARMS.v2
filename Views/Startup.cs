using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Views
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();            
            
            services.AddControllersWithViews();
            //services.AddScoped<AuthenticationStateProvider, CustomAuthenticationSatetProvider>();
            
            services.AddHttpClient();


            // 3rd party 
            services.AddMudServices();
            services.AddBlazorContextMenu();
            services.AddBlazoredSessionStorage();


            services.AddAuthorization(config =>
            {
                config.AddPolicy("IsAdmin", policy => policy.RequireClaim("IsAdmin", "true"));
            });


            services.AddSingleton<TruckDataArrayModel>();


            services.AddSingleton<IDbService, DbService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IConsigneeService, ConsigneeService>();
            services.AddScoped<IDriverService, DriverService>();            
            services.AddScoped<IDriverFaultService, DriverFaultService>();
            services.AddScoped<IDriverLeaveService, DriverLeaveService>();
            services.AddScoped<IDriverLicenceService, DriverLicenceService>();
            services.AddScoped<IOrderService, OrderService>();
            //services.AddScoped<IStateService, StateService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<ITariffService, TariffService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<ITruckService, TruckService>();
            services.AddScoped<ITruckTypeService, TruckTypeService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IGcService, GcService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IPartyBranchService, PartyBranchService>();
            services.AddScoped<IPartyService, PartyService>();
            
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITruckDocumentService, TruckDocumentService>();
         


            //services.AddScoped<IPartyDirectorService, PartyDirectorService>();

            //------------FMS---------------
            services.AddScoped<IBreakdownService, BreakdownService>();
            services.AddScoped<IWorkshopService, WorkshopService>();
            services.AddScoped<IJobcardService, JobcardService>();
            services.AddScoped<IRepairJobService, RepairJobService>();
            services.AddScoped<IJobcardWorkshopService, JobcardWorkshopService>();
            services.AddScoped<IJobInProgressService, JobInProgressService>();
            services.AddScoped<IMechanicService, MechanicService>();
            services.AddScoped<IMechanicJobService, MechanicJobService>();
            services.AddScoped<IPeriodicMaintenanceService, PeriodicMaintenanceService>();
            services.AddScoped<IInsuranceClaimService, InsuranceClaimService>();

            //------------FINANCE-------------------
            services.AddScoped<IChartOfAccountService, ChartOfAccountService>();
            services.AddScoped<ITdsRateService, TdsRateService>();
            services.AddScoped<ITdsAccountMappingService, TdsAccountMappingService>();
            services.AddScoped<ITdsThresholdLimitService, TdsThresholdLimitService>();
            //--------Identity configure--------------
            services.AddScoped<IUserService, UserStore>();
            services.AddScoped<IRoleService<RoleModel>, RoleStore>();

            services.AddTransient<IUserStore<UserModel>, UserStore>();
            services.AddTransient<IRoleStore<RoleModel>, RoleStore>();            
            services.AddIdentity<UserModel, RoleModel>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultTokenProviders();
            services.AddTransient<IClaimsTransformation, AddUserClaimsTransformation > ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var cultureInfo = new CultureInfo("en-IN");            
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseHttpsRedirection();          

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(
            //Path.Combine(Directory.GetParent(env.ContentRootPath).ToString(), "ArmsStaticFiles")),
            //    RequestPath = "/Docs"
            //});

            //app.UseDirectoryBrowser(new DirectoryBrowserOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //Path.Combine(Directory.GetParent(env.ContentRootPath).ToString(), "ArmsStaticFiles")),
            //    RequestPath = "/Docs"
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
