# LINQ in .NET:

Before we begin, I want us to remember a few things:😊

## Difference between Statements and Expressions

- **Statements:**
  - A statement is a complete instruction in a programming language that performs an action.
  - It does not return a value, although it may affect the program state (e.g., changing a variable or calling a function).
  - **Examples:**
    - Assignments: `x = 5;`
    - Conditionals: `if (x > 0) { ... }`
    - Loops: `while (x < 10) { ... }`
  - **Key Feature:** Executed for its effect, not for producing a value.

- **Expressions:**
  - An expression is a combination of values, variables, and operators that is evaluated to produce a value.
  - It can be part of a statement or stand alone.
  - **Examples:**
    - Mathematical operations: `3 + 4`
    - Function calls: `Math.Max(10, 20)`
    - Logical comparisons: `x > y`
  - **Key Feature:** Always evaluates to a value.

---

## Pure vs. Impure Functions

### **Pure Functions:**
- **Always produce the same output for the same input.**
- **Have no side effects** (don’t change external variables or data).
- **Easy to test, debug, and reuse** due to their predictability.
- **Example:** A function that adds two numbers and returns the result.

### **Impure Functions:**
- **Output can vary for the same input.**
- **May cause side effects**, such as :
  - Modifying global variables.
  - Logging to a console.
  - Interacting with a database or API.
  - Reading from or writing to a file.

---
## Higher-Order Functions

1.	Accept Functions as Parameters: we can pass delegates, Func<T>, or Action<T> to other functions.
2.	Return Functions: we can return a delegate or lambda as the result of a function.
3.	Built-in Support: Many LINQ methods, such as Select, Where, and Aggregate, are higher-order functions.


### Examples
 Examples of Higher-Order Functions in .NET
```csharp
//1. Passing Functions as Parameters
//Using Func<T> to pass a function to another method:

class Program
{
    static void Main()
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4 };

        // Passing a lambda to Select (higher-order function)
        var squaredNumbers = numbers.Select(x => x * x);

        Console.WriteLine(string.Join(", ", squaredNumbers)); // Output: 1, 4, 9, 16
    }
}

```
```csharp
//2. Returning Functions
//Returning a Func<T> from a method:

class Program
{
    static void Main()
    {
        var multiplier = CreateMultiplier(3);
        Console.WriteLine(multiplier(5)); // Output: 15
    }

    static Func<int, int> CreateMultiplier(int factor)
    {
        return x => x * factor; // Returns a function
    }
}

```
```csharp
//3. Using Built-In LINQ Methods (Higher-Order Functions)
//LINQ provides several higher-order functions like Where, Select, Aggregate, and more.

class Program
{
    static void Main()
    {
        var numbers = Enumerable.Range(1, 10);

        // Filter and map using Where and Select
        var evenSquares = numbers
            .Where(x => x % 2 == 0)
            .Select(x => x * x);

        Console.WriteLine(string.Join(", ", evenSquares)); // Output: 4, 16, 36, 64, 100
    }
}


```
<h2 style="color:red;">Now we will start with the focus of our topic, which is Language Integrated Query (LINQ):</h2>


## Language Integrated Query (LINQ):
is a feature in .NET that allows querying data in a type-safe and consistent way directly in the programming language. LINQ can work with different types of data sources, such as in-memory collections, databases (SQL), XML, and more.

### LINQ provides two styles for writing queries:
- **Query Syntax:** A declarative style similar to SQL.  
- **Method Syntax:** A functional style using method calls (extension methods).
```csharp
//Query Syntax:

int[] numbers = { 1, 2, 3, 4, 5, 6 };

var evenNumbers = from n in numbers
                  where n % 2 == 0
                  select n;

foreach (var num in evenNumbers)
{
    Console.WriteLine(num); // Output: 2, 4, 6
}
```
```csharp
//Method Syntax:

var evenNumbers = numbers.Where(n => n % 2 == 0);

foreach (var num in evenNumbers)
{
    Console.WriteLine(num); // Output: 2, 4, 6
}
```
### Lazy Loading and Deferred Execution:
```csharp
static void Main(string[] args){

List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

//var evenNumbers = numbers.Where(x => x % 2 == 0);
IEnumerable<int> evenNumbers =
numbers.Where(x => x % 2 == 0); // construction (lazy loading)

numbers.Add(10);
numbers.Add(12);
numbers. Remove(4);

// [1] === > 2, 4, 6,8
// [2] === > 2, 6, 8, 10, 12
foreach (var n in evenNumbers) // enumeration (immediate execution)

Console.Write($" {n}");

Console.ReadKey();
```
### Why i this code it will print 2,6,8,10,12 not 2,4,6,8 🤔?

#### 1.Lazy Loading with IEnumerable:
```csharp
IEnumerable<int> evenNumbers = numbers.Where(x => x % 2 == 0);
```
The Where method does not evaluate the query immediately. Instead, it stores the logic to evaluate which elements of numbers satisfy x % 2 == 0. At this point, evenNumbers is just a representation of the query.
#### 2.Modifications to numbers:
```csharp
numbers.Add(10);
numbers.Add(12); 
numbers.Remove(4);
```
The numbers list is modified after the evenNumbers query is defined. However, because the query has not been executed yet, these changes will affect the result when evenNumbers is enumerated.
#### 3.Enumeration (Execution of the Query):
```csharp
foreach (var n in evenNumbers)
{
    Console.Write($" {n}");
}
```
### Explanation of Query Execution

When the `foreach` loop executes, it triggers the evaluation of the `Where` query. At this point, the **current state** of the `numbers` list is considered:

- **`numbers` contains:** `{ 1, 2, 3, 5, 6, 7, 8, 9, 10, 12 }` (after modifications).
- **The query `x % 2 == 0` is applied to this updated list.**
- **Result:** `{ 2, 6, 8, 10, 12 }`.


### Key Concepts

1. **Lazy Evaluation:**  
   The query is not executed until the `IEnumerable` is enumerated. This ensures the query reflects the state of the list at the time of enumeration, not when the query is defined.

2. **Impact of List Modifications:**  
   Any modifications to the list after defining the query but before execution will affect the query results.  
   - In this example, adding `10` and `12`, and removing `4` changes the outcome of the query.

3. **Deferred Execution:**  
   Lazy loading allows for efficient querying, as the query is not executed until it's actually needed (e.g., when the results are iterated over).

---

## Projection in LINQ

Projection in LINQ refers to transforming data from one form to another. This is achieved using operators such as `Select`, `SelectMany`, and `Zip`. Here's how each operator works:

### 1. Select:
The `Select` operator is used to project or transform each element of a sequence into a new form.

#### Features:
- **Transforms each item in the sequence.**  
- **Commonly used to select properties, perform calculations, or create new objects.**

```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };

// Construct a new type:
var squareNumbers = numbers.Select(x => new { Original = x, Square = x * x });
foreach (var num in squareNumbers)
    Console.WriteLine($"Original: {num.Original}, Square: {num.Square}");

// Project a new property:
var doubledNumbers = numbers.Select(x => x * 2);
Console.WriteLine(string.Join(", ", doubledNumbers));

//  Perform mathematical operations:
var sqrtNumbers = numbers.Select(x => Math.Sqrt(x));
Console.WriteLine(string.Join(", ", sqrtNumbers));

```
#### 2. SelectMany:
The `SelectMany` operator flattens nested sequences into a single sequence. It is often used when each item in a collection contains another collection, and we want to work with all the inner items in a single list.
```csharp
var teams = new List<string[]>
{
    new string[] { "Alice", "Bob" },
    new string[] { "Charlie", "Diana" },
    new string[] { "Eve", "Frank" }
};

// Flattening nested collections:
var allPlayers = teams.SelectMany(team => team);
Console.WriteLine(string.Join(", ", allPlayers));
//output :Alice, Bob, Charlie, Diana, Eve, Frank
```
#### 3. Zip:
The `Zip` operator combines two sequences into a single sequence by applying a specified function. It works element-wise, combining elements from two sequences at the same index.
```csharp
var numbers = new List<int> { 1, 2, 3 };
var words = new List<string> { "One", "Two", "Three" };

// Combining sequences:
var zipped = numbers.Zip(words, (number, word) => $"{number} -> {word}");
Console.WriteLine(string.Join(", ", zipped));
// Output: 1 -> One, 2 -> Two, 3 -> Three
// Note: If one sequence has extra elements, they are ignored. 
// For example:
// numbers = new List<int> { 1, 2, 3, 4 };
// The extra element (4) will be ignored.
// The same applies if the second sequence has extra elements.

var nums1 = new List<int> { 1, 2, 3 };
var nums2 = new List<int> { 4, 5, 6 };
// Performing mathematical operations:
var sums = nums1.Zip(nums2, (a, b) => a + b);
Console.WriteLine(string.Join(", ", sums));
// Output: 5, 7, 9
```

---
# Sorting with LINQ

Sorting in LINQ allows us to order elements in a collection based on one or more criteria. LINQ provides the following main methods for sorting:

### 1. OrderBy:
Sorts elements in ascending order.

### 2. OrderByDescending:
Sorts elements in descending order.

Additionally, we can chain multiple sorting criteria using **ThenBy** and **ThenByDescending** for secondary sorting.


## Example: Sorting by Name and Age

```csharp
var employees = new List<Employee>
{
    new Employee { Name = "Alice", Age = 30 },
    new Employee { Name = "Bob", Age = 25 },
    new Employee { Name = "Alice", Age = 35 }
};

// Sort by Name, then by Age (ascending)
var byNameAndAge = employees
    .OrderBy(e => e.Name)
    .ThenBy(e => e.Age);

Console.WriteLine("Sorted by Name, then by Age:");
foreach (var employee in byNameAndAge)
    Console.WriteLine($"{employee.Name} - {employee.Age}");
```
## Notes on Sorting with LINQ :

### **Note 1: Behavior of Sorting Methods**

The sorting methods `OrderBy`, `OrderByDescending`, `ThenBy`, and `ThenByDescending` return an `IOrderedEnumerable<T>`. 

#### **Key Points About the Returned Value:**

1. **Deferred Execution**:
   - Like most LINQ methods, sorting operations are deferred. This means the query is **not executed** until the collection is iterated (e.g., using a `foreach` loop or `.ToList()`).
   
2. **Does Not Modify the Original Collection**:
   - LINQ methods return a **new sequence** with the sorted elements.
   - The **original collection** remains unchanged.


### **Note 2: Custom Sorting Logic with `IComparer<T>`**

we can define custom sorting logic by implementing the **`IComparer<T>` interface** in C#. This allows us to specify how objects should be compared for sorting or ordering.

#### **Example: Custom Sorting Using `IComparer<T>`**

```csharp
public class EmployeeComparer : IComparer<Employee>
{
    public int Compare(Employee x, Employee y)
    {
        if (x == null || y == null)
            throw new ArgumentNullException("One or both objects to compare are null.");

        // Compare by Name
        int nameComparison = string.Compare(x.Name, y.Name);
        if (nameComparison != 0)
            return nameComparison;

        // If Names are the same, compare by Age (Descending)
        return y.Age.CompareTo(x.Age); // Reverse comparison for descending order
    }
}
```

---
## Partitioning with LINQ:
Data partitioning in LINQ involves dividing a collection into smaller subsets based on specific conditions. LINQ provides three main operators for partitioning data:

#### 1. Take :
Retrieves a specified number of elements from the start of a collection.
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 ,6 ,7, 8 ,9 };
var firstFour = numbers.Take(4);
Console.WriteLine("Using Take:");
foreach (var num in firstFour)
Console.Write(num + " ");  // Output: 1 2 3 4
```
#### 2.	Skip :
Skips a specified number of elements and retrieves the rest.
```csharp
   var numbers = new List<int> { 1, 2, 3, 4, 5 ,6 ,7, 8 ,9 };
   var skipFour = numbers.Skip(4);
   Console.WriteLine("\nUsing Skip:");
   foreach (var num in skipFour)
   Console.Write(num + " ");  // Output: 5 6 7 8 9
```
#### 3.	TakeWhile :
Takes elements from the start of a collection as long as a specified condition is true
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 ,6 ,7, 8 ,9 };
var takeWhileLessThanFive = numbers.TakeWhile(n => n < 5);
Console.WriteLine("\nUsing TakeWhile:");
foreach (var num in takeWhileLessThanFive)
Console.Write(num + " ");  // Output: 1 2 3 4
```

#### 4.	SkipWhile :
Skips elements as long as a specified condition is true and retrieves the remaining elements.
```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 ,6 ,7, 8 ,9 };
var skipWhileLessThanFive = numbers.SkipWhile(n => n < 5);
Console.WriteLine("\nUsing SkipWhile:");
foreach (var num in skipWhileLessThanFive)
  	  Console.Write(num + " ");  // Output: 5 6 7 8 9
```

#### Chunking with LINQ:
Chunking in LINQ refers to splitting a collection into smaller groups (chunks) of a specified size.
```csharp
var numbers = Enumerable.Range(1, 10); // { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
//Split into Chunks of Size 3
var chunks = numbers.Chunk(3);

Console.WriteLine("Chunks:");
foreach (var chunk in chunks)
{
    Console.WriteLine($"[{string.Join(", ", chunk)}]");
}
/*
Chunks:
[1, 2, 3]
[4, 5, 6]
[7, 8, 9]
[10]
*/
```

---

## Quantifiers in LINQ

Quantifiers in LINQ are operators that check whether some or all elements of a sequence satisfy a specific condition. These operators return a **boolean** value (`true` or `false`).

### 1. **Any**
- **Description**: Determines whether **any** element in a sequence satisfies a condition.
- **Returns**: `true` if at least one element matches the condition; otherwise, `false`.
#### 2.	All:
- **Description**: Determines whether all elements in a sequence satisfy a condition.
- **Returns**: Returns `true` if all elements match; otherwise, `false`.
#### 3.	Contains:
- **Description**: Determines whether a sequence contains a specified element.
- **Returns**: Returns `true` if the element exists in the sequence; otherwise, `false`.
```csharp
var users = new List<string> { "Alice", "Bob", "Charlie" };

// Any: Check if there's a user named "Alice".
var hasAlice = users.Any(u => u == "Alice");
Console.WriteLine($"Has Alice? {hasAlice}"); // Output: Has Alice? True

// All: Check if all user names are shorter than 10 characters.
var allShortNames = users.All(u => u.Length < 10);
Console.WriteLine($"All names short? {allShortNames}"); // Output: All names short? True

// Contains: Check if the list contains the user "Charlie".
var containsCharlie = users.Contains("Charlie");
Console.WriteLine($"Contains Charlie? {containsCharlie}"); // Output: Contains Charlie? True
```
## Performance Considerations:
- **Any and Contains may stop early if the condition is satisfied, making them efficient for large datasets.**
- **All checks all elements unless a condition is falsified early.**

---

## Grouping Data with LINQ

Grouping in LINQ allows us to organize elements in a collection into groups based on a specified key. This is useful for scenarios such as categorizing data, summarizing results, or generating reports.

### 1. **GroupBy**
- **Description**: Groups elements in a sequence by a specified key.
- **Returns**: An `IEnumerable<IGrouping<TKey, TElement>>`, where:
  - `TKey` is the key type.
  - `TElement` is the type of elements in each group.
```csharp
var employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Alice", Department = "HR" },
    new Employee { Id = 2, Name = "Bob", Department = "IT" },
    new Employee { Id = 3, Name = "Charlie", Department = "HR" },
    new Employee { Id = 4, Name = "David", Department = "IT" },
    new Employee { Id = 5, Name = "Eve", Department = "Finance" }
};
var groupedByDepartment = employees.GroupBy(e => e.Department);

foreach (var group in groupedByDepartment)
{
    Console.WriteLine($"Department: {group.Key}");
    foreach (var employee in group)
    {
        Console.WriteLine($"  - {employee.Name}");
    }
}
/*
Department: HR
 - Alice
 - Charlie
Department: IT
 - Bob
 - David
*/
```
#### 2.Nested Grouping:
- **Creates groups within groups for hierarchical categorization.**
```csharp
var employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Alice", Department = "HR", Location = "New York" },
    new Employee { Id = 2, Name = "Bob", Department = "IT", Location = "San Francisco" },
    new Employee { Id = 3, Name = "Charlie", Department = "HR", Location = "New York" },
    new Employee { Id = 4, Name = "David", Department = "IT", Location = "Boston" },
    new Employee { Id = 5, Name = "Eve", Department = "Finance", Location = "New York" }
};
//Grouping by Department and Location
var nestedGroups = employees
    .GroupBy(e => e.Department)
    .Select(g => new
    {
        Department = g.Key,
        Locations = g.GroupBy(e => e.Location)
    });

foreach (var departmentGroup in nestedGroups)
{
    Console.WriteLine($"Department: {departmentGroup.Department}");
    foreach (var locationGroup in departmentGroup.Locations)
    {
        Console.WriteLine($"  Location: {locationGroup.Key}");
        foreach (var employee in locationGroup)
        {
            Console.WriteLine($"    - {employee.Name}");
        }
    }
}
/*
Department: HR
  Location: New York
    - Alice
    - Charlie
Department: IT
  Location: San Francisco
    - Bob
  Location: Boston
    - David
Department: Finance
  Location: New York
    - Eve
*/
```
#### 3.ToLookup:
The `ToLookup` operator in LINQ creates a one-to-many dictionary-like structure, where each key maps to a collection of values. It is similar to `GroupBy`, but `ToLookup` always creates a `Lookup<TKey, TElement>` object, which can be queried like a dictionary.

### Features of `ToLookup`:
- **Key Selection**: It groups elements based on a specified key selector.
- **Immediate Execution**: Unlike `GroupBy`, `ToLookup` executes immediately.
- **Indexing**: we can access values by their key using an index, similar to a dictionary.

### Syntax:
```csharp
ILookup<TKey, TElement> lookup = collection.ToLookup(keySelector, elementSelector);
```
```csharp
var employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Alice", Department = "HR" },
    new Employee { Id = 2, Name = "Bob", Department = "IT" },
    new Employee { Id = 3, Name = "Charlie", Department = "HR" },
    new Employee { Id = 4, Name = "David", Department = "IT" },
    new Employee { Id = 5, Name = "Eve", Department = "Finance" }
};
var departmentLookup = employees.ToLookup(e => e.Department);

foreach (var group in departmentLookup)
{
    Console.WriteLine($"Department: {group.Key}");
    foreach (var employee in group)
    {
        Console.WriteLine($"  - {employee.Name}");
    }
}
/*
Department: HR
  - Alice
  - Charlie
Department: IT
  - Bob
  - David
Department: Finance
  - Eve
*/

// Access employees in the HR department
var hrEmployees = departmentLookup["HR"];
foreach (var employee in hrEmployees)
{
    Console.WriteLine(employee.Name);
}

/*
Alice
Charlie
*/
```
## Difference from `GroupBy`:

While both `ToLookup` and `GroupBy` can be used to group data, they have several key differences:

### 1. **Result Type:**
- `ToLookup` always creates a `Lookup<TKey, TElement>`, which is a one-to-many collection (like a dictionary), where each key maps to a collection of values.
- `GroupBy`, on the other hand, returns an `IEnumerable<IGrouping<TKey, TElement>>`, which is a sequence of groups, where each group is an `IGrouping<TKey, TElement>` object containing the key and the grouped elements.

### 2. **Indexing:**
- `ToLookup` supports dictionary-like indexing, which means we can access the groups directly by their key, similar to how we would access items in a dictionary.
- `GroupBy` does not support direct indexing, and we need to iterate through the groups to find the ones we need.

### 3. **Execution Behavior:**
- `ToLookup` executes immediately when called and returns the results right away.
- `GroupBy` uses deferred execution, meaning the query is not executed until the groups are enumerated (e.g., through a `foreach` loop or by calling `ToList()`).

### 4. **Use Cases:**
- **`ToLookup`**: Ideal for situations where we need efficient lookups based on a key and where we may need to access groups directly by their key, as we would with a dictionary. It is suitable for scenarios where we need to categorize and quickly retrieve groups.
- **`GroupBy`**: Useful for grouping elements based on a key when we need to process or transform the grouped data, but do not need immediate indexing. It is better suited for scenarios where the grouping needs to be part of a larger query or transformation.

---

## Join Operations in LINQ

In LINQ, a join operation allows us to combine elements from two or more collections based on a common key. This operation is similar to SQL joins, where we combine data from multiple tables based on a related field.

### 1. **Join**:

The `Join` operation combines elements from two collections based on matching keys. If there is no match, the element is not included in the result. This is equivalent to an **inner join** in SQL.
```csharp
var employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Alice", DepartmentId = 1 },
    new Employee { Id = 2, Name = "Bob", DepartmentId = 2 },
    new Employee { Id = 3, Name = "Charlie", DepartmentId = 1 }
};

var departments = new List<Department>
{
    new Department { Id = 1, Name = "HR" },
    new Department { Id = 2, Name = "IT" }
};

var innerJoin = employees.Join(departments, 
    e => e.DepartmentId, 
    d => d.Id, 
    (e, d) => new { e.Name, DepartmentName = d.Name });

foreach (var item in innerJoin)
{
    Console.WriteLine($"Employee: {item.Name}, Department: {item.DepartmentName}");
}
/*
Employee: Alice, Department: HR
Employee: Bob, Department: IT
Employee: Charlie, Department: HR
*/
```
### 2. **Group Join**:
A `group join` is used to group elements from the second collection into a collection based on the key. The result is a collection of groups where each group contains elements from the second collection that are related to an element from the first collection.
```csharp
var employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Alice", DepartmentId = 1 },
    new Employee { Id = 2, Name = "Bob", DepartmentId = 2 },
    new Employee { Id = 3, Name = "Charlie", DepartmentId = 1 }
};

var departments = new List<Department>
{
    new Department { Id = 1, Name = "HR" },
    new Department { Id = 2, Name = "IT" },
    new Department { Id = 3, Name = "Marketing" }
};

// Group join between employees and departments based on DepartmentId
var groupJoin = departments.GroupJoin(employees, 
    d => d.Id, 
    e => e.DepartmentId, 
    (d, employeesInDepartment) => new { Department = d.Name, Employees = employeesInDepartment });

//((d, employeesInDepartment) => new { Department = d.Name, Employees = employeesInDepartment }): This defines the shape of the result. The first part (d) is the department, and the second part 
//(employeesInDepartment) is the group of employees that match the current department d based on the DepartmentId

foreach (var department in groupJoin)
{
    Console.WriteLine($"Department: {department.Department}");
    foreach (var employee in department.Employees)
    {
        Console.WriteLine($"  Employee: {employee.Name}");
    }
}

/* Output:
Department: HR
  Employee: Alice
  Employee: Charlie
Department: IT
  Employee: Bob
Department: Marketing
*/
```
---

## Generation Operations in LINQ:

In LINQ, generation operations are used to create sequences of data. These operations are useful when we need to generate a sequence based on specific logic, or when we want to create data from scratch.

#### 1. **Enumerable.Range**:
The `Enumerable.Range` method generates a sequence of integers within a specified range. It’s often used when we need to create a series of numbers, like from 1 to N.

```csharp
var range = Enumerable.Range(1, 10);
 // This creates a sequence of numbers from 1 to 10.
```
#### 2. **Enumerable.Repeat**:

The `Enumerable.Repeat` method generates a sequence where all elements are the same value, repeated a specified number of times. It is useful when we need a collection filled with repeated values.
##### What Happens with `Enumerable.Repeat` for Reference Types?
When we use Enumerable.Repeat to generate a sequence of a reference type, it does not create new instances of the object. Instead, it repeats the same reference multiple times. This means all elements in the resulting sequence point to the same instance in memory.
```csharp
class Person
{
    public string Name { get; set; }
}

var person = new Person { Name = "John" };

// Repeat the reference 3 times
var repeatedPersons = Enumerable.Repeat(person, 3).ToList();

// Modify one instance
repeatedPersons[0].Name = "Jane";


foreach (var p in repeatedPersons)
{
    Console.WriteLine(p.Name);
}
/*
Jane
Jane
Jane
*/
//When the Name property of one element (repeatedPersons[0]) is modified to "Jane", it affects all elements because they all point to the same object.
```

#### 3. **Enumerable.Empty**

The `Enumerable.Empty` method returns an empty sequence of a specified type. It's useful when we need an empty collection without creating a new array or list.
```csharp
var empty = Enumerable.Empty<int>();

Console.WriteLine(empty.Any() ? "Has elements" : "Empty");  // Output: Empty
```

---

## Element Operations in LINQ:
Extension methods in LINQ allow us to retrieve specific elements from a collection based on their position or a condition. These methods are useful for working with individual items instead of the entire collection.

#### 1. **First**
- **Returns** the first element of a sequence.
- **Throws** an exception if the sequence is empty.
```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var firstNumber = numbers.First(); // Output: 1

//With a condition :
var firstEven = numbers.First(n => n % 2 == 0); // Output: 2
```
#### 2. **FirstOrDefault**
- **Returns** the first element, or the default value (null for reference types, 0 for value types) if the sequence is empty.
```csharp
var emptyNumbers = new int[0];
var firstOrDefault = emptyNumbers.FirstOrDefault(); // Output: 0
```

#### 3. **Last**
- **Returns** the last element of a sequence.
- **Throws** an exception if the sequence is empty.
```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var lastNumber = numbers.Last(); // Output: 5
//With a condition :
var lastOdd = numbers.Last(n => n % 2 != 0); // Output: 5
```

#### 4. **LastOrDefault**
- **Returns** the last element, or the default value if the sequence is empty.
```csharp
var emptyNumbers = new int[0];
var lastOrDefault = emptyNumbers.LastOrDefault(); // Output: 0
```
#### 5. **Single**
- **Returns** the only element of a sequence.
- **Throws** an exception if:
    -  The sequence contains more than one element.
    -  The sequence is empty.

#### 6. **SingleOrDefault**
The SingleOrDefault method in LINQ is used to retrieve a single element from a collection, but with additional flexibility:
- **If the collection contains exactly one matching element: It returns that element.**
- **If the collection is empty or no elements match the condition: It returns the default value for the type (null for reference types or 0 for numeric value types).**
- **If the collection contains more than one matching element: It throws an exception.**
```csharp
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        // 1. Collection with One Element
        var singleNumber = new[] { 42 };
        var singleResult = singleNumber.SingleOrDefault(); // Output: 42
        Console.WriteLine($"1. Single Element: {singleResult}");

        // 2. Empty Collection
        var emptyNumbers = Array.Empty<int>();
        var emptyResult = emptyNumbers.SingleOrDefault(); // Output: 0 (default for int)
        Console.WriteLine($"2. Empty Collection: {emptyResult}");

        // 3. Multiple Matching Elements
        try
        {
            var multipleNumbers = new[] { 1, 2, 3, 2 };
            var multipleResult = multipleNumbers.SingleOrDefault(x => x == 2);
            Console.WriteLine($"3. Multiple Matching Elements: {multipleResult}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("3. Multiple Matching Elements: Exception - " + ex.Message);
        }

        // 4. Single Matching Element
        var numbersWithMatch = new[] { 1, 2, 3 };
        var singleMatchResult = numbersWithMatch.SingleOrDefault(x => x == 2); // Output: 2
        Console.WriteLine($"4. Single Matching Element: {singleMatchResult}");

        // 5. No Matching Element
        var numbersWithNoMatch = new[] { 1, 2, 3 };
        var noMatchResult = numbersWithNoMatch.SingleOrDefault(x => x == 4); // Output: 0
        Console.WriteLine($"5. No Matching Element: {noMatchResult}");
    }
}
/*
1. Single Element: 42
2. Empty Collection: 0
3. Multiple Matching Elements: Exception - Sequence contains more than one matching element
4. Single Matching Element: 2
5. No Matching Element: 0
*/
```
#### 7. **ElementAt**
- **Returns** the element at a specified index.
- **Throws** an exception if the index is out of range.
```csharp
var numbers = new[] { 1, 2, 3, 4,5 };
var thirdNumber = numbers.ElementAt(2); // Output: 3
```
#### 8. **ElementAtOrDefault**
- **Returns** the element at a specified index, or the default value if the index is out of range.
```csharp
var numbers = new[] { 1, 2, 3, 4,5 };
var outOfRange = numbers.ElementAtOrDefault(10); // Output: 0
```

---
## Equality Operations in LINQ:
LINQ provides methods to compare collections or elements for equality. These methods help us to determine whether two sequences are equivalent or if an element exists in a sequence based on specific equality rules.

#### 1. **SequenceEqual**
- **Compares two sequences for equality, element by element.**
- **The sequences are considered equal if they have the same elements in the same order.**
- **we can use a custom equality comparer for complex types.**

---
## Concatenation in LINQ :
In LINQ, concatenation combines two sequences into a single sequence. The `Concat` method is used to achieve this. It appends the elements of the second sequence to the first sequence, preserving the order of elements.
#### 1. **Concat**
1.	Combines two sequences of the same type into a single sequence.
2.	The resulting sequence includes all elements of the first sequence followed by all elements of the second sequence.
3.	Does not remove duplicates; it simply merges the sequences.
  ```csharp
  //   1. Concat Sequences
static void Main()
    {
        var numbers1 = new[] { 1, 2, 3 };
        var numbers2 = new[] { 4, 5, 6 };

        var combinedNumbers = numbers1.Concat(numbers2);

        Console.WriteLine("Concatenated Numbers:");
        foreach (var number in combinedNumbers)
            Console.WriteLine(number);
    }
/*
Concatenated Numbers:
1
2
3
4
5
6
*/
```
```csharp
//  2-Concat and Extract Specific Properties
class Program
{
    static void Main()
    {
        var employees1 = new[]
        {
            new Employee { Name = "Alice", Department = "HR" },
            new Employee { Name = "Bob", Department = "Finance" }
        };

        var employees2 = new[]
        {
            new Employee { Name = "Charlie", Department = "IT" },
            new Employee { Name = "Diana", Department = "Marketing" }
        };

        var employeeNames = employees1
            .Select(e => e.Name)
            .Concat(employees2.Select(e => e.Name));

        Console.WriteLine("Concatenated Employee Names:");
        foreach (var name in employeeNames)
            Console.WriteLine(name);
    }

    class Employee
    {
        public string Name { get; set; }
        public string Department { get; set; }
    }
}
/*
Concatenated Employee Names:
Alice
Bob
Charlie
Diana
*/
  ```
```csharp
//  2-Concatenation Using Instantiation
var numbers1 = new[] { 1, 2, 3 }; // Existing sequence
var numbers2 = new[] { 4, 5, 6 }; // Existing sequence

// Dynamically create a new sequence
var dynamicNumbers = Enumerable.Range(7, 3); // Generates { 7, 8, 9 }

// Concatenate existing sequences with the new dynamically created sequence
var allNumbers = numbers1.Concat(numbers2).Concat(dynamicNumbers);

Console.WriteLine("\nConcatenation Using Instantiation:");
foreach (var number in allNumbers)
{
    Console.Write(number + " ");
}
// Output: 1 2 3 4 5 6 7 8 9
```
#### Difference between Concatenation Using Instantiation and Concatenation Using Multiple Sequences:
- **Multiple Sequences:** Use this when we already have all the data we need and simply want to merge them.
- **Using Instantiation:** Use this when additional data needs to be generated or fetched dynamically before combining.

---
## Aggregate Operations in LINQ:
Aggregate operations in LINQ are used to perform a reduction or accumulation on a collection. These operations take multiple elements and combine them into a single value.

#### 1. **Aggregate**
The Aggregate function in LINQ is a powerful method that allows us to apply an accumulation or aggregation logic to a sequence of elements. It is often used to reduce a collection to a single value or perform custom aggregation logic.
```csharp
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        // 1. Basic Aggregation (Single Type)
        // This version simply aggregates elements of the same type.
        // Example: Summing up an array of integers.

        var numbers = new[] { 1, 2, 3, 4, 5 };

        var sum = numbers.Aggregate((acc, num) => acc + num);  // Summing up numbers
        Console.WriteLine("Basic Aggregation (Sum): " + sum); // Output: 15

        // 2. Aggregation with Accumulator (Two Types)
        // This version allows us to specify an initial accumulator value.
        // Example: Summing the elements with a custom starting value of 10.

        var sumWithInitialValue = numbers.Aggregate(10, (acc, num) => acc + num);  // Starts with 10 and sums the numbers
        Console.WriteLine("Aggregation with Accumulator (Starting with 10): " + sumWithInitialValue); // Output: 25

        // 3. Aggregation with Result (Three Types)
        // This version allows for complex aggregation and a final result transformation.
        // Example: Calculate both sum and product of the numbers.

        var result = numbers.Aggregate(
            new { Sum = 0, Product = 1 },  // Initial accumulator: an anonymous object with Sum and Product
            (acc, num) => new { Sum = acc.Sum + num, Product = acc.Product * num },  // Aggregation logic
            acc => $"Sum: {acc.Sum}, Product: {acc.Product}"  // Final transformation into a string result
        );
        Console.WriteLine("Aggregation with Result (Sum and Product): " + result); // Output: Sum: 15, Product: 120

        // Additional custom example for clarity
        var customNumbers = new[] { 2, 4, 6, 8 };

        // Basic Aggregation (Single Type)
        var basicSum = customNumbers.Aggregate((acc, num) => acc + num);
        Console.WriteLine("Basic Aggregation (Sum) with custom numbers: " + basicSum); // Output: 20

        // Aggregation with Accumulator (Two Types)
        var customSumWithInitialValue = customNumbers.Aggregate(5, (acc, num) => acc + num);
        Console.WriteLine("Aggregation with Accumulator (Starting with 5): " + customSumWithInitialValue); // Output: 25

        // Aggregation with Result (Three Types)
        var customResult = customNumbers.Aggregate(
            new { Sum = 0, Product = 1 },
            (acc, num) => new { Sum = acc.Sum + num, Product = acc.Product * num },
            acc => $"Sum: {acc.Sum}, Product: {acc.Product}"
        );
        Console.WriteLine("Aggregation with Result (Sum and Product) for custom numbers: " + customResult); // Output: Sum: 20, Product: 3840
    }
}
```
#### 2. **Count**
The Count method returns the number of elements in a sequence. It can also be used with a predicate to count the number of elements that satisfy a given condition.
```csharp
public static void Main()
    {
        var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        
        // Count all elements
        var count = numbers.Count();
        
        Console.WriteLine($"Total count: {count}"); // Output: Total count: 9
        
        // Count even numbers
        var evenCount = numbers.Count(n => n % 2 == 0);
        
        Console.WriteLine($"Even count: {evenCount}"); // Output: Even count: 4
    }
```
#### 3. **Sum**
The Sum method computes the sum of elements in a collection. It can also take a selector function to sum a specific property of complex objects
```csharp
public static void Main()
    {
        var numbers = new[] { 1, 2, 3, 4, 5 };
        
        // Sum of numbers
        var sum = numbers.Sum();
        
        Console.WriteLine($"Sum: {sum}"); // Output: Sum: 15
        
        // Sum of a property of complex objects
        var products = new[] 
        {
            new { Name = "Product1", Price = 10 },
            new { Name = "Product2", Price = 20 },
            new { Name = "Product3", Price = 30 }
        };
        
        var totalPrice = products.Sum(p => p.Price);
        
        Console.WriteLine($"Total price: {totalPrice}"); // Output: Total price: 60
    }
```
#### 4. **Average**
The Average method computes the average (mean) of elements in a sequence. It works the same way as Sum, except that instead of returning the sum, it divides by the count of elements.

```csharp
 public static void Main()
    {
        var numbers = new[] { 1, 2, 3, 4, 5 };
        
        // Average of numbers
        var average = numbers.Average();
        
        Console.WriteLine($"Average: {average}"); // Output: Average: 3
    }
```
#### 5. **Min**
The Min method returns the smallest element in a collection. It can also be used with a selector function to find the minimum value of a property.
```csharp
 public static void Main()
    {
        var numbers = new[] { 10, 20, 30, 40, 50 };
        
        // Minimum value
        var min = numbers.Min();
        
        Console.WriteLine($"Min: {min}"); // Output: Min: 10
        
        // Minimum value of a specific property
        var products = new[] 
        {
            new { Name = "Product1", Price = 10 },
            new { Name = "Product2", Price = 20 },
            new { Name = "Product3", Price = 30 }
        };
        
        var minPrice = products.Min(p => p.Price);
        
        Console.WriteLine($"Min price: {minPrice}"); // Output: Min price: 10
    }
```
#### 6. **Max**
The Max method returns the largest element in a collection. Similar to Min, it can also be used with a selector function to find the maximum value of a property.

```csharp
   public static void Main()
    {
        var numbers = new[] { 10, 20, 30, 40, 50 };
        
        // Maximum value
        var max = numbers.Max();
        
        Console.WriteLine($"Max: {max}"); // Output: Max: 50
        
        // Maximum value of a specific property
        var products = new[] 
        {
            new { Name = "Product1", Price = 10 },
            new { Name = "Product2", Price = 20 },
            new { Name = "Product3", Price = 30 }
        };
        
        var maxPrice = products.Max(p => p.Price);
        
        Console.WriteLine($"Max price: {maxPrice}"); // Output: Max price: 30
    }
```
#### 7. **MaxBy**
This method finds the element with the maximum value based on a specific key or criteria.
#### 8. **MinBy**
Similarly, this method finds the element with the minimum value based on a given key or condition.
```csharp
static void Main()
    {
        var employees = new[]
        {
            new { Name = "John", Age = 30, Salary = 50000 },
            new { Name = "Jane", Age = 25, Salary = 60000 },
            new { Name = "Bob", Age = 35, Salary = 45000 },
            new { Name = "Alice", Age = 28, Salary = 55000 }
        };

        // Finding the employee with the maximum salary
        var maxSalaryEmployee = employees.MaxBy(e => e.Salary);
        Console.WriteLine("Employee with Max Salary:");
        Console.WriteLine($"Name: {maxSalaryEmployee.Name}, Age: {maxSalaryEmployee.Age}, Salary: {maxSalaryEmployee.Salary}");

        // Finding the employee with the minimum salary
        var minSalaryEmployee = employees.MinBy(e => e.Salary);
        Console.WriteLine("\nEmployee with Min Salary:");
        Console.WriteLine($"Name: {minSalaryEmployee.Name}, Age: {minSalaryEmployee.Age}, Salary: {minSalaryEmployee.Salary}");
    }
/*
Employee with Max Salary:
Name: Jane, Age: 25, Salary: 60000

Employee with Min Salary:
Name: Bob, Age: 35, Salary: 45000
*/
// These methodes will return the employee with the highest salary or with Min Salary not the salary it self 
```
## Set operations in LINQ:
#### 1. **Distinct / DistinctBy**
The `Distinct` method removes duplicate elements from a sequence based on the entire object , `DistinctBy` method removes duplicates based on a specific property or key.
```csharp
static void Main()
    {
        var numbers = new[] { 1, 2, 2, 3, 4, 4, 5 };

        var distinctNumbers = numbers.Distinct();

        Console.WriteLine("Distinct:");
        foreach (var number in distinctNumbers)
        {
            Console.WriteLine(number);
        }
    }
/*
Distinct:
1
2
3
4
5
*/
```
```csharp
using System;
using System.Linq;

class Program
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    static void Main()
    {
        var people = new[]
        {
            new Person { Name = "John", Age = 30 },
            new Person { Name = "Jane", Age = 25 },
            new Person { Name = "John", Age = 35 }
        };

        var distinctPeopleByName = people.DistinctBy(p => p.Name).ToList();

        Console.WriteLine("DistinctBy:");
        foreach (var person in distinctPeopleByName)
        {
            Console.WriteLine($"{person.Name}, {person.Age}");
        }
    }
}
/*
DistinctBy:
John, 30
Jane, 25
*/
```
#### 2.  **Except / ExceptBy**
The `Except` method removes elements in the first collection that are also in the second collection. The `ExceptBy` method works similarly but allows you to specify a key or property by which to compare elements.
```csharp
class Program
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    static void Main()
    {
        var set1 = new[]
        {
            new Person { Name = "John", Age = 30 },
            new Person { Name = "Jane", Age = 25 }
        };

        var set2 = new[]
        {
            new Person { Name = "John", Age = 30 }
        };

        var exceptSet = set1.Except(set2).ToList();

        Console.WriteLine("Except:");
        foreach (var person in exceptSet)
        {
            Console.WriteLine($"{person.Name}, {person.Age}");
        }

        var exceptSetByName = set1.ExceptBy(set2, p => p.Name).ToList();

        Console.WriteLine("ExceptBy:");
        foreach (var person in exceptSetByName)
        {
            Console.WriteLine($"{person.Name}, {person.Age}");
        }
    }
}

/*
Output:

Except:
Jane, 25

==> The Except method removes John from set1 because it is present in set2. It uses the entire object for comparison.

ExceptBy:
Jane, 25

==> The ExceptBy method removes John from set1 based on the Name property, but it does not consider Age. So even if John has a different age in the two sets, he will still be excluded because the Name matches.
*/
```

#### 3.  **Intersect / IntersectBy**
The `Intersect` method returns the elements that exist in both sequences. The `IntersectBy` method allows us to intersect based on a specific key or property.
```csharp
using System;
using System.Linq;

class Program
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    static void Main()
    {
        var set1 = new[]
        {
            new Person { Name = "John", Age = 30 },
            new Person { Name = "Jane", Age = 25 },
            new Person { Name = "Jack", Age = 35 }
        };

        var set2 = new[]
        {
            new Person { Name = "John", Age = 30 },
            new Person { Name = "Jack", Age = 35 }
        };

        var intersectSet = set1.Intersect(set2).ToList(); // Finds common elements based on entire object

        Console.WriteLine("Intersect:");
        foreach (var person in intersectSet)
        {
            Console.WriteLine($"{person.Name}, {person.Age}");
        }

        var intersectSetByName = set1.IntersectBy(set2, p => p.Name).ToList(); // Finds common elements based on Name property

        Console.WriteLine("IntersectBy:");
        foreach (var person in intersectSetByName)
        {
            Console.WriteLine($"{person.Name}, {person.Age}");
        }
    }
}

/*
Output:

Intersect:
John, 30
Jack, 35

==> The Intersect method finds common elements between set1 and set2 based on the entire object. So, John and Jack are the common people in both sets.

IntersectBy:
John, 30
Jack, 35

==> The IntersectBy method finds common elements between set1 and set2 based on the Name property. Even if the ages differ, the intersection is based on the name, so both John and Jack are still considered common.
*/

```
 
#### 4.  **Union / UnionBy**
The `Union` method combines two sequences and removes duplicates. The `UnionBy` method performs the same operation but allows us to specify a key or property to compare.
```csharp
 using System;
using System.Linq;

class Program
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    static void Main()
    {
        var set1 = new[]
        {
            new Person { Name = "John", Age = 30 },
            new Person { Name = "Jane", Age = 25 }
        };

        var set2 = new[]
        {
            new Person { Name = "John", Age = 35 },
            new Person { Name = "Jack", Age = 40 }
        };

        var unionSet = set1.Union(set2).ToList(); // Combines set1 and set2 and removes duplicates based on entire object

        Console.WriteLine("Union:");
        foreach (var person in unionSet)
        {
            Console.WriteLine($"{person.Name}, {person.Age}");
        }

        var unionSetByName = set1.UnionBy(set2, p => p.Name).ToList(); // Combines set1 and set2 and removes duplicates based on Name property

        Console.WriteLine("UnionBy:");
        foreach (var person in unionSetByName)
        {
            Console.WriteLine($"{person.Name}, {person.Age}");
        }
    }
}

/*
Output:

Union:
John, 30
Jane, 25
John, 35
Jack, 40

==> The Union method combines both sets and removes duplicates based on the entire object. Even though John appears in both sets, it includes both occurrences with different ages.

UnionBy:
John, 30
Jane, 25
Jack, 40

==> The UnionBy method combines both sets and removes duplicates based on the Name property. In this case, it includes only the first John (with Age 30) and Jack from set2, removing the second John (with Age 35) because they share the same name.
*/

```
---

## IEnumerable Vs IQueryable Interfaces :


| Feature                 | **IEnumerable**                                    | **IQueryable**                                          |
|-------------------------|----------------------------------------------------|--------------------------------------------------------|
| **Namespace**            | `System.Collections.Generic`                      | `System.Linq`                                          |
| **Purpose**              | Used for iterating over in-memory collections.     | Extends `IEnumerable` for querying data sources with deferred execution and translation to query language (e.g., SQL). |
| **Execution**            | Operations are executed in-memory immediately.     | Enables deferred execution and query translation. Operations are executed when the data is enumerated. |
| **Lazy Loading**         | Supports lazy evaluation but loads all data into memory first. | Supports lazy evaluation, and may perform the operation on the database server rather than in-memory. |
| **Usage**                | Typically used for querying in-memory collections like arrays, lists, etc. | Primarily used for querying databases through ORM tools like Entity Framework or LINQ to SQL. |
| **Example**              | `IEnumerable<int> numbers = new List<int> { 1, 2, 3, 4 };`<br>`var result = numbers.Where(x => x > 2);` | `IQueryable<int> numbers = dbContext.Numbers;`<br>`var result = numbers.Where(x => x > 2);`<br>`// The query is translated to SQL and executed on the database.` |

### IEnumerable<T> and IQueryable<T> execution behavior : 
```cshrp
public class Card
{
    public int Value { get; set; }
    public string Suit { get; set; }
}

var deck = new List<Card>
{
    new Card { Value = 3, Suit = "Hearts" },
    new Card { Value = 6, Suit = "Spades" },
    new Card { Value = 10, Suit = "Clubs" },
    new Card { Value = 2, Suit = "Diamonds" }
};
```
- **`IEnumerable`<T> (Immediate Execution)**
```cshrp
var queryIEnumerable = deck
    .Where(x => x.Value > 5)  // Filters cards with value greater than 5
    .OrderBy(x => x.Value)    // Orders the result by Value
    .ToList();                // Immediately executes the query and stores the result in a list
```
Query Execution Is Immediate:
Since we use ToList(), the query runs as soon as it is created, which means all operations (filtering, ordering, etc.) are applied directly to the data.

- **`IQueryable`<T> (Deferred Execution)**
 ```cshrp
var deckQueryable = deck.AsQueryable()  // Convert deck to IQueryable
    .Where(x => x.Value > 5)            // Filter the cards where Value > 5
    .OrderBy(x => x.Value);             // Sort the result by Value
```
Deferred Execution:
The query is not executed until we actually enumerate over it (e.g., using ToList(), ToArray(), or a foreach loop). At that point, the query is sent to the data source (e.g., a database), and the appropriate SQL query is generated based on the expression tree. 

####  so :
- we Use IEnumerable<T> when we're working with data that is already in memory and don't need to delay execution.
- we Use IQueryable<T> when we're working with remote data (e.g., a database) and want the query to be executed only when needed, allowing the provider to optimize the query.
---
## Immediate Execution and Deferred Execution :
- **1. Immediate Execution**
In immediate execution, the query is executed right away when it is declared. The data source is read, the operations are applied, and the result is stored immediately. This execution type occurs when we use methods like ToList(), ToArray(), Count(), First(), etc.
 ```cshrp
var numbers = new int[] { 8, 2, 3, 4, 1, 6, 5, 7, 9 };
var list = numbers
    .Where(x => x > 5)    // Filters out: 8, 6, 7, 9  
    .Take(2)              // Takes the first 2: 8, 6 
    .ToList();            // Executes immediately and stores the result in a list
```
In Immediate Execution, LINQ will process all the elements in the filtered sequence (i.e., it checks every element in the filtered sequence to make sure the Take(2) condition is met), but it will stop processing once it reaches the specified number (in this case, 2) of elements to take.
- **2. Deferred Execution**
In deferred execution, the query does not execute immediately. Instead, it sets up a query expression, which only runs when the data is enumerated (e.g., when we iterate over it in a loop or force execution).
 ```cshrp
var numbers = new int[] { 8, 2, 3, 4, 1, 6, 5, 7, 9 };
var list = numbers
    .Where(x => x > 5)    // Filters out: 8, 6, 7, 9  
    .Take(2)              // Takes the first 2: 8, 6 
    .ToList();            // Executes immediately and stores the result in a list
```
In Deferred Execution ,it will process elements on demand and stop once it has fetched the required number of items (e.g., the first 2 matching elements). It does not continue to the last element if the required number is already found.

---
## Extension Methods:
Before, when we were talking about this keyword, we used it as:
1. Accessing Instance Members
The this keyword can be used to reference the current instance's fields, properties, or methods when they are shadowed by local variables or parameters.
2. Calling Other Constructors (Constructor Chaining)
The this keyword can be used to call another constructor in the same class.
3. Passing the Current Instance
The this keyword can be used to pass the current instance of the class as an argument to another method or constructor.
4. The fourth use is the Extension Method:
#### Defining an Extension Method:
Extension methods are a powerful feature in C# that allows developers to add new methods to an existing type without modifying the original type or creating a new derived type. These methods appear as if they are part of the type being extended and can be called on instances of that type directly.
#### Rules for Extension Methods:
1.	Static Class: The method must be defined in a static class.
2.	Static Method: The extension method itself must be static.
3.	this Keyword: The first parameter must use this to define the type being extended.
```csharp
public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public static class EmployeeExtensions
{
    public static string GetFullName(this Employee employee)
    {
        return $"{employee.FirstName} {employee.LastName}";
    }
}

class Program
{
    static void Main()
   {
        var employee = new Employee { FirstName = "John", LastName = "Doe" };
        Console.WriteLine(employee.GetFullName()); // Output: John Doe
    }
}
```
---
## What Are Expression Trees?

Imagine we write a formula like this in C#:

```csharp
x => x + 10
```
This is a lambda expression. Normally, the computer would just run this formula and give us the result.
But with expression trees, the computer instead saves our formula as a diagram—a tree—so we can look at or modify the formula before running it.

### Why Is This Useful?

Expression trees allow us to:
- **Build queries at runtime**: Create and combine conditions dynamically instead of hardcoding them.
- **Analyze formulas**: Check what operations are being performed in the code.
- **Transform queries**: Convert a LINQ query into a SQL statement for a database.

--

## Visual Representation

Here’s how the expression `x => x + 10` is represented as a tree:
```
 +
/ \
x  10
```
---

## Definition

Expression trees are representations of code as a tree-like structure where:
- Each **node** represents a part of the code (e.g., operators, constants, variables).
- They allow inspection, modification, and runtime execution of expressions.

---

### Example

Here’s an example showing how the code can be divided into an expression tree:
😊
```csharp
using System.Linq.Expressions;
internal class Program
{
    static void Main(string[] args)
    {
        // If we want to represent the expression x => x + 10, we break it into three parts:

        // Step 1: Define a parameter 'x' (the variable in the expression)
        ParameterExpression x = Expression.Parameter(typeof(int), "x");

        // Step 2: Define a constant '10' (the value being added)
        ConstantExpression ten = Expression.Constant(10);

        // Step 3: Define the operation 'x + 10' as a BinaryExpression
        BinaryExpression add = Expression.Add(x, ten);

        // Combine the parameter and operation into a lambda expression (Func<int, int>)
        var lambda = Expression.Lambda<Func<int, int>>(add, x).Compile();

        // Execute the compiled lambda with a value (e.g., 5) and print the result
        Console.WriteLine(lambda(5)); // Output: 15
    }
}

// Using Lambda Expressions
internal class Program
{
    static void Main(string[] args)
    {
        // Define a simple lambda expression to check if a number is negative
        Expression<Func<int, bool>> IsNegativeExpression = (num) => num < 0;

        // Accessing the parameters of the lambda expression:
        // The lambda’s parameters are stored in an array. Here, we retrieve the first parameter.
        // Example:
        // Expression<Func<int, int, bool>> expression = (num1, num2) => num1 < num2;
        // To access the parameters:
        // - ParameterExpression num1 = expression.Parameters[0];
        // - ParameterExpression num2 = expression.Parameters[1];
        // Note: Accessing an invalid index will throw an error.
        ParameterExpression numParam = IsNegativeExpression.Parameters[0];

        // Accessing the lambda's body:
        // The 'Body' of the lambda holds the main logic of the expression.
        // Here, the body contains the BinaryExpression 'num < 0'.
        BinaryExpression operation = (BinaryExpression)IsNegativeExpression.Body;

        // Decomposing the BinaryExpression:
        // - The left side of the operation is the parameter ('num').
        // - The right side is the constant (0).
        ParameterExpression left = (ParameterExpression)operation.Left;
        ConstantExpression right = (ConstantExpression)operation.Right;

        // Displaying the decomposed parts of the expression
        Console.WriteLine($"Decomposed Expression: " +
            $"{numParam.Name} => {left.Name} {operation.NodeType} {right.Value}");

        Console.ReadKey();
    }
}

// Without Using Lambda Expressions
internal class Program
{
    static void Main(string[] args)
    {
        // Representing the expression (num) => num % 2 == 0 step-by-step:

        // Step 1: Define the parameter 'num'
        ParameterExpression numParam = Expression.Parameter(typeof(int), "num");

        // Step 2: Define the constants (2 and 0)
        ConstantExpression twoParam = Expression.Constant(2, typeof(int));
        ConstantExpression zeroParam = Expression.Constant(0, typeof(int));

        // Step 3: Define the modulo operation (num % 2)
        // This creates a BinaryExpression where the left operand is 'num' and the right operand is '2'.
        BinaryExpression moduloBinaryExpression = Expression.Modulo(numParam, twoParam);

        // Step 4: Define the equality operation (num % 2 == 0)
        BinaryExpression isEvenBinaryExpression = Expression.Equal(moduloBinaryExpression, zeroParam);

        // Step 5: Combine the expressions into a lambda: (num) => num % 2 == 0
        Expression<Func<int, bool>> IsEvenExpression = Expression.Lambda<Func<int, bool>>(
            isEvenBinaryExpression, new ParameterExpression[] { numParam });

        // Compile and execute the lambda to test for even numbers
        var isEven = IsEvenExpression.Compile();
        Console.WriteLine(isEven(10)); // True
        Console.WriteLine(isEven(9));  // False

        Console.ReadKey();
    }
}

```
