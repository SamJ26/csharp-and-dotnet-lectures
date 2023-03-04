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
- Change to constructor signature of some dependency leads to modification of every place in a code, where given dependency is used
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
    - With IoC, it is a framework that does the instantiation, method calls and triggers user actions, having full control of the flow and removing this responsibility from the main function, and by consequence the application
- Dependency Inversion Principle:
    - High-level modules should not depend on low-level modules. Both should depend on abstractions
    - Abstractions should not depend on details. Details should depend on abstractions
-->

---

## `BookFacade` example using DI

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

---

TODO - DI internals (jednotlive triedy)

ServiceProvider
ServiceDescriptor

---

## Service lifetimes

- **Transient**
    - Created each time they're requested from the service container
    - This lifetime works best for lightweight, stateless services
- **Scoped**
    - It's creation depends on the scope (e.g. per request in ASP.NET Core)
- **Singleton**
    - One instance for whole application
    - Singleton services **must be thread safe**
    - Services are disposed when the `ServiceProvider` is disposed

---

## Service registration

TODO

- AddXXX vs TryAddXXX vs TryAddEnumerable
- Zaregistrovanie viac krat jedneho servicu vyresolvuje ten posledny
- Pozor na miesanie lifetimes

## Service resolving

TODO

Resolving nezaregistrovaneho servicu
IEnumerable<TService> vs TService

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

- As a _configuration_ can be understood any file that
    **changes the behavior of the program**
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

## Configuration in console app

1. Install following nuget packages:
    - `Microsoft.Extensions.Configuration`
    - `Microsoft.Extensions.Configuration.Binder`
2. According to config files select proper configuration providers
    - `Microsoft.Extensions.Configuration.Json`
3. Make sure that config files are included to the output of the build process
3. Configure providers and build the configuration
4. Use the configuration 👏 

<!--
SPEAKER NOTES:
- Ukazat tento proces na predpripravenej console app
-->

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

TODO

---

## Remarks

- Make sure that config files are included to the output of the build process
- Use **options pattern** when using dependency injection
- **Prefer records over classes** when binding configuration to strongly typed objects
- **Be careful about what you put in config files**

<!--
SPEAKER NOTES:
- Samozrejme nie vzdy je ten options pattern treba ale pri akejkolvek vacsej aplikacii uz sa velmi hodi
-->

---

# Logging

---

# Generic Host

---

## Thank you for your attention :)

---

## Resources

- [Microsoft documentation](https://learn.microsoft.com/en-us/docs/)
- [C# in Nutshell](https://www.amazon.com/gp/product/1098121953?ie=UTF8&tag=cinanu-20&linkCode=as2&camp=1789&creative=9325&creativeASIN=1098121953)
- [VUT FIT IW5 lecture 02 - Michal Mrnuštík](https://github.com/nesfit/IW5/tree/main/Lectures/Lecture_02)
- [Dependency Injection vs Dependency Inversion vs IoC](https://medium.com/ssense-tech/dependency-injection-vs-dependency-inversion-vs-inversion-of-control-lets-set-the-record-straight-5dc818dc32d1)
