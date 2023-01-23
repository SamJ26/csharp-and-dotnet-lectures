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
    - **Constructors**
    - **Methods**
    - Operators
    - Indexers
    - Finalizers
    - Nested Types
    - Events

---

```csharp
public class MyClass
{
    // Private field

    // Public property
}
```

---

### Access Modifiers

- `public` - accessible everywhere
- `private` - accessible only in the same class or struct
- `protected` - accessible only in the same class or struct or in derived class
- `internal` - accessible in the same assembly, but not from another assemblies
- `protected internal` - accessible in the assembly in which it's declared, or from within a derived class in another assembly (_internal_ **OR** _protected_)
- `private protected` - accessible by types derived from the class that are declared within its containing assembly (_internal_ **AND** _protected_)

---

### Summary table

| Caller's location                      | `public` | `protected internal` | `protected` | `internal` | `private protected` | `private` |
| -------------------------------------- | :------: | :------------------: | :---------: | :--------: | :-----------------: | :-------: |
| The class                       |    ✔️️     |          ✔️           |      ✔️      |     ✔️      |          ✔️          |     ✔️     |
| Derived class (SA)          |    ✔️     |          ✔️           |      ✔️      |     ✔️      |          ✔️          |     ❌     |
| Non-derived class (SA)      |    ✔️     |          ✔️           |      ❌      |     ✔️      |          ❌          |     ❌     |
| Derived class (DA)     |    ✔️     |          ✔️           |      ✔️      |     ❌      |          ❌          |     ❌     |
| Non-derived class (DA) |    ✔️     |          ❌           |      ❌      |     ❌      |          ❌          |     ❌     |

---

### Fields

- Variable of any type that is declared directly in a class or struct
- _private_ or _protected_ accessibility
- Access to data via _methods_, _properties_ or _indexers_
- Naming conventions:
    - cammelCase
    - _cammelCase


---

## Thank you for your attention :)

---

## Resources