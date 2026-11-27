- cwd-less apps. [Details](#cwd-less)

### CWD-less

Some command does not work on any workDir. For them leaving default `workDir` environment is wrong. For example `path` miniapp would still propose `<workDir>` although it doesn't make any sense for it.

```text
Description:
  Executes commands defined in the Makefile

Usage:
  tell [<workDir>] [command] [options]

Arguments:
  <workDir>  The working directory.

Options:
  --file <file>                       The file to read commands from 
                                      [default: Makefile]
  -v, --verbose                       Set Minimum Log Level to Trace
  --quiet                             Sets Minimum Log Level to Error
  --log-level                         Sets Minimum Log Level to the 
  <None|Trace|Debug|Information|Warn  Specified Level
  ing|Error|Critical>
  -?, -h, --help                      Show help and usage information
  --version                           Show version information

Commands:
  path             Executes: echo $PATH | tr ':' '\n'
  search <search>  Executes: echo $PATH | tr ':' '\n' | grep '$(SEARCH)' 
                   [default: ""]
```