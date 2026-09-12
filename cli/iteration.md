- [ ] Fix: Bug with Complicated Params. [Details](#bug-with-complicated-params)
- [ ] Fix Bug with Empty String as Default Value. [Details](#bug-with-string-as-default-value) 

## Bug with Complicated Params

```sh
tell paste/cli --file copaster.Makefile in-output Paste --project Copaster
```

Resulted in error:

```text
Required command was not provided.
```

This worked though:

```sh
tell --file=copaster.Makefile paste/cli Paste --project=Copaster
```

--- 

Both

```sh
tell copaster full-pr --branch=magic-implementation-pseudocode --title="magic: Implementation Pseudocode"
```

and

```sh
tell copaster full-pr magic-implementation-pseudocode --title="magic: Implementation Pseudocode" 
```

❌ Resulted in:

```text
Required command was not provided.

Description:
  tell

Usage:
  Tell.Cli [<first> [<second> [<third>]]] [command] [options]

Arguments:
  <first>   The first positional argument. Can be: working directory, target or first argument of the rule.
  <second>  The second positional argument. Can be: target or first argument of the rule.
  <third>   The third positional argument. If present, represents first argument of the rule.

Options:
  --file <file>      The path to the Makefile to use.
  --branch <branch>  The value for the variable 'BRANCH'.
  --title <title>    The value for the variable 'TITLE'.
  -?, -h, --help     Show help and usage information
  --version          Show version information

Commands:
  cli-install              Run the rule 'cli-install'
  copy-reactivity          Run the rule 'copy-reactivity'
  paste-reactivity         Run the rule 'paste-reactivity'
  copy-a-ending            Run the rule 'copy-a-ending'
  buffer-proj              Run the rule 'buffer-proj'
  no-buffer                Run the rule 'no-buffer'
  uninstall-fix-ns         Run the rule 'uninstall-fix-ns'
  install-fix-ns           Run the rule 'install-fix-ns'
  cli-reinstall            Run the rule 'cli-reinstall'
  cli-uninstall            Run the rule 'cli-uninstall'
  copy-io                  Run the rule 'copy-io'
  paste-io                 Run the rule 'paste-io'
  copaste-io               Run the rule 'copaste-io'
  paste-app-folder         Run the rule 'paste-app-folder'
  repaste-reactivity       Run the rule 'repaste-reactivity'
  clean                    Run the rule 'clean'
  round                    Run the rule 'round'
  fresh-buffer             Run the rule 'fresh-buffer'
  remove-buffer            Run the rule 'remove-buffer'
  create-buffer            Run the rule 'create-buffer'
  full-copaster-base       Run the rule 'full-copaster-base'
  copy-copaster-base       Run the rule 'copy-copaster-base'
  paste-copaster-base      Run the rule 'paste-copaster-base'
  full-letters             Run the rule 'full-letters'
  feature-branch <branch>  Run the rule 'feature-branch'
  full-pr <branch>         Run the rule 'full-pr'
  pr <title>               Run the rule 'pr'
  post-pr                  Run the rule 'post-pr'
```

✅ Worked after `cd` to copaster:

```sh
tell full-pr play-with-folder-name-as-command --title="Verified Folder Can Be Command Name Even With Slashes and Dots"
```


## Bug with string as default value

Commmand like this:

```sh
tell copaster/magic/play test
```

on Makefile like this:

```makefile
ARGS ?= example . --name John

run:
	dotnet run -- $(ARGS)

test:
	dotnet test --filter FullyQualifiedName~Playground.$(TEST) --logger "console;verbosity=detailed"
```

Results in

```text
Unhandled exception: System.Collections.Generic.KeyNotFoundException: The placeholder 'TEST' was not found in the replacements dictionary.
   at Tell.Placeholder.Replace(IReadOnlyDictionary`2 replacements)
   at Tell.RecipeFragment.ToCommandFragment(IReadOnlyDictionary`2 variables) in /home/runner/work/tell/tell/makefile/lib/RecipeFragment.cs:line 38
   at Tell.RecipeFragmentExtensions.ToCommandString(IEnumerable`1 fragments, IReadOnlyDictionary`2 variables) in /home/runner/work/tell/tell/makefile/lib/RecipeFragment.cs:line 62
   at Tell.Recipe.ToCommandString(IReadOnlyDictionary`2 variables) in /home/runner/work/tell/tell/makefile/lib/Recipe.cs:line 19
   at Tell.RecipeRunner.Run(Recipe recipe, String workingDirectory, IReadOnlyDictionary`2 variables) in /home/runner/work/tell/tell/makefile/lib/RecipeRunner.cs:line 13
   at Tell.RuleRunner.Run(IEnumerable`1 recipes, String workingDirectory, IReadOnlyDictionary`2 variables) in /home/runner/work/tell/tell/makefile/lib/RuleRunner.cs:line 9
   at Tell.RunRuleCommand.Execute(ParseResult parseResult) in /home/runner/work/tell/tell/makefile/lib/RunRuleCommand.cs:line 22
   at System.CommandLine.Command.<>c__DisplayClass33_0.<<SetAction>b__0>d.MoveNext()
--- End of stack trace from previous location ---
   at System.CommandLine.Invocation.InvocationPipeline.InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken)
```