## Context
1. [.Net Core Serilog – Basic](https://github.com/rtodosic/Serilog01/)
2. [.Net Core Serilog – Configuration](https://github.com/rtodosic/Serilog02/)
3. [.Net Core Serilog - Structured JSON output](https://github.com/rtodosic/Serilog03/)
4. [.Net Core Serilog - Enrichers](https://github.com/rtodosic/Serilog04/)
5. .Net Core Serilog - Custom JSON output
6. [.Net Core Serilog - Adding Sinks](https://github.com/rtodosic/Serilog06/)

This is part 5 of 6.

## 5. .Net Core Serilog – Custom JSON output

Serilog’s “CompactJsonFormatter” may not be what you want. In cases where you need to more control on field names and data, you can use the “Expression Template”.

1.	Add “Serilog.Expression” NuGet package to your project.
   ![Image alt text](Images/NuGet-Serilog-Expressions.png?raw=true)
   
2.	In Program.cs, add “using Serilog.Templates” to the top of the file and change the  CreateHostBuilder() method to the following:
  ```C#
  public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.UseStartup<Startup>();
          })
      .UseSerilog((hostingContect, loggerConfiguration) => loggerConfiguration
          .ReadFrom.Configuration(hostingContect.Configuration)
          .Enrich.FromLogContext()
          .Enrich.WithMachineName()
          .Enrich.WithProperty("Assembly", typeof(Program).Assembly.GetName().Name)
          .WriteTo.Console(new ExpressionTemplate("{ {app_timestamp:@t, message:@m, rendering:@r, level:if @l = 'Debug' then 'DEBUG' else if @l = 'Warning' then 'WARN' else if @l = 'Error' then 'ERR' else if @l = 'Fatal' then 'FTL' else @l, exception:@x, ..@p} }\n"))
      );
  ```
3.	Run the application and notice that the names of the fields have changed and the level is different as well. 
   ![Image alt text](Images/Console-Serilog-Expression.png?raw=true)
 
[Serilog Expressions repo for documentation](https://github.com/serilog/serilog-expressions)
