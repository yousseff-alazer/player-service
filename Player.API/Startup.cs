using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Reservation.DAL.DB;
using Player.DAL.mysqlplayerDB;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MassTransit;
using Reservation.Helpers;
using Player.API.Consumers;
using GreenPipes;
using Microsoft.OpenApi.Models;
using Challenge.API.Consumers;
using Player.Reminders;
using Player.CommonDefinitions.Records;

namespace Reservation.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var allowedOrigins = Configuration.GetSection("AllowedOrigins").Get<String[]>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<PlayerPointConsumer>();
                x.AddConsumer<SportConsumer>();
                x.AddConsumer<SportLocalizeConsumer>();
                x.AddConsumer<NotificationConsumer>();
                x.AddConsumer<LanguageConsumer>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri(Configuration.GetValue<string>("RabbitMQ:RabbitMqRootUri")), h =>
                    {
                        h.Username(Configuration.GetValue<string>("RabbitMQ:UserName"));
                        h.Password(Configuration.GetValue<string>("RabbitMQ:Password"));
                    });
                    cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:playerEnergyPointQueue"), ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<PlayerPointConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:notificationGroupQueue"), ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<NotificationConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:notificationVenueQueue"), ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<NotificationConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:notificationChallengeQueue"), ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<NotificationConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:notificationNutritionistQueue"), ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<NotificationConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:notificationPhysiotherapistQueue"), ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<NotificationConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:notificationReservationQueue"), ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<NotificationConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:notificationCoachQueue"), ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<NotificationConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:sportPlayerQueue"), ep =>
                    {
                        ep.UseMessageRetry(retryConfigurator =>
                        {
                            retryConfigurator.Incremental(
                                3,
                                TimeSpan.FromSeconds(1),
                                TimeSpan.FromSeconds(30)
                            );
                        });

                        ep.ConfigureConsumer<SportConsumer>(provider);
                    });
                    cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:sportLocalizePlayerQueue"), ep =>
                    {
                        ep.UseMessageRetry(retryConfigurator =>
                        {
                            retryConfigurator.Incremental(
                                3,
                                TimeSpan.FromSeconds(1),
                                TimeSpan.FromSeconds(30)
                            );
                        });

                        ep.ConfigureConsumer<SportLocalizeConsumer>(provider);
                    });
					cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:RabbitMqLanguagePlayerUri"), ep =>
					{
						ep.UseMessageRetry(retryConfigurator =>
						{
							retryConfigurator.Incremental(
								3,
								TimeSpan.FromSeconds(1),
								TimeSpan.FromSeconds(30)
							);
						}
								);

						ep.ConfigureConsumer<LanguageConsumer>(provider);
					});
					cfg.ReceiveEndpoint(Configuration.GetValue<string>("RabbitMQ:RabbitMqLanguagePlayerUri_Skipped"), ep =>
					{
						ep.UseMessageRetry(retryConfigurator =>
						{
							retryConfigurator.Incremental(
								3,
								TimeSpan.FromSeconds(1),
								TimeSpan.FromSeconds(30)
							);
						}
								);

						ep.ConfigureConsumer<LanguageConsumer>(provider);
					});

				}));
            });
            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddCors();
            services.Configure<AppSettingsRecord>(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservice.Todo.Consumer", Version = "v1" });
            });

            services.AddMvc(c => { })
                 .AddNewtonsoftJson(
                     options =>
                     {
                         //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                         options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                         options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                         options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;

                     }
                 );

            services.AddScoped<reservationContext>();
            var dbConnectionString = Configuration.GetValue<string>("DatabaseSettings:ConnectionString");
            services.AddDbContext<playerContext>(opt =>
            opt.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString)));
            services.AddHostedService<PushNotificationHostedService>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();



            //services.AddSwaggerGen();
            //services.AddMassTransit(x =>
            //{
            //    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            //    {
            //        cfg.Host(new Uri(Configuration.GetValue<string>("RabbitMqRootUri")), h =>
            //        {
            //            h.Username(Configuration.GetValue<string>("UserName"));
            //            h.Password(Configuration.GetValue<string>("Password"));
            //        });
            //        cfg.ReceiveEndpoint("paymentQueue", ep =>
            //        {
            //            ep.UseMessageRetry(r => r.Interval(2, 100));
            //        });
            //    }));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reservation.API v1"));
            //app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseCors("ApiCorsPolicy");
            app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
