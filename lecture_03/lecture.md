---
marp: true
paginate: true
theme: custom-theme
---

# C# and .NET part 3

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

## Advanced operators

TODO

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