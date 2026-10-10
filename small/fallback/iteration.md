# Tell Fallback Module Current Iteration

- [x] Initial Support: Only Working Directory, Filename, Target allowed
- [x] Allow named options e.g. `--name Bob` -> `NAME=Bob`
- [x] 🪲 Fix filename incorrect resolution. [Details](#filename-resolves-incorrectly-when-context-provided)

## Filename resolves incorrectly when context provided

```text
tell cli/examples --file deps.Makefile meet --name=Egor --verbose
⚠️  Failed to parse Makefile: Syntax error (line 3, column 5): unexpected `:`, expected `?=` or `=`.. Trying fallback to make
Fallback params: MakeFallback { WorkingDirectoryChange = cli/examples, Filename = cli/examples/deps.Makefile, RuleName = meet, Arguments = System.String[] }
Attempting to fallback to make with arguments: -C cli/examples -f cli/examples/deps.Makefile meet NAME=Egor
make: cli/examples/deps.Makefile: No such file or directory
make: *** No rule to make target `cli/examples/deps.Makefile'.  Stop.
```

Note: a situation where filename also contains folders, which are not the same as context as possible as well.