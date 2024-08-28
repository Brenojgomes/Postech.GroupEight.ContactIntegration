using Postech.GroupEight.ContactIntegration.Worker;
using Postech.GroupEight.ContactIntegration.Infra;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
