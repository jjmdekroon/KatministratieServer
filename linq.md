# Linq Coding Guidelines

## Prefer statements instead of query syntax
We use the functional method of Linq, not the declarative way.
```CSharp
// wrong
var result = from s in list
             select s.Name;

// correct
var result = list.Select(s => s.Name);
```


## Chaining Linq Statements

When chaining multiple lines of Linq statements, every statement gets its own line as follows:

```CSharp
// correct
var result = list
    .Where(s => s.Age > 18)
    .Select(s => s.Name)
    .ToList();

// wrong
var result = list.Where(s => s.Age > 18).Select(s => s.Name).ToList();
```
