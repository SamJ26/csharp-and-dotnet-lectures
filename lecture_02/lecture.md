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
    - **Constructors**, Deconstructors
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
    - cammelCase
    - _cammelCase

---

### Constants

- Fields whose value is set at compile time and cannot be changed
- `const` modifier
- Must be initialized with a value!
- User-defined types, including classes, structs, and arrays, cannot be `const`

---

### Methods

- Performs an action in a series of statements
- Can access members of a `class`, `struct` or `record`
- Method signature consist of method **name** and **parameter types**

    ```csharp
    // Classical method
    int Foo(int x) { return x * 2; }

    // Expression-bodied method
    int Foo(int x) => x * 2;

    // Method overloading
    void Foo(int x, int y) => Console.WriteLine(x + y);
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

### Deconstructors

- Opposite of a constructor
- Special method which must be called `Deconstruct` and have one or more `out` parameters,

---

### Finalizers

TODO

---

### Indexers

TODO

---

### Nested Types

TODO

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

## Resources