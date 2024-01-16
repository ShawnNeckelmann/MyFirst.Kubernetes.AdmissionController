using k8s;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.UseHttpsRedirection();

app.MapGet("/podlist", () =>
    {
        var config = KubernetesClientConfiguration.BuildDefaultConfig();
        IKubernetes client = new Kubernetes(config);

        var list = client.CoreV1.ListNamespacedPod("kube-system");
        var items = list.Items.ToList();

        return items.Count > 0 ? Results.Ok(items.First()) : Results.NoContent();
    })
    .WithName("GetPodList")
    .WithOpenApi();

app.Run();