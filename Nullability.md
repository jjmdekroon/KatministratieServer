# Nullability

In our projects we enforce nullability. All project files should have the `<Nullable>enable</Nullable>` flag so that all our projects are written with these new language features.

## Dtos
Dto's are always written as follows:
```CSharp
public class MyDto 
{ 
    // correct
    public string MyStringProperty { get; init; } = null!;
    public string? MyNullableStringProperty { get; init; }
    public MyInnerDto InnerDto { get; init; } = null!;
    public IReadOnlyCollection<MyInnerDto> MyCollection { get; init; } = Array.Empty<MyInnerDto>();

    // incorrect
    public string MyStringProperty { get; init; } = string.Empty; // not consistent with objects
    public string? MyNullableStringProperty { get; init; } = null!; // not mandatory for nullable fields
    public MyInnerDto InnerDto { get; init; } = new MyInnerDto(); // not consistent and can cause weird side effects
    public IReadOnlyCollection<MyInnerDto> MyCollection { get; init; } = null!; // Collections should be empty as default

}
```

Once the [proposal for required properties](https://github.com/dotnet/csharplang/issues/3630) is implemented in C# we want to utilise this functionality to make everything that should not be `null` required. However this is currently not part of our toolkit.