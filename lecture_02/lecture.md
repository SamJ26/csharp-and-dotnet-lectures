---
marp: true
paginate: true
theme: custom-theme
---

<style>
img[alt~="center"] {
  display: block;
  margin: 0 auto;
}
</style>

# Introduction to OOP

---

## What is OOP?

- **Programming paradigm** based on **objects**
- **Object** interconnects data and behavior together:
    - **Behavior** is described by procedures and functions, both called methods
    - **Data** is stored in object's member variable (field)
- Often compared with functional programming (FP)
- First appearance at MIT in 1950-1960

---

## Class vs Object

**Class**
- **Template** (blueprint) for object creation
- Abstraction of entity from the real world
- Logical entity

**Object**
- **Instance of some class**
- A concrete entity from the real world
- Physical entity

---

![center](./images/class-vs-object.png)

<a href="https://www.flaticon.com/free-icons/car" title="car icons">Car icons created by Freepik - Flaticon</a>

---

## Pillars of OOP

- Abstraction
- Encapsulation
- Inheritance
- Polymorphism

---

### Abstraction

- Two meanings:
    - **Object is only a blackbox with some state and API**
        - We don't need to know internal details about the object to use it
    - **Object is an abstraction of a real-world entity**
        - It's a model which represents all details relevant to given context

---

### Encapsulation

- **Ability of an object to hide parts of its state and behaviors from other objects**
- Related to abstraction - object is a **blackbox**
- Improves modularity
- At the code level it is achieved using _access modifiers_

---

### Inheritance

- **Ability to build new classes on top of existing ones**
- Allows us to share functionality and data
- _SubClass_ vs _SuperClass_
- A frequent stumbling-block
    - Types of relationships:
        - **is** - dog _is_ an animal
        - **has** - dog _has_ a leg
    - _Composition over inheritance_

---

### Polymorphism

- **Ability to access objects of different types through the same interface**
- Ability of an object to “pretend” to be something else
- Related to inheritance
- Types:
    - **Ad hoc polymorphism** - function overloading
    - **Parametric polymorphism** - generics in OOP
    - **Subtyping**

---

## Pros and Cons of OOP

**Pros**:
- Structured code and productivity
- Analogy with real world
- Logical conhesion
- Code maintainability
- Black box

**Cons**:
- Wrong design leads to huge complexity, high coupling and low cohesion
- Black box

---

# OOP in C#

---

## Class

- The most common kind of reference type
- Members:
    - **Fields**, **Properties**, **Constants**
    - **Methods**
    - **Constructors**
    - Operators
    - Indexers
    - Finalizers
    - Nested Types
    - Events

---

### Access Modifiers

- `public` - accessible everywhere
- `private` - accessible only in the same class or struct
- `protected` - accessible only in the same class or struct or in derived class
- `internal` - accessible in the same assembly, but not from another assemblies
- `protected internal` - accessible in the assembly in which it's declared, or from within a derived class in another assembly (_internal_ **OR** _protected_)
- `private protected` - accessible by types derived from the class that are declared within its containing assembly (_internal_ **AND** _protected_)

[Summary Table](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers#summary-table)

---

### Fields

- Variable of any type that is declared directly in a class or struct
- Access to data via _methods_, _properties_ or _indexers_
- Initialization is **optional**:
    - Initialized before the constructor call (with default or predefined value)
    - Constructor can override initial value
- Naming conventions:
    - cammelCase or _cammelCase for `private` fields
    - PascalCase for `protected` fields

---

### Constants

- Fields whose value is set at compile time and cannot be changed
- `const` modifier
- Must be initialized with a value!
- User-defined types, including classes, structs, and arrays, cannot be `const`
- Naming conventions: PascalCase

---

```csharp
class MyClass
{
    private string _name;
    protected int Number = 8;
    
    private const string Constant = "I am constant";
}
```

---

### Methods

- Performs an action in a series of statements
- Can access members of a `class`, `struct` or `record`
- Method signature consist of method **name** and **parameter types**
- Parameters vs Arguments:
    - **Parameters** - method definition specifies the names and types of any parameters that are required
    - **Argument** - concrete values provided by calling code for each parameter when method is called

---

```csharp
class MyClass
{
    private string _name;
    private const string Constant = "I am constant";

    public void SayHi() => Console.Write($"Hi, I am {_name}");

    public int DivideByTwo(int num)
    {
        if (num > 0)
        {
            return num / 2;
        }
        return 0;
    }
}

```

---

#### Local methods

- Method within another method
- Visible only to the enclosing method
- Can access the local variables and parameters of the enclosing method

    ```csharp
    void WriteCubes()
    {
        const int n = 2;

        Console.WriteLine(Compute(3));
        Console.WriteLine(Compute(4));
        Console.WriteLine(Compute(5));

        int Compute(int value) => value * n;
    }
    ```

---

### Properties

- Flexible mechanism to read, write, or compute the value of a private field
- Simple **encapsulation** mechanism
- Property accessors:
    - `get` - return the property value
    - `set` - assign a new value
    - `init` - assign a new value only during object construction
- Types of properties:
    - Read-write
    - Read-only
    - Write-only
    - Init-only

---

```csharp
class MyClass
{
    public int ReadWriteProp { get; set; }
    public int ReadOnlyProp { get; }
    public int ComputedProp => ReadWriteProp * 2;
    public int InitOnlyProp { get; init; }
}

var o = new MyClass() { InitOnlyProp = 5 };

var v0 = o.InitOnlyProp;        // Ok
o.InitOnlyProp = 5;             // Error

var v1 = o.ReadWriteProp;       // Ok
o.ReadWriteProp = 5;            // Ok

var v2 = o.ReadOnlyProp;        // Ok
o.ReadOnlyProp = 5;             // Error - property has no setter

var v3 = o.ComputedProp;        // Ok
o.ComputedProp = 5;             // Error - property has no setter
```

---

#### Properties under the hood

- _Backing field_ = private field which holds value of property
- Accessors internally compile to methods `get_XXX` and `set_XXX`

    ```csharp
    class MyClass
    {
        // public int Prop { get; set; }
        private int _propBackingField;
        public int Prop
        {
            get => _propBackingField;
            set => _propBackingField = value;
        }
    }

    var o = new MyClass();
    var val = o.Prop;
    o.Prop = 5;
    ```

---

### Constructors

- Contains initialization code on a `class`, `struct` or `record`
- Special method with the same name as enclosing type and without return type
- Support expression-bodied notation
- Class can have multiple constructors with different parameters
- **Implicit public parameterless constructor**
    - Generated by compiler automatically
    - If and only if you do not define any other constructor

---

```csharp
class Car
{
    public string Name { get; set; }
    public string Owner { get; set; }
    
    public Car(string name) => Name = name;

    public Car(string name, string owner)
    {
        Name = name;
        Owner = owner;
    }
}

var car1 = new Car("Porsche");
var car2 = new Car("Trabant", "Patrik Švikruha");
var car3 = new Car();   // Error - ????
```

---

### Indexers

-  Allow instances of a `class` or `struct` to be indexed just like arrays
- Indexers resemble properties except that their accessors take parameters
- Read-only indexer by omitting `get` accessor
- Indexers can be overloaded
- Indexers can have more than one formal parameter
- **Be sure to incorporate some type of error-handling on invalid index value**

---

```csharp
public class TempRecord
{
    float[] temps = new float[10]
    {
        56.2F, 56.7F, 56.5F, 56.9F, 58.8F,
        61.3F, 65.9F, 62.1F, 59.2F, 57.5F
    };

    public int Length => temps.Length;
    
    // Indexer
    public float this[int index]
    {
        get => temps[index];
        set => temps[index] = value;
    }
}
```

---

### Nested Types

- A type defined within a `class` or `struct`
- They are accessible only from their containing type
- A nested type has access to all of the members of its containing type

---

```csharp
public class Container
{
    private const string Name = "Container";
    
    public class Nested
    {
        private Container _parent;
        
        public Nested(Container parent)
        {
            Console.WriteLine(Name);
            this._parent = parent;
        }
    }
}
```

---

## `this` keyword

- Refers to the **current instance** of the class

    ```csharp
    public class Employee
    {
        private string name;
        private int age;

        public Employee(string name, int age)
        {
            this.name = name;
            this.age = age;
        }
    }
    ```

---

## `static` modifier

- Class
- Members
- Constructor

---

### Static class

- **Cannot be instantiated**
- Class members are always accessed by the class name
- Contains only static members
- Is sealed - cannot be inherited
- Cannot contain instance constructors

---

```csharp
static class Calculator
{
    public static double Add(double num1, double num2) => num1 + num2;

    public static double Subtract(double num1, double num2) => num1 - num2;
}

var calculator = new Calculator();  // Error - cannot create an instance of the static class

var res1 = Calculator.Add(5, 4);
var res2 = Calculator.Subtract(5, 4);
```

---

### Static members

- A non-static class can contain static methods, fields, properties, or events
- Callable on a class even when no instance of the class has been created
- Always accessed by the class name
- Only one copy of a static member exists
- A `const` field is essentially static in its behavior
- Static method call is faster than non-static method call
(`call` vs `callvirt` IL instructions)

---

```csharp
class MyClass
{
    public uint Id { get; }
    
    private static uint _instanceCounter = 0;

    public MyClass()
    {
        _instanceCounter++;
        Id = _instanceCounter;
    }

    public static void SayHi() => Console.Write("Hi");
}

var firstInstance = new MyClass();
Console.WriteLine(firstInstance.Id);    // Output: 1

var secondInstance = new MyClass();
Console.WriteLine(secondInstance.Id);   // Output: 2

MyClass.SayHi();

firstInstance.SayHi();      // Error - Cannot access static method in non-static context
```

---

### Static constructor

- **Used to initialize any static data**, or to perform a particular action only once
- Is called automatically before the first instance is created or any static members are referenced
- A static constructor doesn't take access modifiers or have parameters
- A class or struct can only have one static constructor

---

```csharp
class MyClass
{
    public uint Id { get; }
    
    private static uint _instanceCounter;

    public MyClass() => Id = ++_instanceCounter;

    static MyClass()
    {
        _instanceCounter = 10;
        // Id = 10; Error - cannot access non-static property in static context 
    }
}

var firstInstance = new MyClass();
Console.WriteLine(firstInstance.Id);    // Output: 11

var secondInstance = new MyClass();
Console.WriteLine(secondInstance.Id);   // Output: 12
```

---

## Inheritance in C#

- **Inheritance = ability to build new classes on top of existing ones**
- Used to express an "**is a**" relationship
- A class can only inherit from a single class
- **Structs do not support inheritance**
- Inheritance is transitive
- What is not inherited by derived class:
    - Static constructors
    - Instance constructors
    - Finalizers

---

```csharp
class Publication
{
    public string Publisher { get; set; }
    public string Title { get; set; }
    public uint Pages { get; set; }
}

class Book : Publication
{
    public string ISBN { get; set; }
}

class Article : Publication
{
    public string WebSite { get; set; }
}

var book = new Book();
var article = new Article();

Console.WriteLine(book.Title);
Console.WriteLine(article.Title);
```

---

### `base` keyword

- Used to:
    - Access members of the base class from within a derived class
    - Specify which base-class constructor should be called
- Cannot be used in static method

---

```csharp
class Publication
{
    public string Title { get; set; }

    public Publication(string title) => Title = title;
}

class Book : Publication
{
    public string ISBN { get; set; }

    public Book(string ISBN, string title) : base(title)
    {
        this.ISBN = ISBN;
    }
}
```

---

### Composition vs Inheritance

TODO

---

## `System.Object`

- All types in the .NET type system **implicitly** inherit from `System.Object`
- The common functionality of `Object` is available to any type
- Any type can be upcast to object
- The most important methods:
    - `Equals(Object)`
    - `GetHashCode()`
    - `GetType()`
    - `ToString()`

---

TODO - example usage of System.Object methods

---

## Thank you for your attention :)

---

## Required properties (C# 11)

- Adding `required` to property forces client code to initialize given property
- Replacement of simple constructors

    ```csharp
    public class SaleItem
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }
    }

    var item = new SaleItem
    {
        Name = "Shoes",
        Price = 19.95m
    };
    Console.WriteLine($"{item.Name}: sells for {item.Price:C2}");
    ```

---

## Finalizers

- Used to perform any final clean-up when a class instance is being collected by GC
- Important remarks:
    - A class can only have one finalizer
    - Finalizers cannot be called => they are invoked automatically
    - A finalizer does not take modifiers or have parameters
- Better alternative is to use `IDisposable`
-  **.NET Core and later versions don't call finalizers as part of application termination**
- _Finalizers are somewhat like lawyers - although there are cases in which you really need them, in general you don’t want to use them unless absolutely necessary_
[Joseph Albahari - C# in Nutshell](https://www.albahari.com/nutshell/)

---

```csharp
class Car
{
    public string Name { get; set; }

    // Finalizer
    ~Car()
    {
        // Cleanup statements...
    }
}
```

---

## Resources