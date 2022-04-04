using API.Extensions;
using API.Helpers;
using API.MiddleWare;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddScoped<IProductRepository, ProductRepository>();
// builder.Services.AddScoped(typeof(IGenericRepository< >),typeof(GenericRepository< >));
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers();
//to reduce startup class we have created above class extension
builder.Services.AddApplicationServices();
// builder.Services.Configure<ApiBehaviorOptions>(options=>
// {
//     options.InvalidModelStateResponseFactory=actioncontext=>
//     {
//         var errors=actioncontext.ModelState
//         .Where(e=>e.Value.Errors.Count>0)
//         .SelectMany(x=>x.Value.Errors)
//         .Select(x=>x.ErrorMessage).ToArray();
//         var errorResponse=new ApiValidationErrorResponse
//         {
// Errors=errors
//         };
//         return new BadRequestObjectResult(errorResponse);
//     };
// });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddDbContext<StoreContext>(x => x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(opt=>
{
    opt.AddPolicy("CorsPolicy",policy=>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();
        await StoreContextSeed.SeedAsync(context,loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occured");
    }
}
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
// {   
    // app.UseSwagger();
    // app.UseSwaggerUI();
// }
app.UseSwaggerDocumentation();
app.UseStatusCodePagesWithReExecute("/error/{0}");

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.UseRouting();
app.UseStaticFiles();

app.MapControllers();

app.Run();
