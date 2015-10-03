# Fade
Yet another tool for Language Recognition

# Getting Startted
## import reference and namespace
```
using Fade;
```

## Lexer Generator
For a input `*.fade` file, you can use it by the following code.
```
LexerGenerater.FromFile("yourDef.fade").Generate("youLexerFile.cs");
```

## Fade file structure
A general Fade tokenizer definition file is like the following.
```
{
    //This is a simple Fade file example.
    "config" : {
        "skip" : [ "Whitespace" ],
        "namespace" : "FadeLexerGenTest",
        "lexRuleName" : "HelloFade"
    },
    "rule" : {
        "Whitespace" : @"[ \t\r\n]",
        "Number": "[0-9]+"
    }
}
```

### Differences between fade file and Json file
The definition of Fade file is just like Json, but not very simliar with Json.
Here are 3 differences with Json. These features are impled by FadeJson.

1. Comments support.
2. Original String support. Like
```
@"\tthis a original string" == "\\tthis a original string"
```
or
```
@"""
Python docstring like
""" == "\nPython docstring like"
```

### Config section
```
"config" : {
        "skip" : [ "Whitespace" ],
        "namespace" : "FadeLexerGenTest",
        "lexRuleName" : "HelloFade"
    }
```
1. "skip" is a rule list. The rule which is in would be skip.
2. The generated class would be in FadeLexerGenTest namespace.
3. The generated class would be named "HelloFade".

### Rule Section
```
"rule" : {
    "Whitespace" : @"[ \t\r\n]",
    "Number": "[0-9]+"
}
```
Lex rule name should begin with a capital letter.

# License
MIT