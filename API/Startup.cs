using System;
using System.IO;
using API.Interop;
using API.Services;
using Data.Context;
using MeetMusic.ExceptionMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true);
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MeetMusicDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection"));
            });

            //Add services
            services.AddTransient<IMusicGenreService, MusicGenreService>();
            services.AddTransient<IMusicFamilyService, MusicFamilyService>();
            services.AddTransient<IUserService, UserService>();

            //Add Oauth flow with spotify
            //Deactivated until prompt resolution
            //services.AddAuthentication(options =>
            //    {
            //        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    })
            //    .AddCookie(options =>
            //    {
            //        options.Cookie.Path = "/api";
            //        options.LoginPath = "/api/account";
            //        options.ExpireTimeSpan = TimeSpan.FromDays(20);
            //    })
            //    .AddOAuth("Spotify", options =>
            //    {
            //        options.ClientId = Configuration["Spotify:ClientId"];
            //        options.ClientSecret = Configuration["Spotify:ClientSecret"];
            //        options.CallbackPath = new PathString("/spotify-login");

            //        // Configure the Auth0 endpoints                
            //        options.AuthorizationEndpoint = "https://accounts.spotify.com/authorize";
            //        options.TokenEndpoint = "https://accounts.spotify.com/api/token";
            //        options.UserInformationEndpoint = "https://api.spotify.com/v1/me";

            //        // To save the tokens to the Authentication Properties we need to set this to true
            //        // See code in OnTicketReceived event below to extract the tokens and save them as Claims
            //        options.SaveTokens = true;

            //        // Set scopes to see user's private informations and user's top artists
            //        options.Scope.Clear();
            //        options.Scope.Add("user-read-private");
            //        options.Scope.Add("user-read-email");
            //        options.Scope.Add("user-read-birthdate");
            //        options.Scope.Add("user-top-read");

            //        options.Events = new OAuthEvents
            //        {
            //            OnCreatingTicket = async context =>
            //            {
            //                // Retrieve user info
            //                var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            //                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            //                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //                var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
            //                response.EnsureSuccessStatusCode();

            //                // Extract the user info object
            //                var user = JObject.Parse(await response.Content.ReadAsStringAsync());

            //                // Add the name identifier claim
            //                var userId = user.Value<string>("id");
            //                if (!string.IsNullOrEmpty(userId))
            //                {
            //                    context.Identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, userId, ClaimValueTypes.String, context.Options.ClaimsIssuer));
            //                }

            //                // Add the email
            //                var email = user.Value<string>("email");
            //                if (!string.IsNullOrEmpty(email))
            //                {
            //                    context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String, context.Options.ClaimsIssuer));
            //                }
            //            }
            //        };
            //    });

            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "MeetMusic API",
                    Version = "v1",
                    Description = "A simple music based social API",
                    TermsOfService = "None"
                });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "MeetMusicAPI.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpStatusCodeExceptionMiddleware();
            }
            else
            {
                app.UseHttpStatusCodeExceptionMiddleware();
                app.UseExceptionHandler();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeetMusic API V1");
            });

            //app.UseAuthentication();
            app.UseMvc();
        }
    }
}
