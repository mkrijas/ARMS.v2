using ArmsModels.BaseModels;
using ArmsServices;
using ArmsServices.DataServices;
using ArmsServices.DataServices.Finance.Transactions;
using ArmsServices.DataServices.FMS;
using ArmsServices.DataServices.General;
using ArmsServices.DataServices.Inventory;
using ArmsServices.DataServices.Operations;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.SignalR.Client;
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

using Views.Data;
using Microsoft.AspNetCore.ResponseCompression;
using Core.IDataServices.Finance.Transactions;
using DAL.DataServices.Finance.Transactions;
using System.Security.Cryptography;
using DAL.DataServices.General;

using Microsoft.AspNetCore.Identity.UI.Services;
using static MudBlazor.Defaults;
using Core.IDataServices.Finance.LedgerViews;
using DAL.DataServices.Finance.LedgerViews;
using Microsoft.AspNetCore.Http;
using Core.IDataServices.Operations;
using DAL.DataServices.Operations;
using Core.IDataServices.Finance;
using DAL.DataServices.Finance;
using ArmsModels.BaseModels.Finance.Transactions;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;

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
            services.AddControllers();

            // Allow CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy( policy =>
                {
                    //policy.WithOrigins("http://*ReportServer").SetIsOriginAllowedToAllowWildcardSubdomains()
                     policy.SetIsOriginAllowed( origin => true )
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            services.AddControllersWithViews();
            //services.AddScoped<AuthenticationStateProvider, CustomAuthenticationSatetProvider>();
            
            services.AddHttpClient();
            // 3rd party 
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Classes.Position.BottomRight;

            });
            services.AddBlazorContextMenu();
            services.AddBlazoredSessionStorage();

            // ------ SIgnalR & Hub ---------- //
            services.AddScoped<SignalRService>();
            services.AddSignalRCore();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                      new[] { "application/octet-stream" });
            });

            // -------- Authorization----------- //
            services.AddAuthorization(config =>
            {
                config.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
                config.AddPolicy("Limited_To_HO", policy => policy.RequireClaim("BranchID", "7"));
            });

            services.AddTransient<ICatalogNameProvider, CatalogNameProvider>();

        #region ---------------Email_Sender---------------

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<EmailSenderOptions>(options =>
            {
                options.Host_Address = "10.200.50.20";
                options.Host_Port = 25;
                options.Host_Username = "noreply_arms@teamthai.in";
                options.Host_Password = "arms@123";
                //options.Host_Address = "smtp-relay.sendinblue.com";
                //options.Host_Port = 587;
                //options.Host_Username = "mkrijas@gmail.com";
                //options.Host_Password = "wbrhVfpAD4c210yU";
                options.Sender_EMail = "noreply_arms@teamthai.in";
                options.Sender_Name = "ArmsV2";
            });
        #endregion

            services.AddSingleton<IDbService, DbService>();

        #region  ------------Operation_Services---------------

            services.AddSingleton<TruckDataArrayModel>();            
            services.AddSingleton<SqlTableDependencyService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IBranchSettingsService, BranchSettingsService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IConsigneeService, ConsigneeService>();
            services.AddScoped<IDriverService, DriverService>();            
            services.AddScoped<IDriverFaultService, DriverFaultService>();
            services.AddScoped<IDriverLeaveService, DriverLeaveService>();
            services.AddScoped<IDriverLicenceService, DriverLicenceService>();
            services.AddScoped<IDriverTransferService, DriverTransferService>();
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
            services.AddScoped<IPartyService, PartyService>();            
            services.AddScoped<IOpTranService, OpTranService>();
            services.AddScoped<IAssetDocumentService, AssetDocumentService>();
            services.AddScoped<IExpenseMappingServices, ExpenseMappingServices>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();            
            services.AddScoped<ITripFuelService, TripFuelService>();
            services.AddScoped<ITripAdvanceService, TripAdvanceService>();
            services.AddScoped<IGeneralSettingsService, GeneralSettingsService>();
            services.AddScoped<ITruckAvailabilityService, TruckAvailabilityService>();
            #endregion

        #region ------------Dashboard---------------
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IFinanceDashboardService, FinanceDashboardService>();
            #endregion

        #region ------------FMS---------------
            services.AddScoped<IBreakdownService, BreakdownService>();
            services.AddScoped<IWorkshopService, WorkshopService>();
            services.AddScoped<IJobcardService, JobcardService>();
            services.AddScoped<IRepairWorkService, RepairWorkService>();
            services.AddScoped<IJobcardWorkshopService, JobcardWorkshopService>();
            services.AddScoped<IJobInProgressService, JobInProgressService>();
            services.AddScoped<IMechanicService, MechanicService>();
            services.AddScoped<IMechanicJobService, MechanicJobService>();
            services.AddScoped<IPeriodicMaintenanceService, PeriodicMaintenanceService>();
            services.AddScoped<IInsuranceClaimService, InsuranceClaimService>();
            services.AddScoped<IRoutineCheckListService, RoutineCheckListService>();
            //services.AddScoped<ITruckTransferService, TruckTransferService>();
        #endregion

        #region ------------INVENTORY-------------------
            services.AddScoped<IInventoryGroupService, InventoryGroupService>();
            services.AddScoped<IInventoryItemService, InventoryItemService>();
            services.AddScoped<IInventoryItemReOrderLevelService, InventoryItemReOrderLevelService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
            services.AddScoped<IInventoryGrnService, InventoryGrnService>();
            services.AddScoped<ITyreService, TyreService>();
            services.AddScoped<IInventoryRequestService, InventoryRequestService>();
            services.AddScoped<IOpInventoryReleaseService, OpInventoryReleaseService>();
            services.AddScoped<IStockTransferService, StockTransferService>();
            #endregion

        #region ------------DATA AUTHENTICATION-------------------
            services.AddScoped<IDataAuthorizationSettingsService, DataAuthorizationSettingsService>();
            services.AddScoped<IDataAuthorizationService, DataAuthorizationService>();
            #endregion

        #region ------------ASSETS-------------------
            services.AddScoped<IAssetClassService, AssetClassService>();
            services.AddScoped<IAssetDocumentRequestService, AssetDocumentRequestService>();
            services.AddScoped<IAssetSettingsService, AssetSettingsService>();
            #endregion

        #region ------------FINANCE-------------------
            services.AddScoped<IChartOfAccountService, ChartOfAccountService>();
            services.AddScoped<ITdsRateService, TdsRateService>();
            services.AddScoped<ITdsAccountMappingService, TdsAccountMappingService>();
            services.AddScoped<ITdsThresholdLimitService, TdsThresholdLimitService>();
            services.AddScoped<IGstUsageIDService, GstUsageIDService>();
            services.AddScoped<IGstItemService, GstItemService>();
            services.AddScoped<ICostCenterService, CostCenterService>();
            services.AddScoped<IDimensionService, DimensionService>();
            services.AddScoped<IInterBranchMappingService, InterBranchTransactionService>();
            services.AddScoped<IRoutineCheckListMasterService, RoutineCheckListMasterService>();
            services.AddScoped<IDocVoucherService, DocVoucherService>();
            services.AddScoped<IMileageShortageReceiptService, MileageShortageReceiptService>();


            //------------FINANCE TRANSACTIONS-------------------

            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ITaxPurchaseService, TaxPurchaseService>();
            services.AddScoped<IAccountInfoService, AccountInfoService>();           
            services.AddScoped<IPaymentInitiatedService, PaymentInitiatedService>();
            services.AddScoped<IPaymentFinalizeService, PaymentFinalizeService>();
            services.AddScoped<IOutstandingBillsService, OutstandingBillsService>();
            services.AddScoped<ISundryPaymentService, SundryPaymentService>();
            services.AddScoped<ISundryReceiptService, SundryReceiptService>();
            services.AddScoped<IContraService, ContraService>();
            services.AddScoped<IFreightBillingService, FreightBillingService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IDrCrNoteService, DrCrNoteService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IJournalService, JournalService>();
            services.AddScoped<IInventoryReleaseService, InventoryReleaseService>();
            services.AddScoped<IFastTagService, FastTagService>();
            services.AddScoped<IReimbursementService, ReimbursementService>();
            services.AddScoped<IReverseEntryService, ReverseEntryService>();
            services.AddScoped<ISundryMaintenanceService, SundryMaintenanceService>();
            services.AddScoped<ItdsEntryService, TdsEntryService>();
            services.AddScoped<IGeneralLedgerTransferService, GeneralLedgerTransferService>();


            //----IBASE GROUP
            services.AddScoped<IbaseInterface<PaymentMemoModel> , PaymentService>();
            services.AddScoped <IbaseInterface<TaxPurchaseModel> , TaxPurchaseService>();
            services.AddScoped<IbaseInterface<SundryReceiptModel>, SundryReceiptService>();
            services.AddScoped<IbaseInterface<SundryPaymentModel>, SundryPaymentService>();
            services.AddScoped<IbaseInterface<ReceiptModel>, ReceiptService>();
            services.AddScoped<IbaseInterface<DrCrNoteModel>, DrCrNoteService>();
            services.AddScoped<IbaseInterface<OpTranModel>, OpTranService>();
            services.AddScoped<IbaseInterface<CancellationReasonCodesByDocumentType>, ReverseEntryService>();
            services.AddScoped<IbaseInterface<SundryMaintenanceModel>, SundryMaintenanceService>();
            services.AddScoped<IbaseInterface<InterBranchReimbursementModel>, ReimbursementService>();
            services.AddScoped<IbaseInterface<TdsTransactionModel>, TdsEntryService>();
            services.AddScoped<IbaseInterface<GeneralLedgerTransferModel>, GeneralLedgerTransferService>();


            //------------FINANCE POSTING GROUP-------------------
            services.AddScoped<IBankPostingGroupService, BankPostingGroupService>();
            services.AddScoped<IBankAccountOwnService, BankAccountOwnService>();
            services.AddScoped<ICustomerPostingGroupService, CustomerPostingGroupService>();
            services.AddScoped<IRenterPostingGroupService, RenterPostingGroupService>();
            services.AddScoped<IVendorPostingGroupService, VendorPostingGroupService>();
            services.AddScoped<ISisterPostingGroupService, SisterPostingGroupService>();
            services.AddScoped<ICashAccountService, CashAccountService>();
            services.AddScoped<IAssetPostingGroupService, AssetPostingGroupService>();
            services.AddScoped<IUnreconciledBankEntryService, UnreconciledBankEntryService>();
            services.AddScoped<IOperationPostingGroupService, OperationPostingGroupService>();
            services.AddScoped<ICancellationReasonCodeService, CancellationReasonCodeService>();

            //-----------FINANCE VIEWS GROUP---------------------

            services.AddScoped<IPartyLedgerViewService, PartyLedgerViewService>();
            services.AddScoped<IAssetLedgerViewService, AssetLedgerViewService>();
            services.AddScoped<IBankLedgerViewService, BankLedgerViewService>();


       //     System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(item => item.GetInterfaces()
       //       .Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(IbaseInterface<>)) && !item.IsAbstract && !item.IsInterface)
       //   .ToList()
       //   .ForEach(assignedTypes =>
       //   {
       //       var serviceType = assignedTypes.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IbaseInterface<>));
       //       services.AddScoped(serviceType, assignedTypes);
       //   });

    
            #endregion

         
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder
            //            .WithOrigins("http://10.200.50.39:8484/") // Add the origin of your Blazor application
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowCredentials());
            //});
        #region------------ASSETS-------------------
            services.AddScoped<IAssetClassService, AssetClassService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IAssetTransferService, AssetTransferService>();
        #endregion

            //------------General-------------------
            services.AddScoped<IConfigTable, ConfigTable>();

        #region--------Identity configure--------------
            services.AddScoped<IUserService, UserStore>();
            services.AddScoped<IRoleService<RoleModel>, RoleStore>();
            services.AddTransient<IUserStore<UserModel>, UserStore>();
            services.AddTransient<IRoleStore<RoleModel>, RoleStore>();            
            services.AddIdentity<UserModel, RoleModel>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultTokenProviders()
                .AddDefaultUI();
            services.AddTransient<IClaimsTransformation, AddUserClaimsTransformation > ();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var connectionString = Configuration.GetConnectionString("ArmsDB");
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

            app.UseResponseCompression(); // SignalR Response compression

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            //app.UseSqlTableDependency<SqlTableDependencyService>(connectionString);


            // Configure proxy


            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapReverseProxy("proxy", proxyPipeline =>
            //    {
            //        // Replace with your Reporting Services server URL
            //        proxyPipeline.UseProxyLoadBalancing();
            //        proxyPipeline.Use((context, next) =>
            //        {
            //            context.Request.Headers.Add("X-Forwarded-Host", context.Request.Host.Host);
            //            return next();
            //        });
            //    });
            //});

           
            //app.UseCors("AllowAnyOriginPolicy");
            //app.UseCors("CorsPolicy");
           // app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

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
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapHub<ServerSignalHub>("/chatHub");
                endpoints.MapFallbackToPage("/_Host");
                
            });
        }
    }
}