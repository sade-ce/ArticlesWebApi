﻿using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Common.WebApiCore.Setup
{
    public static class SwaggerConfig
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Articles WebAPI",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Email = "altun.mursalov00@gmail.com",
                        Name = "Altun Mursalov",
                        Url = new Uri("https://t.me/altun_mursalov")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "Apache-2.0",
                        Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0")
                    },
                    Description = "This is the server side application for <span style=\"color: #1bd63a;\">Articles</span> project.<br><br>" +
                                  "To make requests to protected routes you should first login through Auth/Login endpoint.<br>" +
                                  "If you don't have an account, create one using Auth/Sign-Up endpoint.<br>" +
                                  "Once you successfully logged, copy your <b>AccessToken</b> and pass to the Authorize form,<br>" +
                                  "that you can find in the right top side of this page.<br>" +
                                  "For example: Bearer <b>AccessToken</b>"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });
        }

        public static void UseConfiguredSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "v1");
                c.DefaultModelsExpandDepth(-1);
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });
        }
    }
}