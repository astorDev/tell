## Tell Installer

This module is dedicated to using tell util for converting a Makefile into a persistent script. For example:

```sh
tell install gitflow.Makefile gf
```

Would convert recipes from the `gitka.Makefile` into a `gf` util with recipes as subcommands, which could be used like this:

```sh
gitka pr
```

```sh
gitka feature-branch
```

The challenge is, of course, dealing with dependencies (scripts in relative folders, etc.)