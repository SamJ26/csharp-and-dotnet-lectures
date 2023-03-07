---
marp: true
paginate: true
theme: custom-theme
---

# Dependency Injection, Configuration and Logging

<div class="lectors">
    <hr/>
    Patrik Švikruha
    <br/>
    Samuel Janek
</div>

---

# Dependency Injection

---

## Motivation - `BookFacade` example

```csharp
class BookFacade
{
    private readonly BookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly HelperService _helperService;
    
    public BookFacade()
    {
        _bookRepository = new BookRepository(new DbContext());
        _mapper = new Mapper(new MapperConfig());
        _helperService = new HelperService();
    }
}
```

---

**Disadvantages**:
- `BookFacade` is responsible for creation of its dependencies
- `BookFacade` must configure and create additional dependencies required by its dependencies (e.g. create `MapperConfig` for `Mapper`)
- To replace mapper with different implementation, `BookFacade` must be modified
- Change to constructor signature of some dependency leads to modification of every place in a code, where given service is used
- It's difficult to unit test because dependencies cannot be mocked
- We have to manually handle lifetimes of dependencies

---

## Dependency Injection (DI)

- Technique for achieving **Inversion of Control (IoC)** between classes and their dependencies
- Realized by dependency injection frameworks
- Responsible for resolving dependencies and managing their lifecycle
- _Dependency_ = an object that another object depends on
- _Service_ = an object that provides a service to other objects

---

## Inversion of Control (IoC)

- Class dependencies are managed outside of the class by **IoC container**
- DI framework does the binding and instantiation of dependencies
- Less coupling between classes
- **Dependency inversion** = depending on abstraction instead of implementation

<!--
SPEAKER NOTES:
- IoC:
    - In traditional procedural programming, the code that controls the execution of the program — the main function — instantiates objects, calls methods and even asks the user for input so that the execution can continue and the program can achieve its task
    - With IoC, it is a framework that does the instantiation, method calls and triggers user actions, having full control of the flow and removing this responsibility from the main function
- Dependency Inversion Principle:
    - High-level modules should not depend on low-level modules. Both should depend on abstractions
    - Abstractions should not depend on details. Details should depend on abstractions
-->

---

## `BookFacade` example using DI 👏

```csharp
class BookFacade
{
    private readonly BookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly HelperService _helperService;
    
    public BookFacade(
        BookRepository bookRepository,
        IMapper mapper,
        HelperService helperService)
    {
        _bookRepository = bookRepository;
        _mapper = mapper;
        _helperService = helperService;
    }
}
```

<!--
SPEAKER NOTES:
- Namiesto priameho vytvárania dependencies v BookFacade len pomocou ctoru povieme že ich potrebujeme pre svoju prácu
- DI framework sa potom postará o všetko to, čo sme doteraz museli robiť manuálne v ctore a len nám poskytne vytvorené inštancie
- Čo musíme ale my spraviť, je povedať DI frameworku ako má dané inštancie vyrobiť:
    - Čo všetko do neho chceme zaregistrovať
    - Aké lifetimes chceme použiť
-->

---

## Service lifetimes

- **Transient**
    - Created each time they're requested from the service container
    - This lifetime works best for lightweight, stateless services
- **Scoped**
    - It's creation depends on the scope (e.g. per request in ASP.NET Core)
- **Singleton**
    - One instance for whole application lifetime
    - Singleton services **must be thread safe**
    - Services are disposed when the `ServiceProvider` is disposed

---

## Service registration methods

- Standard methods:
    - `services.AddSingleton<Dep>();`
    - `services.AddSingleton<IDep, Dep>();`
    - `services.AddSingleton<IDep>(sp => new Dep());`
    - `services.AddSingleton<IDep>(new Dep());`
- Other methods:
    - `TryAddSingleton`
    - `TryAddEnumerable`

**Note**: instead of "_Singleton_" can be anything from set { _Singleton_, _Transient_, _Scoped_ }

<!--
- Ak zaregistrujeme service viac krát a resolvujeme ho bez IEnumerable tak sa vyresovluje ten posledný registrovaný
- TryAddXXX vs TryAddEnumerable:
    - TryAddXXX - registeres service iff there isn't already implementation for `IDep` registered
    - TryAddEnumerable - registeres service iff there isn't already implementation of the same type for `IDep` registered
-->

---

## DI in .NET

- Namespace `Microsoft.Extensions.DependencyInjection`
- Dependency Injection utilities are distributed via _NuGet packages_
	- Console apps - utilities **need to be installed**
	- Class libraries - utilities **need to be installed**
	- Web apps - utilities are **available via _generic host_**

---

## MS DI internals

- `ServiceDescriptor`
    - Describes a service with its service type, implementation, and lifetime
- `ServiceCollection`
    - Collection of `ServiceDescritor` objects
    - You can add services to this collection e.g. using `AddSingleton` method
- `ServiceProvider`
    - Defines a mechanism for retrieving a service object
    - Created by calling `BuildServiceProvider` on `ServiceCollection`

---

## DI in console app - DEMO

1. Install `Microsoft.Extensions.DependencyInjection` nuget package
2. Create `ServiceCollection` and configure services
3. Build `ServiceProvider`
4. Create scope
5. Use resolved services 👏

---

## Lifetime mixing

- Not every service can use every type of dependency

    |    Service \ Dependency  | Transient | Scoped | Singleton |
    |:------------------------:|:---------:|:------:|:---------:|
    |         Transient        |    ✅    |   ✅   |    ✅    |
    |          Scoped          |    ✅    |   ✅   |    ✅    |
    |         Singleton        |    🚫    |   🚫   |    ✅    |

- **You should not**:
    - Consume _scoped_ dependency in a _singleton_ service
    - Consume _transient_ dependency in a _singleton_ service

<!--
SPEAKER NOTES:
- V pripade kedy konzumujeme scoped dependency v singleton service a mame povolene scope valiadation tak nam to hodi exception pri pokuse o resolving
- Ak konzumujeme transient dependency v singleton service, tak nam to exception nehodi ale z transient dep sa stava prakticky singleton
- Summary: dlhsie zijuce services by nemali konzumovat dep s kratsim lifetimom (vid captive dependencies)
-->

---

## DI remarks

- Make services small, well-factored, and easily tested
- Create thread-safe singleton services
- Do not call `Dispose` method on disposable service when it is no longer needed
    (**it is the responsibility of IoC container**)
- Avoid using the _service locator pattern_
- Don't register IDisposable instances with a transient lifetime
- Be careful when mixing different lifetimes - avoid [captive dependencies](https://blog.ploeh.dk/2014/06/02/captive-dependency/)
- Do not resolve scoped services outside of the scope - they become singletons

---

## DI frameworks

- [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection)
- [Autofac](https://autofac.org/)
- [Castle Windsor](http://www.castleproject.org/projects/windsor/)
- [Ninject](http://www.ninject.org/)
- And many more...
- You can also create your own 🤔

---

# Configuration

---

## Motivation

- Why do we need it?
	- Logging configuration
	- Secret data (e.g. connection strings)
	- Environment variables
	- Different bahavior in dev and production
	- And many more...
- **Allows us to change the behavior of the app wihtout directly changing the code**

<!--
SPEAKER NOTES:
- Cielom konfiguracie je nemat premenne (konfiguracne) data hardcoded v kode ale mimo v specializovanych suboroch ktore sa daju lahko menit bez nutnosti rekompilacie aplikacie
- V kode nikdy nechceme mat napisane nejake tajne data lebo samotny kod je vacsinou volne dostupny napr. na githube. Preto sa taketo data ukladaju do zvlast suborov ku ktorym sa potom pristupuje cez nejakeho configuration providera.
-->

---

## Configuration providers

- As a _configuration_ can be understood any external file and data that
    **change the behavior of the program**
- Configuration files are read by **configuration providers**:
	- File config provider (`.json`, `.xml`, `.init` files)
	- Environment variable config provider
	- Command-line config provider
	- Memory config provider
- **Configuration is loaded during runtime**

<!--
SPEAKER NOTES:
- V zavisloti od toho odkial konfiguracne data pochadzaju rozlisujeme roznych providerov
- Cele to je navrhnute tak aby sme si mohli napisat aj vlastneho config providera kt. bude napr. pracovat s DB
- Pri nastavovani providerov potom zalezi v akom poradi ich pridame
-->

---

## Config file example - `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DB_CONNECTION_STRING": ""
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

---

## Configuration in .NET

- Namespace `Microsoft.Extensions.Configuration`
- Configuration utilities are distributed via _NuGet packages_
	- Console apps - utilities **need to be installed**
	- Class libraries - utilities **need to be installed**
	- Web apps - utilities are **available via _generic host_**
- **_Configuration pattern_ isn't designed to be programmatically writable**

<!--
SPEAKER NOTES:
- Nie vzdy su utitilities na pracu s konfiguraciou v aplikacii dostupne (niekde ich treba manualne doinstalovat)
- Praca s konfiguraciou v console app sa mierne lisi od web apps (vo web apps s konfiguraciou pracujeme cez generic hosta)
-->

---

## Configuration in console app - DEMO

1. Install following nuget packages:
    - `Microsoft.Extensions.Configuration`
    - `Microsoft.Extensions.Configuration.Binder`
2. According to config files select proper configuration providers
    - `Microsoft.Extensions.Configuration.Json`
3. Make sure that config files are included to the output of the build process
3. Configure providers and build the configuration
4. Use the configuration 👏 

---

## Abstractions

- **Agnostic to their underlying configuration provider**
- `IConfiguration`
    - Represents a set of key/value app config properties
- `IConfigurationRoot`
    - Represents the root of a configuration hierarchy
    - `ConfigurationBuilder.Build` method returns `IConfigurationRoot`
- `IConfigurationSection`
    - Represents a section of app config values
    - `GetSection` method returns `IConfigurationSection`

---

## Options pattern

- Provides **strongly-typed** access to groups of related settings
- Used in combination with **Dependency Injection**
- Provides a mechanism to validate configuration data
- Possible through following interfaces:
    - `IOptions<TOptions>`
    - `IOptionsSnapshot<TOptions>`
    - `IOptionsMonitor<TOptions>`

<!--
SPEAKER NOTES:
- Rozdiel medzi klasickym pouzivanim konfiguracie a options patternom:
    - Na options pattern potrebujeme mat DI
    - Ku konfiguracii v pripade options patternu pristupujeme cez IoC container (constructor injection)
    - Options pattern je strongly-typed
-->

---

### Options pattern and Dependency Injection

**Configure options**

```csharp
services.Configure<LoggerOptions>(configuration.GetSection(nameof(LoggerOptions)));
```

**Use configured options**

```csharp
public class Service
{
    private readonly IOptions<LoggerOptions> _options;

    public Service(IOptions<LoggerOptions> options)
    {
        _options = options;
    }

    public void Foo() => Console.WriteLine(_options.Value);
}
```

---

### `IOptions<TOptions>`

- Registered as **singleton**
- **Does not support reloading** of configuration after app startup

### `IOptionsSnapshot<TOptions>`

- Registered as **scoped**
- **Supports reloading** of configuration values during runtime
    (**options are computed once per scope/request**)
- Designed for use with transient and scoped dependencies

---

### `IOptionsMonitor<TOptions>`

- Registered as **singleton**
- **Supports reloading** of configuration values during runtime
    (**retrieves the latest option values at any time**)
- Has a cache
- Supports change notifications and selective options invalidation
- Especially useful in singleton dependencies

---

### Options pattern demo

---

### Options pattern benefits

- Decouples the lifetime of the underlying option from the DI container
- Enables us to create generic constrians using `IOptions<T>` interface
- The evaluation of the `T` configuration instance is deferred to the accessing of `IOptions<TOptions>.Value`, rather than when it is injected

---

## Configuration remarks

- Make sure that config files are included to the output of the build process
- Use **options pattern** when using dependency injection
- Use **init-only** properties in strongly-typed configuration classes
- **Be careful about what you put in config files**

<!--
SPEAKER NOTES:
- Samozrejme nie vzdy je ten options pattern treba ale pri akejkolvek vacsej aplikacii uz sa velmi hodi
- Niekedy je nutné načítanú konfiguráciu pozmeniť a v takých prípadoch sa init-only props nepoužívajú
-->

---

# Logging

---

## Motivation

- Why do we need it?
    - Debugging informations
    - Application flow tracking
    - Performance infromations
    - Unification of console outputs

---

## Example

```csharp
public class Service
{
    private readonly ILogger<Service> _logger;

    public Service(ILogger<Service> logger)
    {
        _logger = logger;
    }

    public void Foo()
    {
        _logger.LogInformation("Information");
    }
}
```

---

## Logging in .NET

- Namespace `Microsoft.Extensions.Logging`
- Logging utilities are distributed via _NuGet packages_
	- Console apps - utilities **need to be installed**
	- Class libraries - utilities **need to be installed**
	- Web apps - utilities are **available via _generic host_** 

---

## Logging providers

- Available via separate _NuGet packages_
- Built-in providers:
    - Console
    - Debug
    - EventSource
    - EventLog (Windows only)
- Third-party providers:
    - [elmah](https://elmah.io/)
    - [Serilog](https://serilog.net/)
    - [And many more...](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging-providers#third-party-logging-providers)

---

## Abstractions

- `ILogger<TCategoryName>`
    - Used to enable activation of a named `ILogger` from dependency injection
    - `TCategoryName`:
        - The type whose name is used for the logger category name 
        - **The convention is to use the class name** where the logger is used
        - Used to identify the place from which logs are comming
- `ILoggerFactory`
    - Represents a type used to configure and create instances of `ILogger`
    - Knows about all registered logging providers

---

## Log levels

- `LogLevel` enum defines logging severity levels:
    - (0) `Trace` - detailed messages with sensitive info
    - (1) `Debug` - for debugging purposes
    - (2) `Information` - ordinary message
    - (3) `Warning` - abnormal or unexpected events
    - (4) `Error`- errors and exceptions that cannot be handled
    - (5) `Critical` - failures that require immediate attention
    - (6) `None` - no messages should be written

**Note**: 0 is the lowest priority

<!--
SPEAKER NOTES:
- Napr. ak máme min log level na Information, tak sa nám bude logovat všetko od Infromation vyššie (levely 2,3,4,5,6)
-->

---

## Logging in console app - DEMO

1. Install `Microsoft.Extensions.Logging` nuget package
2. Select provider and install specific nuget package
    - `Microsoft.Extensions.Logging.Console`
3. Create and configure logger factory
4. Create logger
5. Use the logger 👏 

---

## Logging configuration

- Usually done via `appsettings.json` file
- The logger API does not support configuration reloading during runtime

    ```json
    "Logging": {                         // Root of logger configuration
        "LogLevel": {                    // Default log levels for all providers according to category
            "Default": "Information",    // Log level for ILogger<??> loggers (any provider)
            "Microsoft": "Warning"       // Log level for ILogger<Microsoft> loggers (any provider)
        },
        "Debug": {                       // Specific config for Debug logging provider
            "LogLevel": {                // Overrides LogLevel section above for this provider
                "Default": "Warning",    // Log level for ILogger<??> loggers (Debug provider)
                "Microsoft": "Trace"     // Log level for ILogger<Microsoft> loggers (Debug provider)
            }
        }
    }
    ```

<!--
SPEAKER NOTES:
- Some configuration providers are capable of reloading configuration, which takes immediate effect on logging configuration
-->

---

## Log message formats

❌ **String interpolation** 

```csharp
_logger.LogInformation($"Getting item {id} at {DateTime.Now}");
```

❌ **Structured logging** - using numbers for placeholders

```csharp
_logger.LogInformation("Getting item {0} at {1}", id, DateTime.Now);
```

✅ **Structured logging** - using names for placeholders

```csharp
_logger.LogInformation("Getting item {Id} at {Time}", id, DateTime.Now);
```

<!--
SPEAKER NOTES:
- In case of proper structured logging:
    - The arguments themselves are passed to the logging system, not just the formatted message template
    - This enables logging providers to store the parameter values as fields for further processing and analysis
-->

---

## Logging remarks

- Logging at the `Trace` or `Debug` levels:
    - Produces a high-volume of detailed log messages
    - Can contain sensitive informations
- If a logging datastore is slow, don't write to it directly
- Use structured logging with named placeholders

<!--
SPEAKER NOTES:
- To control costs and not exceed data storage limits, log Trace and Debug level messages to a high-volume, low-cost data store
- Consider writing the log messages to a fast store initially, then moving them to the slow store later. For example, when logging to SQL Server, don't do so directly in a Log method, since the Log methods are synchronous. Instead, synchronously add log messages to an in-memory queue and have a background worker pull the messages out of the queue to do the asynchronous work of pushing data to SQL Server
-->

---

# Generic Host

---

## Introduction

- Program initialization utility
- Namespace `Microsoft.Extensions.Hosting`
- A **host** is an object that:
    - Controls app startup and graceful shutdown
    - Encapsulates an app's resources:
        - Dependency Injection
        - Configuration
        - Logging
        - `IHostedService` implementations

---

## Example usage

```csharp
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder => { })
    .ConfigureServices(services => 
    {
        services.AddHostedService<Worker>();
    })
    .ConfigureHostConfiguration(builder => { })
    .ConfigureAppConfiguration(builder => { })
    .Build();

host.Run();
```

---

### `Host.CreateDefaultBuilder()`

- **Sets the content root** to the path returned by `GetCurrentDirectory()`
- **Loads host configuration** from env variables and command-line args
- **Loads app configuration** from:
    - `appsettings.json` and `appsettings.{Environment}.json`
    - Secret Manager when the app runs in the `Development` env
    - Environment variables and command-line args
- **Adds logging providers**: Console, Debug, EventSource, EventLog (Windows only)
- **Enables scope validation and dependency validation** when the env is `Development`

<!--
SPEAKER NOTES:
- The IHostEnvironment.ContentRootPath property represents the default directory where appsettings.json and other content files are loaded in a hosted application
-->

---

### `ConfigureHostConfiguration`

- Used to initialize the `IHostEnvironment` for use later in the build process

### `ConfigureAppConfiguration`

- Sets up the configuration for the remainder of the build process and application
- The configuration passed in is the host's configuration built from calls to `ConfigureHostConfiguration`

**Notes**:
- Both methods can be called multiple times and the results will be additive
- **Main difference is where methods search for configuration files**

---

## `BackgroundService`

- Implements `IHostedService` interface
- Base class for implementing a long running background tasks
- Easier to use than creating your own implementation of `IHostedService`
- Provides better exception logging
- Example of the [Template Method Pattern](https://refactoring.guru/design-patterns/template-method)

---

## Generic host in console app - DEMO

- `dotnet new worker -n <project_name> --use-program-main`
- Command creates console app with:
    - Configured generic host
    - `appsettings.json` and `appsettings.Development.json` config files

---

## Generic host remarks

- Avoid performing long, blocking initialization work in `ExecuteAsync` method - no further services are started until `ExecuteAsync` becomes asynchronous
- By default, no scope is created for a hosted service - if you need to use scoped services, you need to create scope manually
- **By default, unhandled exception in some hosted services will stop the Host**
- `AddHostedService<Worker>()` adds a **singleton instance** of `Worker`
- Services are:
    - Started serially in order they are registered
    - Stopped serially in the reverse order of how they were registered

<!--
SPEAKER NOTES:
- ExecuteAsync method sa vykonava synchronne pokym nenarazi na await a teda ak je ta synchronna cast dlha/pomala, tak to bude blokovat spustenie ostatnych hosted services
- Exceptions v hosted services:
    - Console apps a workers sa typicky deployuju samostatne a teda ich zastavenie kvoli exception az tak nevadi
    - V pripade ASP.NET Core app ale hosted services bezia na pozadi nejakej webovej sluzby a exception by zhodila celu appku
    - To ci sa ma zastavit cely Host kvoli jednej exception sa da nakonfigurovat cez BackgroundServiceExceptionBehavior
-->

---

## Thank you for your attention :)

---

## Resources

- [Microsoft documentation](https://learn.microsoft.com/en-us/docs/)
- [C# in Nutshell](https://www.amazon.com/gp/product/1098121953?ie=UTF8&tag=cinanu-20&linkCode=as2&camp=1789&creative=9325&creativeASIN=1098121953)
- [VUT FIT IW5 lecture 02 - Michal Mrnuštík](https://github.com/nesfit/IW5/tree/main/Lectures/Lecture_02)
- [Dependency Injection vs Dependency Inversion vs IoC](https://medium.com/ssense-tech/dependency-injection-vs-dependency-inversion-vs-inversion-of-control-lets-set-the-record-straight-5dc818dc32d1)
