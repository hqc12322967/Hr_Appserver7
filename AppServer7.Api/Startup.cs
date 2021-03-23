using AppDatabase.Data;
using AppDatabase.Services;
using HRDependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AppServer7.Api
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

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //https://www.cnblogs.com/lwqlun/p/11137788.html 查阅相关资料

            var assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "AircraftGrt.dll");
            var mvcBuilders = services.AddMvc();
            var controllerAssemblyPart = new AssemblyPart(assembly);
            mvcBuilders.ConfigureApplicationPartManager(apm =>
            {
                apm.ApplicationParts.Add(controllerAssemblyPart);
            });
            mvcBuilders.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);




            services.AddControllers();

            //services.AddScoped<ICompanyRepository, CompanyRepository>();
            #region  依赖注入
            //services.AddScoped<IUserService, UserService>();




            #region AddScoped
            //var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            //var referencedAssemblies = System.IO.Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            //var scopedBaseType = typeof(IHRAddScoped);
            //var scopedTypes = referencedAssemblies
            //    .SelectMany(a => a.DefinedTypes)
            //    .Select(type => type.AsType())
            //    .Where(x => x != scopedBaseType && scopedBaseType.IsAssignableFrom(x)).ToArray();

            //var scopedImplementTypes = scopedTypes.Where(x => x.IsClass).ToArray();
            //var scopedInterfaceTypes = scopedTypes.Where(x => x.IsInterface).ToArray();
            //foreach (var implementType in scopedImplementTypes)
            //{
            //    var interfaceType = scopedInterfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
            //    if (interfaceType != null)
            //        services.AddScoped(interfaceType, implementType);

            //}

            var baseType = typeof(IHRAddScoped);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = System.IO.Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToArray();
            var types = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
            var implementTypes = types.Where(x => x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                    services.AddScoped(interfaceType, implementType);
            }

            #endregion
            /*
            #region AddSingleton
            var SingletonBaseType = typeof(IHRAddSingleton);
            var SingletonTypes = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != SingletonBaseType && SingletonBaseType.IsAssignableFrom(x)).ToArray();

            var SingletonImplementTypes = SingletonTypes.Where(x => x.IsClass).ToArray();
            var SingletonInterfaceTypes = SingletonTypes.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in SingletonImplementTypes)
            {
                var interfaceType = SingletonInterfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                    services.AddSingleton(interfaceType, implementType);

            }
            #endregion

            #region AddTransient
            var TransientBaseType = typeof(IHRAddTransient);
            var TransientTypes = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != TransientBaseType && TransientBaseType.IsAssignableFrom(x)).ToArray();

            var TransientImplementTypes = TransientTypes.Where(x => x.IsClass).ToArray();
            var TransientInterfaceTypes = TransientTypes.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in TransientImplementTypes)
            {
                var interfaceType = TransientInterfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                    services.AddTransient(interfaceType, implementType);

            }
            #endregion
            */
            #endregion


            services.AddDbContext<RmbtDbContext>(options =>
            {
                //options.UseSqlServer("Data Source=localhost;DataBase=routine;Integrated Security=SSPI");
                options.UseSqlite("Data Source=routine.db");
            

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
