---
marp: true
paginate: true
theme: custom-theme
---

# C# and .NET part 4

<div class="lectors">
    <hr/>
    Patrik Švikruha
    <br/>
    Samuel Janek
</div>

---

## Collections

- There are two ways how to group objects:
	- **Arrays** - part of C# language
	- **Collections** - ordinary classes
		- Generic
            - Type-safe at compile time
            - Better performance
		- Non-generic
            - Legacy support
            - **DO NOT USE THEM**

---

### Important interfaces

![height:500 center](./images/collection-interfaces.png)

Image from [C# in Nutshell](https://www.albahari.com/nutshell/) page 334

---

#### Enumeration and iterators

- Enumeration = **froward-only** traversal through the collection
- Realized using
	- `IEnumerator` and `IEnumerable` - non-generic version
	- `IEnumerator<T>` and `IEnumerable<T>` - generic version

---

##### `IEnumerator<T>`

- Defines a protocol by which elements in a collection can be
	traversed in **forward-only** manner
- `IDisposable` ensures that _enumerator_ is disposed after enumeration

	```csharp
	public interface IEnumerator
    {
        bool MoveNext();
        object Current { get; }
        void Reset();
    }

	public interface IEnumerator<out T> : IEnumerator, IDisposable
    {
        new T Current { get; }
    }
	```

---

##### `IEnumerable<T>`

- Collection which implements this interface can be
    traversed in **forward-only** manner
- Automatically implemented by **arrays**
- "_IEnumerator provider_"

	```csharp
	public interface IEnumerable
    {
        IEnumerator GetEnumerator();
    }

	public interface IEnumerable<out T> : IEnumerable
    {
        new IEnumerator<T> GetEnumerator();
    }
	```

---

##### `foreach` statement and enumerators

`foreach` statement is a consumer of an _enumerator_

<div class="col2">
<div>

**Syntactic sugar**

```csharp
string beer = "beer";
foreach (var c in beer)
{
	Console.Write(c);
}
```

</div>
<div>

**Lowered code**

```csharp
string beer = "beer";
using (var e = "beer".GetEnumerator())
{
	while (e.MoveNext())
	{
		var c = e.Current;
		Console.Write(c);
	}
}
```

</div>
</div>

---

##### Iterators

- Realized using `yield` statement
- An **iterator method** defines how to generate the objects in a sequence

    ```csharp
    static IEnumerable<int> GenerateNumbers()
    {
        yield return 1;
        yield return 2;
        yield return 3;
    }

    foreach (var number in GenerateNumbers())
    {
        Console.Write(number);
    }
    // Output: 123
    ```

---

##### Iterators

- Producer of an _enumerator_
- Alternative to implementing `IEnumerable<T>` interface by yourself
- An iterator methods must return one of the following:
    - `IEnumerable` or `IEnumerable<T>`
    - `Enumerator` or  `IEnumerator<T>`
- `yield return` cannot appear in `try`, `catch` or `finally` blocks
- `yield break` signals the end of iteration

---

##### `IEnumerable<T>` remarks

- The base interface for collections in the `System.Collections.Generic` namespace
- It **doesn't** provide mechanism to:
    - Determine the size of the collection
    - Access member by index
    - Modify the collection
    - Search in the collection

---

#### `ICollection<T>`

```csharp
public interface ICollection<T> : IEnumerable<T>
{
    int Count { get; }
    bool IsReadOnly { get; }
    void Add(T item);
    void Clear();
    bool Contains(T item);
    void CopyTo(T[] array, int arrayIndex);
    bool Remove(T item);
}
```

---

##### `ICollection<T>` remarks

- Extends `IEnumerable<T>` interface
- Standard interface for **countable collections** of objects.
- The base interface for classes in the `System.Collections.Generic` namespace
- Read-only version is `IReadOnlyCollection<T>`

---

#### `IList<T>`

```csharp
public interface IList<T> : ICollection<T>
{
    T this[int index] { get; set; }
    int IndexOf(T item);
    void Insert(int index, T item);
    void RemoveAt(int index);
}
```

---

##### `IList<T>` remarks

- Extends `ICollection<T>` interface
- Represents a collection of objects that can be individually **accessed by index**
- Arrays implement both `IList` and `IList<T>`
- Read-only version is `IReadOnlyList<T>`

---

#### `IDictionary<TKey, TValue>`

```csharp
public interface IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>
{
    TValue this[TKey key] { get; set; }
    ICollection<TKey> Keys { get; }
    ICollection<TValue> Values { get; }
    bool ContainsKey(TKey key);
    void Add(TKey key, TValue value);
    bool Remove(TKey key);
    bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value);
}
```

---

##### `IDictionary<TKey, TValue>` remarks

- Extends `ICollection<T>` interface
- Defines the standard protocol for all key/value-based collections
- Keys can be any **non-null** object
- Values can be any object
- **Duplicate keys are forbidden**
- Read-only version is `IReadOnlyDictionary<TKey, TValue>`

---

### Collection types

- `Array`
- `List<T>`
- `LinkedList<T>`
- `Queue<T>`
- `Stack<T>`
- `HashSet<T>`
- `Dictionary<TKey, TValue>`

---

#### `Array`

- Implements `ICollection`, `IEnumerable`, `IList`
- Implicit base class for all types of arrays in C#
- Fixed length, contiguous space in memory

---

TODO - sample

---

#### `List<T>`

- Implements `ICollection<T>`, `IEnumerable<T>`, `IList<T>`
- Represents a **strongly typed** list of objects that **can be accessed by index**
- Internally implemented using an **array whose size is dynamically increased as required**
- Accepts `null` as a valid value for reference types and allows duplicate elements

---

TODO - sample

---

#### `LinkedList<T>`

- Implements `ICollection<T>`, `IEnumerable<T>`
- **Does not** implement `IList<T>` => **access by index is not supported**
- Generic **doubly linked** list
- Nodes are of type `LinkedListNode<T>`
- Accepts `null` as a valid value for reference types and allows duplicate values

---

![height:500 center](./images/linked-list.png)

Image from [C# in Nutshell](https://www.albahari.com/nutshell/) page 366

---

TODO - sample

---

#### `Queue<T>`

- Implements `IEnumerable<T>`
- **Does not** implement `IList<T>` => **access by index is not supported**
- FIFO (first-in-first-out) data structure
- Internally implemented using an **array whose size is dynamically increased as required**
- Accepts `null` as a valid value for reference types and allows duplicate elements
- Basic operations:
    - `Enqueue` - adds an element to the end 
    - `Dequeue` - removes the oldest element from the start
    - `Peek` - returns the oldest element that is at the start but does not remove it

---

TODO - sample

---

#### `Stack<T>`

- Implements `IEnumerable<T>`
- **Does not** implement `IList<T>` => **access by index is not supported**
- LIFO (last-in-first-out) data structure
- Internally implemented using an **array whose size is dynamically increased as required**
- Accepts `null` as a valid value for reference types and allows duplicate elements
- Basic operations:
    - `Push` - inserts an element at the top
    - `Pop` - removes an element from the top
    - `Peek` - returns an element that is at the top but does not remove it

---

TODO - sample

---

#### `HashSet<T>`

- Implements `ICollection<T>`, `IEnumerable<T>`
- Represents a set of values
- Provides high-performance set operations
- **No duplicate elements**
- Internally implemented using **hash table**
- Supports mathematical set operations such as _union_, _intersection_, _subtraction_

---

TODO - sample

---

#### `Dictionary<TKey, TValue>`

- Implements `ICollection<T>`, `IEnumerable<T>` where `T` is `KeyValuePair<TKey,TValue>`
- Represents a **collection of keys and values**
- Internally implemented using **hash table**
- A key cannot be `null`
- `Add` method throws an exception when attempting to add a **duplicate key**
- `KeyNotFoundException` is thrown when a requested key is not present

---

TODO - sample

---

## LINQ

- **Language Integrated Query**
- Defined in `System.Linq` namespace
- Set of langauge and runtime features for writing **structured type-safe queries**
- Usable with any collection implementing `IEnumerable<T>` or `IQueryable<T>`
- Extensive use of generics, delegates and extension methods
- Inspired by functional programming
- Different types:
    - LINQ to SQL
    - LINQ to XML
    - LINQ to Objects

---

### Motivation - sum of all items

**Custom method**

```csharp
static int Sum(int[] arr)
{
    var sum = 0;
    foreach (var num in arr)
        sum += num;
    return sum;
}
    
static void Main(string[] args)
{
    int[] numbers = { 1, 2, 3, 4, 5 };
    var sum = Sum(numbers);
    Console.WriteLine(sum);     // Output: 15
}
```

---

### Motivation - sum of all items

**Using LINQ**

```csharp
using System.Linq;

static void Main(string[] args)
{
    int[] numbers = { 1, 2, 3, 4, 5 };
    var sum = numbers.Sum();
    Console.WriteLine(sum);     // Output: 15
}
```

---

### Query expression

- Specifies what operations should be applied to data source
- Composed of **query operators**
- Only stores the query commands (not the result)
- LINQ queries are compiled:
    - `IQueryable<T>` => **expression trees**
    - `IEnumerable<T>` => **delegates**

---

### Fluent vs Query syntax

<div class="col2">
<div>

**Fluent syntax**

```csharp
string[] names = { "Dick", "Harry", "Jay" };

IEnumerable<string> query = names
    .Where(n => n.Contains("a"))
    .OrderBy(n => n.Length)
    .Select(n => n.ToUpper());

foreach (string name in query)
    Console.WriteLine(name);

// Output: JAY, HARRY
```

</div>
<div>

**Query syntax**

```csharp
string[] names = { "Dick", "Harry", "Jay" };

IEnumerable<string> query =
    from n in names
    where n.Contains("a")
    orderby n.Length
    select n.ToUpper();

foreach (string name in query)
    Console.WriteLine(name);

// Output: JAY, HARRY
```

</div>
</div>

---

![height:400 center](./images/linq-conveyor-belt.png)

Image from [C# in Nutshell](https://www.albahari.com/nutshell/) page 399

---

### Deferred vs Immediate execution

- **Execution of the query is deferred** until you iterate over the query variable
- `ToList` or `ToArray` methods force immediate execution of the query
- Some queries are by default executed immediately (`Count`, `Max`, `Average`, `First`)

    ```csharp
    string[] names = { "Dick", "Harry", "Jay" };
            
    IEnumerable<string> query = names.Where(n => n.Contains("a"));
    // query: instance of Enumerable.WhereArrayIterator<string>

    var count = names.Max(n => n.Length);
    // count: 5

    var queryResult = query.ToList();
    // queryResult: { "Harry", "Jay" }
    ```

---

### Reevalution

- **Deferred execution query is reevaluated when you reenumerate it**
- Some queries can be computationally intensine => it is better to save a copy of the result (evaluation) using `ToList` or `ToArray` methods

    ```csharp
    string[] names = { "Dick", "Harry", "Jay" };

    IEnumerable<string> query = names.Select(n => n.ToUpper());

    // 1. evaluation
    foreach (var name in query) Console.WriteLine(name);
    
    // 2. evaluation
    foreach (var name in query) Console.WriteLine(name);

    var evaluationResult = query.ToList();
    ```

---

### Query operators

- Defined as **extension methods** of the type that they operate on
- **Never alter the input sequence** (they return a new sequence)
- The standard query operators differ in the timing of their execution
- Not each of them is applicable using query expression syntax
- [50+ operators](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/classification-of-standard-query-operators-by-manner-of-execution#classification-table) 🚀

---

#### Standard operators - `Where`

- Filters a sequence of values based on a predicate
- **Deffered execution**

    ```csharp
    int[] numbers = new[] { 0, 1, 2, 3, 4 };
            
    var query = numbers.Where(n => n > 5);

    foreach (var number in query)
    {
        Console.WriteLine(number);
    }
    // Output: { 6, 7, 8, 9 }
    ```

---

#### Standard operators - `Select`

- Projects each element of a sequence into a new form
- **Deffered execution**

    ```csharp
    int[] numbers = new[] { 5, 6, 7, 8 };
            
    var query = numbers.Select((number, index) => number * index);

    foreach (var number in query)
    {
        Console.WriteLine(number);
    }
    // Output: { 0, 6, 14, 24 }
    ```

---

#### Standard operators - `Any`

- Determines whether any element of a sequence exists or satisfies a condition
- **Immediate execution**

    ```csharp
    int[] numbers = new[] { 1, 2, 3, 4, 5 };
            
    var res1 = numbers.Any();
    var res2 = numbers.Any(n => n > 10);

    Console.WriteLine(res1);    // True
    Console.WriteLine(res2);    // False
    ```

---

#### Standard operators - `Contains`

- Determines whether a sequence contains a specified element by using the default equality comparer
- **Immediate execution**

    ```csharp
    int[] numbers = new[] { 1, 2, 3, 4, 5 };

    var res1 = numbers.Contains(3);
    var res2 = numbers.Contains(10);

    Console.WriteLine(res1);    // True
    Console.WriteLine(res2);    // False
    ```

---

#### Standard operators - `ElementAt`

- Returns the element at a specified index in a sequence
- **Immediate execution**

    ```csharp
    int[] numbers = new[] { 1, 2, 3, 4, 5 };

    var elem1 = numbers.ElementAt(3);

    Console.WriteLine(elem1);           // 4

    var elem2 = numbers.ElementAt(10);  // ArgumentOutOfRangeException
    ```

---

#### Standard operators - `First` and `FirstOrDefault`

- `First` - returns the first element of a sequence
- `FirstOrDefault` - returns the first element of the sequence that satisfies a condition, or a specified default value if no such element is found
- **Immediate execution**

    ```csharp
    int[] numbers = new[] { 1, 2, 3, 4, 5 };

    var n1 = numbers.First();                        // 1
    var n2 = numbers.First(n => n > 3);              // 4
    var n3 = numbers.FirstOrDefault();               // 1
    var n4 = numbers.FirstOrDefault(n => n > 10);    // 0
    ```

---

#### Standard operators - `Single` and `SingleOrDefault`

- `Single` - returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists
- `SingleOrDefault` - returns the only element of a sequence, or a default value if the sequence is empty or throws an exception if more than one such element exists
- **Immediate execution**

    ```csharp
    int[] numbers = new[] { 1, 2, 3, 4, 5 };

    var n1 = numbers.Single(n => n == 2);               // 2
    var n2 = numbers.SingleOrDefault(n => n == 10);     // 0 - default value
    var n3 = numbers.SingleOrDefault(n => n > 1);       // Exception 
    ```

---

#### Standard operators - `GroupBy`

- Groups the elements of a sequence according to a specified key
- **Deffered execution**

    ```csharp
    record Person(string Name, uint Age);

    var persons = new Person[]
        { new("John", 40), new("Adam", 22), new("Mike", 22), new("Carl", 52) };

    var grouped = persons.GroupBy(p => p.Age);

    foreach (var group in grouped) Console.WriteLine(group);

    // { Key: 40, [ { Name: "John", Age: 40 } ] }
    // { Key: 22, [ { Name: "Adam", Age: 22 }, { Name: "Mike", Age: 22 } ] }
    // { Key: 50, [ { Name: "Carl", Age: 50 } ] }
    ```

---

#### Standard operators - `OrderBy`

- Sorts the elements of a sequence in ascending order according to a key
- **Deffered execution**

    ```csharp
    record Person(string Name, uint Age);

    var persons = new Person[]
        { new("John", 40), new("Adam", 22), new("Mike", 22), new("Carl", 52) };

    var ordered = persons.OrderBy(p => p.Age);

    foreach (var person in ordered) Console.WriteLine(person.Name);

    // Output: Adam, Mike, John, Carl
    ```

---

#### Standard operators - `Skip` and `SkipWhile`

- `Skip` - bypasses a specified number of elements in a sequence and then returns the remaining elements
- `SkipWhile` - bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements
- **Deffered execution**

    ```csharp
    int[] numbers = new[] { 1, 2, 3, 4, 5 };

    var query1 = numbers.Skip(2);
    var qeury2 = numbers.SkipWhile(n => n < 3);

    foreach (var n in query1) Console.WriteLine(n);     // Output: 3,4,5
    foreach (var n in qeury2) Console.WriteLine(n);     // Output: 3,4,5
    ```

---

#### Standard operators - `Take` and `TakeWhile`

- `Take` - returns a specified number of elements from the start of a sequence
- `TakeWhile` - returns elements from a sequence as long as a condition is true
- **Deffered execution**

    ```csharp
    int[] numbers = new[] { 1, 2, 3, 4, 5 };

    var query1 = numbers.Take(2);
    var qeury2 = numbers.TakeWhile(n => n < 3);

    foreach (var n in query1) Console.WriteLine(n);     // Output: 1,2
    foreach (var n in qeury2) Console.WriteLine(n);     // Output: 1,2
    ```

---

#### Standard operators - `Range`

- Generates a sequence of integral numbers within a specified range
- Parameters:
    1. `start` - the value of the first integer in the sequence
    2. `count` - the number of sequential integers to generate
- **Deffered execution**

    ```csharp
    var seq1 = Enumerable.Range(-5, 5);
    var seq2 = Enumerable.Range(1, 5);

    foreach (var n in seq1) Console.WriteLine(n);   // -5,-4,-3,-2,-1
    foreach (var n in seq2) Console.WriteLine(n);   // 1,2,3,4,5
    ```

---

#### Standard operators - `Aggregate`

- Applies an accumulator function over a sequence
- The specified seed value is used as the initial accumulator value
- **Deffered execution**

    ```csharp
    string[] words = { "car", "do", "monitor", "auto", "begin" };

    string longestWord = words.Aggregate("car", (longest, text) =>
    {
        if (longest.Length < text.Length)
            return text;
        return longest;
    });

    Console.WriteLine(longestWord);     // Output: monitor
    ```

---

## Concurrency and Asynchrony

---

### Motivation

- Most applications need to deal with more than one thing happening at a time
- When do we need _concurrency and asynchrony_?
    - Writing responsive UI
    - Parallel programming
    - I/O operations (e.g. file reading)
    - Expensive computations

---

### Possible approaches

- Working with threads
- Using tasks
- **Asynchronous programming**

---

### Asynchronous programming

- Supported by .NET runtime
- Uses `async` and `await` keywords
- Preferred over direct usage of threads or tasks
- Compiler does most of the heavy lifting
- Async programming with `async` and `await` follows the
    [Task-based Asynchronous Pattern (TAP)](https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap)

---

#### Tasks

- The core of async programming
- Represent an async operation that **might or might not be backed by a thread**
- The **task scheduler** orchestrates task execution
- Higher-level abstraction compared to a threads
- `Task` - represents an async operation that **does not return a value**
- `Task<TResult>` - represents an async operation that **returns a value**
- `ValueTask<TResult>` - optimization scenarios

<!--
SPEAKER NOTES:
- Tasky nie su nevyhnutne použivané kvôli performance
- Ich hlavná výhoda je zvýšenie škálovateľnosti aplikácie (neblokujeme jedno hlavné vlákno)
- V praxi by sme sa nemali až tak starať o to ako sú realizované na úrovni HW
- ValueTask:
    - Nepoužívajte ak fakt neviete ako sa s ním pracuje
    - V prípade že metóda skončí synchronne ušetrí alokáciu na heape
    - Je to value type
-->

---

TODO - async await a async methods

---

#### What happens in an async method

![height:500 center](./images/navigation-trace-async-program.png)

---

#### Cooking example - demo

- Code for breakfast preparation
- Multiple versions:
    - Synchronous version
    - Asynchronous slow version
    - Asynchronous fast version

TODO - pridat nejaky meme obrazok

<!--
SPEAKER NOTES:
- Ukázať Cooking project v /src zložke
-->

---

TODO - Pouzitie taskov a ich properties

---

ASYNC

- turns a method into an async method, which allows you to use the await keyword in its body

AWAIT

- suspends the calling method and yields control back to its caller until the awaited task is complete
- Under the covers, the await functionality installs a callback on the task by using a continuation

ASYNC METHOD

- runs synchronously until it reaches its first await expression
- Is intended to be non-blocking operation
- Can be awaited by calling methods
- Doesn't require multithreading because it doesn't run on its own thread (to spawn a new thread use `Task.Run` method)
- Runs on the current synchronization context
- Typically returns a `Task` or `Task<TResult>`
- Pridat vsetky mozne return values
- The async method can't declare any in, ref or out parameters

---

TASK CLASS INTERNALS

- The Result property is a blocking propert
y. If you try to access it before its task is finished, the thread that's currently active is blocked until the task completes and the value is available
- provides a life cycle for asynchronous operations, and that cycle is represented by the TaskStatus enumeration

ASYNC EXCEPTIONS

- Asynchronous methods throw exceptions, just like their synchronous counterparts
- When a task that runs asynchronously throws an exception, that Task is faulted and that exception is stored in the Task
- Faulted tasks throw an exception when they're awaited
- Nepouzivat async void
- Nepouzivat try catch ak tam neawaitujeme async operaciu

BEST PRACTICES AND CONVENTIONS

- Async methods has `Async` sufffix
- Async all the way down
- Do not use async methods without await inside it - big performance implication
- Odkaz na github clanok o async await
- I/O-bound, use async and await without Task.Run
- CPU-bound and you care about responsiveness, use async and await, but spawn off the work on another thread with Task.Run
- Use await instead of Task.Wait - second options blocks the thread
- Consider using ValueTask where possible
- Do not use void return type with async methods unless you are forced to do it - because of exceptions
- Use `Task.Delay` instead of `Thread.Sleep` method

CANCELLATION TOKEN

---

## Thank you for your attention :)

---

### Iterators under the hood

- Compiler converts iterator method into private class that implements `IEnumerable<T>` and `IEnumerator<T>`
- Iterator method then returns instance of a compiler written class which has a `GetEnumerator()` method which returns enumerator
- `foreach` statement which consumes your iterator method is rewritten to `while` loop which uses enumerator to travers the collection
- For more details, you can examine lowered code on [sharplab.io](https://sharplab.io/)

---

### Async vs Parallel

TODO

- Async vs Parallel execution:
    - One person can make breakfast asynchronously by starting the next task before the first task completes
    - For a parallel algorithm, you'd need multiple cooks (or threads). One would make the eggs, one the bacon, and so on.

---

## Resources

- [C# in Nutshell](https://www.amazon.com/gp/product/1098121953?ie=UTF8&tag=cinanu-20&linkCode=as2&camp=1789&creative=9325&creativeASIN=1098121953)
- [VUT FIT ICS slides](https://github.com/nesfit/ICS/tree/master/Lectures)
- [Tasks are not threads and they are not for performance](https://steven-giesel.com/blogPost/d095383f-7ea9-4419-96b8-889c6981cce0?utm_source=substack&utm_medium=email)
- [David Fowler - Async Guidance](https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md)