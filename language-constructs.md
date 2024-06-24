# Usage of language constructs

## var vs explicit
In all our code we use `var`. We use well named variable names to describe the meaning of a variable.
```CSharp
public void MyMethod()
{
    // not correct, does not describe what this variable contains
    var result = "JohandeKroon";

    // correct, variable name describes the content.
    var authorName = "Johan de Kroon";
}
```

## Switch expressions
For switch expressions we allow that the default case is omitted.
This for instance is valid:
```CSharp
enum Letter { A, B, C }

var letter = Letter.A;

var asChar = letter switch
{
  Letter.A = > 'A',
  Letter.B => 'B',
  Letter.C => 'C'
};
```


However you must ensure that all cases are defined, if you do not want this behavior you can still add the default case.

