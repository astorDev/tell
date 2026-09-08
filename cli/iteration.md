- [ ] Fix: Bug with Complicated Params. [Details](#bug-with-complicated-params)

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

Resulted in:

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