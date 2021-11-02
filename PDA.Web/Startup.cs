using DNTCaptcha.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PDA.Repository;
using PDA.Web.Extensions;
using System;

namespace PDA.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices(Configuration);

            services.AddDbContext<PDADbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver =
                        new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                })
                .AddSessionStateTempDataProvider();

            // For FileUpload
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
                x.ValueLengthLimit = int.MaxValue; //not recommended value
                x.MemoryBufferThreshold = Int32.MaxValue;
            });

            // For Setting Session Timeout
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
            });


            //  Cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
                options.OnAppendCookie = (context) =>
                {
                    context.IssueCookie = true;
                };
            });

            services.AddAntiforgery(x => x.HeaderName = "XSRF-TOKEN");

            services.AddControllersWithViews();
            //services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddControllers();

            // using Memory Cache 
            services.AddMemoryCache();

            #region Registering AddDNTCaptcha

            //  AddDNTCaptcha
            services.AddDNTCaptcha(options =>
                options.UseCookieStorageProvider()
                    .ShowThousandsSeparators(false)
                    .WithEncryptionKey("9F3baE2KFTM7m0C^tt%^Ag")
                    );
            #endregion
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
                //app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/error/{0}"); 

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCookiePolicy();
            // Enabling Session
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Portal}/{action=Login}/{id?}");
                //pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
