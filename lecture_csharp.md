---
marp: true
paginate: true
theme: default
---

# Introduction to C#

---

# What is C#?

- **General-purpose, multi-paradigm, type-safe programming language**
- Created for .NET platform
- Defined by [ECMA-334](https://www.ecma-international.org/publications-and-standards/standards/ecma-334/)
- Syntax is based on C language family
- **Current version is C# 11**
- Designers: Anders Hejlsberg, Mads Torgersen
- _Microsoft Java_

---

# HelloWorld project

- Create new project:
`dotnet new console -n HelloWorld --use-program-main`

- Examine project structure:
`Program.cs`
`HelloWorld.csproj`

---

# HelloWorld project - `Program.cs`

```csharp
namespace HelloWorld;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
```

---

# HelloWorld project - `HelloWorld.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>

```

---

# Syntax

- **Identifiers**
    - Names that programmers choose for entities (variables, methods...)
    ```csharp
    var identifierOfVariable= "value of variable";
    ```
- **Keywords**
    - Names that mean something special to the compiler
    - Can be used as identifier with prefix `@`
    - [List of keywords](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/)
    ```csharp
    int class = 10;     // Illegal
    int @class = 10;    // Legal but ugly
    ```
---

# Syntax

- **Delimiters**
    - Characters used to structure the code
    - **Curly braces** `{}`
        - Creates code blocks
        - Used to impart a scope
    - **Semicolon** `;`
        - Delimits a statement
        - C# allows to write statements on more lines
        ```csharp
        Console.WriteLine
            (1 + 2 + 3 + 4);
        ```
---

# Comments

### Single-line comment
```csharp
// line comment
```

### Multiline comment - DO NOT USE
```csharp
/* I am
   multiline comment */
```

### Documentation
```csharp
/// <summary>
/// This is my awesome docs for some class/method/property...
/// </summary>
```
---

# Data types

- Instruct compiler or interpreter how the programmer intends to use data
- **Value types**
    - Variable directly **contains data**
    - Each variable has its own copy of the data!
- **Reference types**
    - Variable **stores a reference** to the data
    - Two variables can reference the same object!
- **Generic type parameters**
- **Pointer types**

---

# * Reference vs Pointer *

## Pointer
- An **address** which is an offset into the entire virtual address space of the process
- Addresses can be **manipulated mathematically**

## Reference
- Some vague thing that lets you reference an object
- Internal implementation depends on runtime!
- _The CLR actually does implement managed object references as addresses to objects owned by the garbage collector, **but that is an implementation detail**!_

---

# Value types

- Variable directly **contains data**
- Assignment **always copies the instance**
- Value types are identical iff the bit sequences of their data are the same

---

# Value types

- **Simple types**
    - Signed integral: _sbyte, short, int, long_
    - Unsigned integral: _byte, ushort, uint, ulong_
    - Unicode characters: _char_
    - IEEE floating point: _float, double_
    - High-precision decimal: _decimal_
    - Boolean: _bool_
- **Enum types**
    - User-defined set of constant values
- **Struct types**
    - User-defined types that can encapsulate data and related functionality
- **Nullable value types**
    - Extensions of all other value types with a null value

---

# Value types - Simple types

TODO

---

# Value types - Enum types

TODO

---

# Value types - Struct types

TODO

---

# Value types - Nullable value types

TODO

---

# Reference types

- Variable **stores a reference** to the object (data)
- Assignment **copies the reference**, not the object instance
- Reference types are identical iff their locations are the same
- Require separate allocations of memory for the reference and object

---

# Reference types

- **Class types**
    - Ultimate base class of all other types: `object`
    - Unicode strings: `string`
    - User-defined types of the form `class C {...}`
- **Interface types**
    - User-defined types of the form `interface I {...}`
- **Array types**
    - Single-and multi-dimensional, e.g., `int[]` and `int[,]`
- **Delegate types**
    - User-defined types of the form `delegate int D(...)`
- **Generics**
    - Parameterized with other types `MyGenericType<T>`

---

# Reference type - String

- Represents sequence of characters
- Always **IMMUTABLE**
- Literal is denoted by double-quotes: `"string value"`
- Equality operators are defined to compare the values of string objects, not references

---

# Reference type - String

```csharp
string s0 = null;                           // s0 = null
string s1 = string.Empty;                   // s1 = ""

// String concatenation
var s2 = "Hello";
var s3 = "World";
var s4 = s2 + s3;                           // s4 = "HelloWorld"
var s5 = "Five: " + 5;                      // s5 = "Five: 5"

// String comparison
var isEqual1 = Equals(s4, "HelloWorld");    // isEqual1 = true
var isEqual2 = s4 == "HelloWorld";          // isEqual2 = true

// String interpolation
int i = 1;
string s7 = $"{i}. iteration";              // s7 = "1. iteration"

// Accessing individual characters
char c = s7[0]                              // c = '1'                 
```

---

# StringBuilder

- `System.Text.StringBuilder`
- Using the `+` operator repeatedly to build up a string is inefficient
- Represents a mutable string of characters

```csharp
StringBuilder sb = new StringBuilder();
sb.Append("This is the beginning of a sentence, ");         
sb.Replace("the beginning of ", "");
sb.Insert(sb.ToString().IndexOf("a ") + 2, "complete ");
sb.Replace(",", ".");
Console.WriteLine(sb.ToString());       // This is a complete sentence
```

---

# Reference types - Array types

- Represents fixed length data structure of homogeneous items
- The elements are stored in a **contiguous block of memory**
- Initialization according to data type
- **Single-dimensional**
- **Multidimensional**
- **Jagged**

```csharp
int[] array1 = new int[5];
int[] array2 = { 1, 3, 5, 7, 9 };

int[,] multiDimensionalArray1 = new int[2, 3];
int[,] multiDimensionalArray2 = { { 1, 2, 3 }, { 4, 5, 6 } };
```

---

# Default values

| Type                 | Default value                                                    |
|----------------------|------------------------------------------------------------------|
| Reference type       | `null`                                                           |
| Numerical types      | `0`                                                              |
| `bool`               | `false`                                                          |
| `char`               | `\0`                                                             |
| `enum`               | The value produced by the expression `(EType)0`                  |
| `struct`             | The value produced by setting all fields to their default values |
| Nullable value types | `null`                                                           |

---

# * Stack vs Heap *

**Memory in .NET is managed by CLR**
~~Value types are stored on the stack, reference types are stored on the heap~~

## Stack
- LIFO data structure
- Storage for: _local variables, parameters, return values_

## Heap
- Can be viewed as a random jumble of objects
- Managed by **Garbage Collector**
- Storage for: _reference types, values types used in reference types, static variables_

---

# Resources

[Heap vs Stack](https://tooslowexception.com/heap-vs-stack-value-type-vs-reference-type/)
[Reference vs Pointer](https://learn.microsoft.com/en-us/archive/blogs/ericlippert/references-are-not-addresses)
[The Stack Is An Implementation Detail, Part One](https://learn.microsoft.com/en-us/archive/blogs/ericlippert/the-stack-is-an-implementation-detail-part-one)
[The Stack Is An Implementation Detail, Part Two](https://learn.microsoft.com/en-us/archive/blogs/ericlippert/the-stack-is-an-implementation-detail-part-two)