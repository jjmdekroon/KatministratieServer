# Naming and ordering
This section contains guidelines with regards to naming

The general naming of a file should be as follows:

## Static fields
Any static fields are not using the C# convention of prefixing them with `s_` or `t_`, we use the same naming conventions as a normal field of a class.

## Constants
We write constants in UPPERCASE with a `_` seperation. For example: `public const int MY_UNIQUE_IDENTIFIER = 10`


## Private fields
All private fields should be prefixed with an underscore.

```CSharp
public class Person
{
    // correct: private with underscore
    private readonly string _name;

    // wrong: private without underscore
    private readonly string surname;
}
```
