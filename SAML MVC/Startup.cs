using System.Security.Cryptography.X509Certificates;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;
using ITfoxtec.Identity.Saml2.Util;
using Microsoft.IdentityModel.Logging;

namespace SAML_MVC
{
    public class Startup
    {
        public static IWebHostEnvironment AppEnvironment { get; private set; }
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            AppEnvironment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            services.BindConfig<Saml2Configuration>(Configuration, "Saml2", (_, saml2Configuration) =>
            {
                saml2Configuration.SigningCertificate =
                    CertificateUtil.Load(AppEnvironment.MapToPhysicalFilePath(Configuration["auth:saml2:SigningCertificateFile"]),
                        Configuration["auth:saml2:SigningCertificatePassword"], X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
                saml2Configuration.AllowedAudienceUris.Add(saml2Configuration.Issuer);

                saml2Configuration.AllowedIssuer = Configuration["auth:saml2:Issuer"];
                saml2Configuration.SingleSignOnDestination = new Uri(Configuration["auth:saml2:SingleSignOnDestination"] + "/" + Configuration["auth:saml2:Tenant"] + "/" + Configuration["auth:saml2:ApplicationID"]);
                saml2Configuration.SingleLogoutDestination = new Uri(Configuration["auth:saml2:SingleLogoutDestination"]!);
                if (saml2Configuration.SigningCertificate.IsValidLocalTime())
                {
                    saml2Configuration.SignatureValidationCertificates.Add(saml2Configuration.SigningCertificate);
                }
                else
                {
                    throw new Exception("The IdP signing certificates is expired.");
                }

                saml2Configuration.SignAuthnRequest = false;

                return saml2Configuration;
            });

            services.AddSaml2();
            services.AddHttpClient();

            services.AddControllersWithViews();
            services.AddSingleton(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSaml2();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}