using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITenantRedisCacheProvider, TenantRedisCacheProvider>();
//builder.Services.AddSingleton<IAzureKeyVaultService, AzureKeyVaultService>();
//builder.Services.AddSingleton(new SecretClient(new Uri(builder.Configuration["KeyVault:Uri"]), new DefaultAzureCredential()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<TenantRedisCacheMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
