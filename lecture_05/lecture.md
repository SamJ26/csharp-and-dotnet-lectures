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

# Logging

---

# Generic Host

---

## Thank you for your attention :)

---

## Resources

- [C# in Nutshell](https://www.amazon.com/gp/product/1098121953?ie=UTF8&tag=cinanu-20&linkCode=as2&camp=1789&creative=9325&creativeASIN=1098121953)
- [VUT FIT ICS slides](https://github.com/nesfit/ICS/tree/master/Lectures)
- [Dependency Injection vs Dependency Inversion vs IoC](https://medium.com/ssense-tech/dependency-injection-vs-dependency-inversion-vs-inversion-of-control-lets-set-the-record-straight-5dc818dc32d1)