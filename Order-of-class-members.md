# Order of class members

There is no guideline describing us how we should order our members in our class. Therefore, we fall back to the following order within a class:

First group your class in member type:
- Fields
- Properties
- Constructors
- Methods
- Child classes
- Child structs

Then group each member type by its access scope (most accessible to least accessible)
- public
- internal
- protected
- private

Then group each access type by instance and class scope:
- static
- non static

Finally group members that can be readonly by readonly and non-readonly:
- readonly
- non-readonly


## Example class containing most common patterns that we use in Rademaker

```CSharp
public class MyClass
{
    private readonly IDependency _dependency;

    private int _number;

    public string MyReadOnlyName { get; } // this is readonly, thus should be grouped at readonly

    public string MyName { get; set; }

    protected abstract ImplementMe { get; }

    public MyClass() { }
    private MyClass() { }

    public static MyClass Create() { }

    public void MyMethod() { }

    private static void DoSomethingStatic() { } 

    private void MyHelperMethod() { } 
}
```