var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add application services
builder.Services.AddApplicationService();

// Add identity handler services and configure identity services
builder.Services.AddSwaggerExplorer()
    .InjectDbContext(builder.Configuration)
    .AddIdentityHandlersAndStores()
    .ConfigureIdentityOptions()
    .AddIdentityAuth(builder.Configuration);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddResponseCaching();

var app = builder.Build();
app.UseResponseCaching();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

    await ApplicationSeedConfig.SeedAsync(context, mediator);
}

// Enable forwarded headers for IP Address
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Enables serving static files from wwwroot
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerExplorer();
}

// Configuration CORS
app.ConfigureCors(builder.Configuration)
   .AddIdentityAuthMiddlewares();

app.UseHttpsRedirection();

// Use exception handler 
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.MapGroup("/api");

app.Run();