var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddInfrastructure(builder.Configuration);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxConcurrentConnections = 100;
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
});

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

//app.UseCors("CorsPolicy");
//app.MapHub<HubClient>("/hubClient");

app.MapControllers();

app.Run();
