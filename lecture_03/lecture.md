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

---

### Passing value types by value

TODO

---

### Passing reference types by value

TODO

---

### Passing value types by reference

TODO

---

### Passing reference types by reference

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
- Potential performance optimization (large data can be safely passed by reference)

---

TODO - example

---

### `params` keyword

- Method parameter that takes a variable number of arguments
- No additional parameters are permitted after the `params` keyword
- Only one `params` keyword is permitted in a method declaration
- Given parameter must be of type **single-dimensional array**

---

```csharp
static int Sum(params int[] numbers)
{
    var sum = 0;
    foreach (var num in numbers)
    {
        sum += num;
    }
    return sum;
}

Sum(1,2,3,4,5,6);
```

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

## Exceptions


- All exceptions derive from `System.Exception`

- Exception is thrown in `try` block => associated exception handler is invoked
- Exception is thrown anywhere else => the program stops with an error message

---

### Rules for using exceptions

- Don't catch an exception unless you can handle it

---

### Key properties of `System.Exception`

- `StackTrace`
    - A string representing all the methods that are called from the origin of the exception to the catch block
- `Message`
    - A string with a description of the error
- `InnerException`
    - The inner exception(if any) that caused the outer exception
    - InnerException may have another InnerException

---

## Operator overloading

TODO

---

## Collections in .NET

- Generics everywhere

TODO

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