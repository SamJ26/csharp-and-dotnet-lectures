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

<!--
SPEAKER NOTES:
- IEnumerator predstavuje implementáciu Iterator patternu
-->

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
- The array size is limited to a total of 4 billion elements

---

#### `List<T>`

- Implements `ICollection<T>`, `IEnumerable<T>`, `IList<T>`
- Represents a **strongly typed** list of objects that **can be accessed by index**
- Internally implemented using an **array whose size is dynamically increased as required**
- Accepts `null` as a valid value for reference types and allows duplicate elements

---

```csharp
record Person(string Name, uint Age);

List<Person> persons = new List<Person>();

persons.Add(new Person("Samuel", 24));
persons.AddRange(new []
{
    new Person("Adam", 42),
    new Person("Norbit", 58)
});

Console.WriteLine(persons.Count);       // 3
Console.WriteLine(persons.Capacity);    // 4 - it is not always Count + 1

Console.WriteLine(persons.Contains(new Person("Samuel", 24)));    // True
Console.WriteLine(persons.Contains(new Person("Animal", 100)));   // False

Console.WriteLine(persons[0].Name);     // "Samuel"
```

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

```csharp
LinkedList<string> list = new();

Console.WriteLine(list.First == list.Last == false);

list.AddFirst("1");
list.AddFirst("0");
list.AddLast("4");
list.AddBefore(list.Last!,"2");
list.AddBefore(list.Last!, "3");

Console.WriteLine(list.Count);      // 5

foreach (var node in list)
{
    Console.WriteLine(node);        // Output: 0 1 2 3 4
}
```

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

```csharp
Queue<int> numbers = new Queue<int>();
        
numbers.Enqueue(5);
numbers.Enqueue(10);

Console.WriteLine(numbers.Peek());      // 5
Console.WriteLine(numbers.Count);       // 2
Console.WriteLine(numbers.Dequeue());   // 5
Console.WriteLine(numbers.Count);       // 1

Console.WriteLine(numbers.Contains(11));    // False
```

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

```csharp
Stack<int> numbers = new Stack<int>();
        
numbers.Push(5);
numbers.Push(10);

Console.WriteLine(numbers.Peek());      // 10
Console.WriteLine(numbers.Count);       // 2
Console.WriteLine(numbers.Pop());       // 10
Console.WriteLine(numbers.Count);       // 1

Console.WriteLine(numbers.Contains(11));    // False
```

---

#### `HashSet<T>`

- Implements `ICollection<T>`, `IEnumerable<T>`
- Represents a set of values
- Provides high-performance set operations
- **No duplicate elements**
- Internally implemented using **hash table**
- Supports mathematical set operations such as _union_, _intersection_, _subtraction_

---

```csharp
HashSet<int> set1 = new HashSet<int>();

set1.Add(1);
set1.Add(2);
set1.Add(2);    // Trying to add duplicate value

HashSet<int> set2 = new() { 2, 3, 4 };

Console.WriteLine(set1.Count);       // 2
Console.WriteLine(set2.Count);       // 3

set1.UnionWith(set2);

foreach (var n in set1)
{
    Console.WriteLine(n);       // Output: 1 2 3 4
}
```

---

#### `Dictionary<TKey, TValue>`

- Implements `ICollection<T>`, `IEnumerable<T>` where `T` is `KeyValuePair<TKey,TValue>`
- Represents a **collection of keys and values**
- Internally implemented using **hash table**
- A key cannot be `null`
- `Add` method throws an exception when attempting to add a **duplicate key**
- `KeyNotFoundException` is thrown when a requested key is not present

---

```csharp
Dictionary<int, string> dictionary = new();
        
dictionary.Add(8, "Eight");         // Ok
dictionary.Add(8, "Value");         // Exception ❌
dictionary.TryAdd(8, "Value");      // Ok

var v1 = dictionary[8];                         // Ok
dictionary.TryGetValue(8, out string? v2);      // Ok
var v3 = dictionary[10];                        // Exception ❌

foreach (var pair in dictionary)
{
    Console.WriteLine(pair.Key + " " + pair.Value);
}
```

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
- **Async != parallel**

---

#### Tasks

- The core of async programming
- Represent an async operation that **might or might not be backed by a new thread**
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

##### Important properties

- `Exception` - gets the `AggregateException` that caused the task to end prematurely
- `IsCanceled` - gets whether the task has completed due to being canceled
- `IsCompleted` - gets a value that indicates whether the task has completed
- `IsCompletedSuccessfully` - gets whether the task ran to completion
- `IsFaulted` - gets whether the task completed due to an unhandled exception

<!--
SPEAKER NOTES:
- IsCompleted will return true when the task is in one of the three final states: RanToCompletion, Faulted, or Canceled
- IsCompletedSuccessfully je true iba ak sa task skončil úspešne
-->

---

#### `async` keyword

- Turns a method into an async method, which allows you to use the `await` keyword in its body

#### `await` operator

- Suspends the calling method and yields control back to its caller until the awaited task is complete
- Under the covers, the await functionality installs a callback on the task by using a continuation

---

#### Asynchronous method

- Is intended to be non-blocking operation
- Runs synchronously until it reaches its first `await` expression
- Can't declare any `in`, `ref` or `out` parameters
- Can have the following return types:
    - `Task` or `Task<TResult>`
    - `void` - event handlers
    - `IAsyncEnumerable<T>` - async streams
    - Any type that has an accessible `GetAwaiter` method

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

<!--
SPEAKER NOTES:
- Ukázať Cooking project v /src zložke
-->

---

#### Exceptions in asynchronous code

- Asynchronous methods throw exceptions, just like their synchronous counterparts
- When a task that runs asynchronously throws an exception, that task is faulted and exception is stored in the task
- Faulted tasks throw an exception when they're awaited
- **Important remarks**:
    - Exceptions from `async void` methods cannot be catched!!
    - Always await asynchronous method in `try` block

---

#### Exceptions in asynchronous code

<div class="col2">
<div>

**async void** ❌

```csharp
static async void ThrowExceptionVoidAsync()
    => throw new Exception();

static async Task Main(string[] args)
{
    Console.WriteLine("START");
    try
    {
        ThrowExceptionVoidAsync();
    }
    catch (Exception e)
    {
        Console.WriteLine("EXCEPTION");
    }
    Console.WriteLine("END");
}

// Output: START, END
```

</div>
<div>

**async Task** ✅

```csharp
static async Task ThrowExceptionTaskAsync()
    => throw new Exception();

static async Task Main(string[] args)
{
    Console.WriteLine("START");
    try
    {
        await ThrowExceptionTaskAsync();
    }
    catch (Exception e)
    {
        Console.WriteLine("EXCEPTION");
    }
    Console.WriteLine("END");
}

// Output: START, EXCEPTION, END
```

</div>
</div>

---

#### Cancellation

- Sometimes, we need to cancel async operation after it's started
- `CancellationTokenSource` - class
    - Manages cancellation tokens
    - Has `Cancel` method which initiates cancellation
- `CancellationToken` - struct
    - Propagates notification that operations should be canceled
    - Cannot be used to initiate cancellation
- **Cancelled task throws `TaskCanceledException` when awaited**
- Cancellation can be initiated automatically after specified period of time
- Most asynchronous methods in the CLR support cancellation tokens

<!--
SPEAKER NOTES:
- Cancellation je riešená cez dve triedy kvoli bezpečnosti:
    - Producent vytvorí CancellationTokenSource objekt z ktorého získa CancellationToken a ten poskytne konzumentovi
    - Konzument tokenu (async method) môže na CancellationToken objekte skontrolovať či neprišiel požiadavok na zrušenie vykonávania
    - Požiadavok na zrušenie je možné vyvolať pomocou Cancel metody na CancellationTokenSource
    - Tým že k metóde Cancel nemá konzument prístup, tak nemôže nastať prípad že by si sám pozastavil vykonávanie
-->

---

#### Best practices

- Add "_Async_" suffix to async method name
- Do not create async methods without `await` in its body
- Prefer `async Task` methods over `async void` methods
- Async all the way - don’t mix blocking and async code
- Do not use `Task.Wait` or `Task.Result` - await the task instead
- Use `Task.Delay` instead of `Thread.Sleep` method
- You can find more tips here: [David Fowler - Async Guidance](https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md)

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

- **Asynchronous** cooking:
    - You start boiling the eggs and in the meantime you prepare the bread
    - You are not waiting until the eggs are finished
    - You are the only one person who is cooking
- **Parallel** cooking:
    - One person prepares the eggs and the other prepares the bread
    - Both activities are running in parallel
    - Both activities are performed by different chefs

---

## Resources

- [Microsoft documentation](https://learn.microsoft.com/en-us/docs/)
- [C# in Nutshell](https://www.amazon.com/gp/product/1098121953?ie=UTF8&tag=cinanu-20&linkCode=as2&camp=1789&creative=9325&creativeASIN=1098121953)
- [Tasks are not threads and they are not for performance](https://steven-giesel.com/blogPost/d095383f-7ea9-4419-96b8-889c6981cce0?utm_source=substack&utm_medium=email)
- [David Fowler - Async Guidance](https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md)