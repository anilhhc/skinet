using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.Configure<ApiBehaviorOptions>(options =>
 {
     options.InvalidModelStateResponseFactory = actioncontext =>
      {
          var errors = actioncontext.ModelState
          .Where(e => e.Value.Errors.Count > 0)
          .SelectMany(x => x.Value.Errors)
          .Select(x => x.ErrorMessage).ToArray();
          var errorResponse = new ApiValidationErrorResponse
          {
              Errors = errors
          };
          return new BadRequestObjectResult(errorResponse);
      };
 });
            return services;
        }
        //public static IApplicationBuilder UseSwaggerDocumentation(this WebApplication app,IWebHostEnvironment a1)
        public static IApplicationBuilder UseSwaggerDocumentation(this WebApplication app)
        {
            //done trial on a1 is development
            // if (a1.IsDevelopment())
            // {
                app.UseSwagger();
                app.UseSwaggerUI();
            // }
            return app;
        }
    }
}