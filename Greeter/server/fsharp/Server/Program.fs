
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting 
open Microsoft.Extensions.DependencyInjection 

type Startup() = 
    member _.ConfigureServices(services: IServiceCollection) = 
        services.AddGrpc() |> ignore
     
    member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if env.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        app
            .UseRouting()
            .UseEndpoints(fun endpoints ->
                endpoints
                    .MapGrpcService<GreeterService.GreeterService>()
                |> ignore

                endpoints.MapGet("/", fun ctx -> 
                    ctx.Response.WriteAsync("Hello World!"))
                |> ignore
            )
        |> ignore

[<EntryPoint>]
let main args =  
    let app = 
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(fun builder ->
                builder.UseStartup<Startup>() |> ignore 
            )
            .Build() 
    app.Run()
    0  

