---
marp: true
paginate: true
theme: custom-theme
---

# C# and .NET part 3

<div class="lectors">
    <hr/>
    Patrik Švikruha
    <br/>
    Samuel Janek
</div>

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

## Extension methods

- Enables us to add methods to existing types
- **Static methods** called as if they were instance methods
- Cannot access private variables in the type they are extending
- **Must be defined in static class**
- Extending _value types_ can be tricky
    (first parameter `this instance` vs `ref this instance`)
- Compatible instance method will always take precedence over an extension method

---

### Example - extension method for `string`

```csharp
static class Extensions
{
    public static bool IsCapitalized(this string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        return char.IsUpper(s[0]);
    }
}

class Program
{
    static void Main(string[] args)
    {
        string text = "HelloWorld";
        Console.WriteLine(text.IsCapitalized());    // True
        // Extensions.IsCapitalized(text);
    }
}
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
- Indicate that an error has occurred while running the program
- Two types of exceptions:
    - _Hardware exceptions_ - initiated by CPU
    - _Software exceptions_ - initiated by apps or OS
- C# has [Structured Exception Handling](https://learn.microsoft.com/en-us/windows/win32/debug/structured-exception-handling) - handles HW and SW exceptions identically
- All exceptions derive from `System.Exception`

---

### Keywords

- `try`
    - Code block subject to error-handling or cleanup code
    - Must be followed by `catch` blocks or `finally` block
- `catch`
    - Executes when an error occurs in the `try` block
    - Has access to thrown exception
- `finally`
    - Executes always
- `throw`
    - Signals the occurrence of an exception during program execution

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
- Don't throw `Exception`, `SystemException`, `NullReferenceException`, or `IndexOutOfRangeException`
- Use "_Exception_" suffix for user-defined exceptions
- Re-throwing exceptions:
    - `throw;`
    - `throw e;`
        - Stack trace of the original exception is not preserved
        - **DO NOT USE THIS**

---

## `IDisposable` and `using` statement

- **Used to properly release unmanaged resources**
- `IDisposable`
    - Interface with only one method `Dispose()`
    - Implementated by all types which encapsulate unmanaged resources
- `using` statement
    - Syntactic sugar for calling `Dispose()` on objects that implement `IDisposable`
    - A variable declared with a `using` declaration is **read-only**
    - Defines new scope
---

### Example - file reading

```csharp
class Program
{
    static void Main(string[] args)
    {
        using (StreamReader sr = new StreamReader("TestFile.txt"))
        {
            string line = sr.ReadLine();
            while (line != null)
            {
                Console.WriteLine(line);
                line = sr.ReadLine();
            }
        }
    }
}
```

---

### `using` under the hood

<div class="col2">
<div>

**Original code**

```csharp
using (var sr = new StreamReader("file"))
{
    // ...
}
```

</div>
<div>

**Lowered code**

```csharp
StreamReader sr =
    new StreamReader("file");
try
{
    // ...
}
finally
{
    if (sr != null)
    {
        ((IDisposable)sr).Dispose();
    }
}
```

</div>
</div>

---

### Remarks

- The GC does not call `Dispose` instead of you
- Dispose pattern has a bit different implementations for sealed and non-sealed classes (see the [docs](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose) for more information)
- For asynchronous cleanup operations use `DisposeAsync` method

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

## Null and related stuff

- `null` - reference that points to a nonexistent or invalid object or address
- [Tony Hoare](https://en.wikipedia.org/wiki/Tony_Hoare) - **billion-dollar mistake**
- Nullable value types
- Nullable reference types
- Null operators

---

### Nullable value types

- Extension of other value types with a `null` value
- Denoted with a value type followed by the `?` symbol

    ```csharp
    bool? isReady = null;
    ```

- `T?` translates into generic `System.Nullable<T>` structure
- Default value is `null`
- **Operator lifting** - `T?` supports any operator that is supported by `T`
- Conversions:
    - Implicit conversion from `T` to `T?`
    - Explicit conversion from `T?` to `T` (exception when value of `T` is `null`)

---

### Nullable reference types

- **Optional** language feature (turned on by default)
- Goal is to avoid `NullReferenceExceptions` 
- Denoted with a reference type followed by the `?` symbol

    ```csharp
    string? text = null;
    ```

- Only available in **nullable context**:
    - Enabled using `.csproj` settings or `#nullable ...` directives
    - Advanced `null` related static analysis
- Compile-time feature - reference types `T` and `T?` are equal during runtime

---

#### Example - nullable context

```csharp
#nullable enable

namespace HelloWorld;

class Program
{
    static void Main(string[] args)
    {
        string text1 = null;        // Compiler warning
        string? text2 = null;       // Ok - it is nullable reference type
        int num1 = 5;               // Ok - it is ordinary value type
        int? num2 = null;           // Ok - it is nullable value type

        Console.WriteLine(text1);   // null
        Console.WriteLine(text2);   // null
        Console.WriteLine(num1);    // 5
        Console.WriteLine(num2);    // null
    }
}
```

---

#### Null-forgiving operator `!`

- Useful only in _nullable context_
- Compile-time feature - has no effect at run time
- Suppresses compiler warnings related to `null`

    ```csharp
    class Person
    {
        public string Name { get; set; } = null!;   // Good usage
        public string? Address { get; set; }

        public void Foo(Person obj)
        {
            var firstLetter = obj.Address![0];   // Bad usage - possible exception
        }
    }
    ```

---

### Null-coalescing operator `??`

- `var x = y ?? z`
- Returns `y` if it isn't `null`, otherwise evaluates `z` and returns result
- `x` must be a nullable type
- Right-associative: `a ?? b ?? c` <=> `a ?? (b ?? c)`

    <div class="col2">
    <div>

    ```csharp
    string text = null;
    if (text == null)
    {
        throw new Exception();
    }
    string copy = text;
    ```

    </div>
    <div>

    ```csharp
    string text = null;
    var copy = text ?? throw new Exception();
    ```

    </div>
    </div>

---

### Null-coalescing assignment operator `??=`

- `var x ??= y`
- Assigns `y` to `x` if and only if `x` evaluates to `null`
- `x` must be a nullable type
- Right-associative: `d ??= e ??= f` <=> `d ??= (e ??= f)`

    ```csharp
    string text = null;
    Console.WriteLine(text);    // null
    text ??= "seesharp";
    Console.WriteLine(text);    // "seesharp"
    ```
---

### Null-conditional operator `?.`

- `var x = y?.z`
- Applies a member access operation or method call iff `y` evaluates to non-null
- Returns `null` if `y` evaluates to `null`

    ```csharp
    StringBuilder sb = null;
    string text = sb?.ToString();
    char? firstLetter = (sb?.ToString())?[0];
    ```

---

## Delegates

- **Type that represents references to methods**
- Defines method's return type and parameters
- Assigning a method to a delegate variable creates a delegate instance
- The code associated with a delegate is invoked using a virtual method added to a delegate type
- **Delegates are immutable**
- Syntax:

    ```csharp
    delegate void Calculation(int n1, int n2);
    ```

---

### Example - instance target methods

```csharp
class Calculator
{
    public int Add(int x, int y) => x + y;
    public int Sub(int x, int y) => x - y;
}

delegate int Delegate(int x, int y);

class Program
{
    static void Main(string[] args)
    {
        var obj = new Calculator();
        Delegate calc1 = obj.Add;
        Console.WriteLine(calc1(10, 5));    // 15
        Delegate calc2 = obj.Sub;
        Console.WriteLine(calc2(10, 5));    // 5
    }
}
```

---

### Example - pass method as argument

```csharp
class Calculator
{
    public int Calculate(Delegate method, int x, int y) => method(x, y);
}

delegate int Delegate(int x, int y);

class Program
{
    public static int Add(int x, int y) => x + y;
    public static int Sub(int x, int y) => x - y;
    
    static void Main(string[] args)
    {
        var obj = new Calculator();
        Console.WriteLine(obj.Calculate(Add, 10, 5));    // 15
        Console.WriteLine(obj.Calculate(Sub, 10, 5));    // 5
    }
}
```

---

### Multicast delegates

- **Invoke more than one method usign one delegate**
- Delegate contains invocation list
- Methods are invoked in the same order in which they were added
- All delegate instances have _multicast_ capability
- To add method to invocation list use `+` or `+=`
- To remove method from invocation list use `-` or `-=`
- If a multicast delegate has a nonvoid return type, **the caller receives the return value from the last method to be invoked**

---

#### Example - multicasting

```csharp
class MethodsClass
{
    public void Foo1() => Console.Write("1");
    public void Foo2() => Console.Write("2");
    public void Foo3() => Console.Write("3");
}

delegate void MulticastDelegate();

class Program
{
    static void Main(string[] args)
    {
        var obj = new MethodsClass();
        MulticastDelegate del = obj.Foo1;
        del += obj.Foo2;
        del += obj.Foo3;

        del();  // output: 1 2 3
    }
}
```

---

### `Func` and `Action` delegates

- Based on _generics_
- Defined in `System` namespace
- `Func` - has a return value

    ```csharp
    public delegate TResult Func<out TResult>();
    public delegate TResult Func<in T,out TResult>(T arg);
    public delegate TResult Func<in T1,in T2,out TResult>(T1 arg1, T2 arg2);
    ```

- `Action` - does not have a return value

    ```csharp
    public delegate void Action();
    public delegate void Action<in T>(T obj);
    public delegate void Action<in T1,in T2>(T1 arg1, T2 arg2);
    ```

---

#### Example - `Func` and `Action` delegates

```csharp
class MyClass
{
    public void Foo1(Action method) => method();
    public int Foo2(Func<int, int> method, int value) => method(value);
}

class Program
{
    static void SayHi() => Console.WriteLine("Hi");
    static int Multiply(int value) => value * 2;
    
    static void Main(string[] args)
    {
        var obj = new MyClass();
        obj.Foo1(SayHi);                    // output: "Hi"
        var result = obj.Foo2(Multiply, 2);     
        Console.WriteLine(result);          // output: 4 
    }
}
```

---

## Lambda expressions

- `(parameters) => expression-or-statement-block`
- Used to create anonymous functions
- Compiler converts the lambda expression to _delegate instance_ or _expression tree_

    ```csharp
    Func<int, int> pow = (int x) => x * x;
    pow(2);             // output: 4

    Action greet = () => Console.WriteLine("Hi");
    greet();            // output: "Hi"

    Action<string> writer = Console.WriteLine;
    writer("Hello");    // output: "Hello"
    ```

---

### Capture of outer variables and variable scope

- Variables outside of lambda's scope (outer variables) can be used in lambda's body
- These outer variables are called _captured variables_
- Lifetime of _captured variables_ is extended to lifetime of the delegate

    ```csharp
    static Func<int> Foo()
    {
        int seed = 0;
        return () => seed++;        // Returns closure
    }
    
    static void Main(string[] args)
    {
        Func<int> foo = Foo();
        Console.WriteLine(foo());   // output: 0
        Console.WriteLine(foo());   // output: 1
    }
    ```

---

### Static lambdas

- _Captured variables_ incur a small performance cost
- **Static lambda cannot capture any outer variable**

    ```csharp
    int factor = 2;
    Func<int, int> multiplier1 = static n => n * 2;         // Ok
    Func<int, int> multiplier2 = static n => n * factor;    // Error
    ```

---

## Attributes

- **Mechanism for adding custom information (metadata) to code elements**
- Program can examine metadata using _reflexion_
- All attributes inherit from `System.Attribute`
- By convention, all attribute names end with "_Attribute_" suffix
- The target of the attribute can be _class_, _method_, _assembly_, _parameter_...
- Syntax:

    ```csharp
    [Serializable]
    public class SampleClass {}
    ```

---


### Example - custom attribute

```csharp
[AttributeUsage(AttributeTargets.Class)]
class AuthorAttribute : Attribute
{
    public string Name { get; }
    
    public AuthorAttribute(string name) => Name = name;
}
```

```csharp
[Author("Samuel Janek", "1.0.0")]
class Program
{
    static void Main(string[] args)
    {
        var attribute = Attribute.GetCustomAttribute(typeof(Program), typeof(AuthorAttribute));
        var authorAttr = (AuthorAttribute)attribute;
        Console.WriteLine(authorAttr.Name);
    }
}
```

---

## Preprocessor directives

- Conditional compilation

    ```csharp
    #define DEBUG
    #ifdef DEBUG
    // this is included if DEBUG symbol is defined
    #endif
    ```

- Disable/restore warnings

    ```csharp
    #pragma warning disable 414
    ```

- Disable/enable nullable reference types

    ```csharp
    #nullable enable
    ```

---

## Thank you for your attention :)

---

### Generics at runtime

- Generic type **substitutions are performed at run time**
- Compiled generic type contains metadata that identifies it as having type params
- Type parameter of a **value type**:
    - When specialized generic type should be created, CLR takes compiled generic type and generates new type by substituting _type parameters_
    - Specialized generic types are created one time for each unique value type that is used as a parameter
    - Previously generate specialized generic types are reused
- Type parameter of a **reference type**:
    - The first time a specialized generic type is constructed with any reference type, CLR generates new type by substituting _type parameters_ with _object_ references
    - Previously created specialized version of the generic type is reused for every reference type

---

## Resources

- [C# in Nutshell](https://www.amazon.com/gp/product/1098121953?ie=UTF8&tag=cinanu-20&linkCode=as2&camp=1789&creative=9325&creativeASIN=1098121953)
- [VUT FIT ICS slides](https://github.com/nesfit/ICS/tree/master/Lectures)