namespace Tell;

public enum MakefileTokenKind
{
    /// <summary>
    /// A word used as a target name, prerequisite, variable name, or recipe text.
    /// </summary>
    Word,

    /// <summary>
    /// <c>\t</c> - Marks the start of a recipe line — the command Make will execute for a rule.
    /// </summary>
    Tab,

    /// <summary>
    /// <c>\n</c> - Separates rules, variable assignments, and other directives.
    /// </summary>
    NewLine,

    /// <summary>
    /// <c>:</c> - Separates a target from its prerequisites in a rule.
    /// </summary>
    Colon,

    /// <summary>
    /// <c>::</c> - Defines a double-colon rule, allowing multiple independent definitions for the same target.
    /// </summary>
    DoubleColon,

    /// <summary>
    /// (<c>:=</c>) Immediately-expanded assignment — the value is expanded once at definition time.
    /// </summary>
    ColonEquals,

    /// <summary>
    /// (<c>=</c>) Recursively-expanded assignment — the value is re-expanded every time the variable is used.
    /// </summary>
    Equals,

    /// <summary>
    /// (<c>?=</c>) Conditional assignment — sets the variable only if it has no value yet.
    /// </summary>
    QuestionEquals,

    /// <summary>
    /// (<c>!=</c>) Shell assignment — sets the variable to the stdout output of the given shell command.
    /// </summary>
    BangEquals,

    /// <summary>
    /// <c>+=</c> - Append assignment — adds to the existing variable value rather than replacing it.
    /// </summary>
    PlusEquals,

    /// <summary>
    /// <c>$(</c> - Opens a variable or built-in function reference, e.g. <c>$(VAR)</c> or <c>$(subst ...)</c>.
    /// </summary>
    VariableOpener,

    /// <summary>
    /// <c>)</c> - Closes a variable or built-in function reference.
    /// </summary>
    VariableCloser,

    /// <summary>
    /// <c>\</c> - At end of a line, continues the logical line onto the next physical line.
    /// </summary>
    Backslash,

    /// <summary>
    /// (<c>%</c>) Wildcard used in pattern rules to match any stem, e.g. <c>%.o: %.c</c>.
    /// </summary>
    Percent,

    /// <summary>
    /// (<c>@</c>) When prefixed to a recipe command, suppresses its echoing to stdout during execution.
    /// </summary>
    At,

    /// <summary>
    /// (<c>,</c>) Separates arguments in built-in Make functions, e.g. <c>$(subst from,to,text)</c>.
    /// </summary>
    Comma,

    /// <summary>
    /// (<c>#</c>)
    /// Everything from here to the end of the line; ignored by Make entirely.
    /// </summary>
    Comment,
}
