---
marp: true
paginate: true
theme: custom-theme
---

# C# and .NET part 3

---

## Method parameters

- Arguments can be passed:
    - **by value** = passing a **copy of the variable** to the method
    - **by reference** = passing **access to the variable** to the method
- **By default, all types are passed by value**
- Use `ref` keyword to pass variable by reference

---

### Passing value types by value

```csharp
class Program
{
    static void Change(int number)
    {
        number = number * 2;    // This is local change
    }
    
    static void Main(string[] args)
    {
        int num = 1;
        // num: 1
        Change(num);
        // num: 1
    }
}
```

---

### Passing reference types by value

```csharp
class Program
{
    static void Change(int[] arr)
    {
        arr[0] = 888;                    // This change affects arr in Main
        arr = new int[3] { 1, 1, 1 };    // This change is local
    }
    
    static void Main(string[] args)
    {
        int[] arr = { 1, 2, 3 };
        // arr: { 1, 2, 3 }
        Change(arr);
        // arr: { 888, 2, 3 }
    }
}
```

---

### Passing value types by reference

```csharp
class Program
{
    static void Change(ref int number)
    {
        number = number * 2;    // This change affects num in Main
    }
    
    static void Main(string[] args)
    {
        int num = 1;
        // num: 1
        Change(ref num);
        // num: 2
    }
}
```

---

### Passing reference types by reference

```csharp
class Program
{
    static void Change(ref int[] arr)
    {
        arr[0] = 888;                    // This change affects arr in Main
        arr = new int[3] { 1, 1, 1 };    // This change affects arr in Main
    }
    
    static void Main(string[] args)
    {
        int[] arr = { 1, 2, 3 };
        // arr: { 1, 2, 3 }
        Change(ref arr);
        // arr: { 1, 1, 1 }
    }
}
```

---

### `out` parameter modifier

- Argument is **passed by reference**
- Arguments do not have to be initialized before being passed
- The method must assign a value to given parameter before the method returns
- Used to return multiple values from single method

---

```csharp
static bool TryParse(string text, out int number)
{
    foreach (var ch in text)
    {
        if (ch < 48 || ch > 57)
        {
            number = 0;
            return false;
        }
    }
    number = int.Parse(text);
    return true;
}
```

```csharp
var parseResult = TryParse("12a3456", out int result);
if (parseResult == false)
    Console.WriteLine("Parsing failed");
else
    Console.WriteLine(result);
```

---

### `in` parameter modifier

- Argument is **passed by reference** and **cannot be modified** in method
- Variables passed as _in_ arguments must be initialized
- Potential performance optimization when using with **readonly** data structures

    ```csharp
    static void Foo(in double number)
    {
        number = 8;     // Error
    }

    Foo(1.5);
    ```

---

### `params` keyword

- Method parameter that takes a variable number of arguments
- No additional parameters are permitted after the `params` keyword
- Only one `params` keyword is permitted in a method declaration
- Given parameter must be of type **single-dimensional array**

    ```csharp
    static int Sum(params int[] numbers)
    {
        var sum = 0;
        foreach (var num in numbers) { sum += num; }
        return sum;
    }

    Sum(1,2,3,4,5,6);
    ```

---

## Tuples

```csharp
// Tuple with default names
var t1 = (111, "text");
Console.WriteLine($"{t1.Item1} {t1.Item2}");

// Tuple with field names
(int Number, string Text) t2 = (111, "text");
Console.WriteLine($"{t2.Number} {t2.Text}");

// Tuples equality
Console.WriteLine(t1 == t2);    // True

// Deconstruction
(int n, string t) = t2;
Console.WriteLine(n);   // 111
Console.WriteLine(t);   // "text"
```

---

- Mutable **value types**
- Provides concise syntax to group multiple data in a lightweight data structure
- Support the `==` and `!=` operators
- Syntactic sugar for `System.ValueTuple` struct
- Tuple elements are public
- For immutable tuples use reference type `System.Tuple`

---

#### Deconstructors

- Assigns fields to a set of variables
- A deconstruction method must be called `Deconstruct` and have one or more _out_ parameters
- A deconstruction method can be implemented as _extension method_

---

```csharp
class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public void Deconstruct(out string firstName, out string lastName)
    {
        firstName = FirstName;
        lastName = LastName;
    }
}
```

```csharp
var person = new Person() { FirstName = "Samuel", LastName = "Janek" };
(string firstName, string lastName) = person;
Console.WriteLine(firstName);
Console.WriteLine(lastName);
```

---

## Exceptions

- Events that require execution of code outside the normal flow of control
- Two types of exceptions:
    - _Hardware exceptions_ - initiated by CPU
    - _Software exceptions_ - initiated by apps or OS
- C# has [Structured Exception Handling](https://learn.microsoft.com/en-us/windows/win32/debug/structured-exception-handling) - handles HW and SW exceptions identically
- All exceptions derive from `System.Exception`

---

### Keywords

- `try`
    - Code block subject to error-handling or cleanup code
    - Must be followed by:
        - `catch` block
        - `finally` block
        - or both
- `catch`
    - Executes when an error occurs in the `try` block
    - Has access to thrown exception
- `finally`
    - Executes always

---

### Syntax

```csharp
try
{
 ... // exception may get thrown within execution of this block
}
catch(ExceptionA ex)
{
 ... // handle exception of type ExceptionA
}
catch(ExceptionB ex)
{
 ... // handle exception of type ExceptionB
}
finally
{
 ... // cleanup code - unmanaged resources
}
```

---

### Example - throwing exceptions

```csharp
class Test
{
    static void Display(string name)
    {
        if(name == null)
            throw new ArgumentNullException(nameof(name));
        Console.WriteLine(name);
    }

    static void Main()
    {
        try { Display(null); }
        catch(ArgumentNullException ex)
        {
            Console.WriteLine("Caught the exception");
        }
    }
}
```

---

### Example - finally block

```csharp
static void ReadFile()
{
    StreamReader reader = null; // In System.IO namespace
    try
    {
        reader = File.OpenText("file.txt");
        if(reader.EndOfStream) return;
        Console.WriteLine(reader.ReadToEnd());
    }
    finally
    {
        if(reader != null) reader.Dispose();
    }
}
```

---

### Key members of `System.Exception`

- `StackTrace`
    - A string representing all the methods that are called from the origin of the exception to the catch block
- `Message`
    - A string with a description of the error
- `InnerException`
    - The inner exception (if any) that caused the outer exception
    - InnerException may have another InnerException

---

### Important remarks

- Don't catch an exception unless you can handle it
- Don't throw `System.Exception` - create specialized exceptions
- Use "_Exception_" suffix for the user defined exceptions
- Rethrowing exceptions:

TODO

---

## Generics

- **Code template that contains placeholder (_type parameter_) for specific type**
- _Type parameters_ are substituted with actual type
- Similar to C++ templates
- Supported at runtime level

---

### What problem do they solve? 🤔

```csharp
public class Stack
{
    private uint _position = 0;
    private readonly int[] _data = new int[50];

    public void Push(int item) => _data[_position++] = item;
    public int Pop() => _data[_position--];
}
```

```csharp
var stack = new Stack();
stack.Push(1);
stack.Push(2);
var val = stack.Pop();
```

---

### Generic version - problem solved 👏

```csharp
public class Stack<T>
{
    private uint _position = 0;
    private readonly T[] _data = new T[50];

    public void Push(T item) => _data[_position++] = item;
    public T Pop() => _data[_position--];
}
```

```csharp
var intStack = new Stack<int>();
intStack.Push(1);

var stringStack = new Stack<string>();
stringStack.Push("auto");
```

---

### Generic interfaces

```csharp
public interface IContainer<T>
{
    void Push(T item);
    T Pop();
}

public class Stack<T> : IContainer<T>
{
    private uint _position = 0;
    private readonly T[] _data = new T[50];

    public void Push(T item) => _data[_position++] = item;
    public T Pop() => _data[_position--];
}
```

---

### Generic methods

```csharp
class Program
{
    static void DoWork<T>() =>
        Console.WriteLine(typeof(T));

    static void DoWork<T1, T2>() {}

    static void Foo<T1, T2>() =>
        Console.WriteLine($"T: {typeof(T1)}, U: {typeof(T2)}");
    
    static void Main(string[] args)
    {
        DoWork<int>();              // Output: System.Int32
        Foo<double, object>();      // T: System.Double, U: System.Object
    }
}
```

---

### Why generics?

- **Type safety** - no need for type checks
- **Less code** and code is more easily **reused**
- **Better performance** - no boxing and unboxing
- Better debugging experience

---

### Generic constrains

- Informs the compiler about the capabilities a type argument must have
- No constrains => compiler assumes the members of `System.Object`


    | Constrain                | Description                                       |
    |--------------------------|---------------------------------------------------|
    | `where T : struct`       | T must be a non-nullable value type               |
    | `where T : class`        | T must be a reference type                        |
    | `where T : <base_class>` | T must be or derive from the specified base class |
    | `where T : <interface>`  | T must be or implement the specified interface    |
    | `where T : new()`        | T must have a public parameterless ctor           |

---

```csharp
public interface IContainerItem {}
public class Item : IContainerItem {}

public class Stack<T> where T : IContainerItem
{
    private uint _position = 0;
    private readonly T[] _data = new T[50];

    public void Push(T item) => _data[_position++] = item;
    public T Pop() => _data[_position--];
}
```

```csharp
var stack1 = new Stack<int>();       // Error
var stack2 = new Stack<Item>();      // Ok
```

---

## Thank you for your attention :)

---

### Generics at runtime

- Generic type **substitutions are performed at run time**
- Compiled generic type contains metadata that identifies it as having type parameters
- Type parameter of a **value type**:
    - When specialized generic type should be created, CLR takes compiled generic type and generates new type by substituting _type parameters_
    - Specialized generic types are created one time for each unique value type that is used as a parameter
    - Previously generate specialized generic types are reused
- Type parameter of a **reference type**:
    - All references are the same size
    - The first time a specialized generic type is constructed with any reference type, CLR generates new type by substituting _type parameters_ with object references
    - CLR reuses the previously created specialized version of the generic type for every reference type

TODO

---

## Resources