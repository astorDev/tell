## Tell Mini Apps

This module is dedicated to using tell util for converting a Makefile into a persistent script. For example:

```sh
tell-miniapp install gitflow.Makefile gf
```

Would convert recipes from the `gitflow.Makefile` into a `gf` util with rules as subcommands, which could be used like this:

```sh
gf pr
```

```sh
gf feature-branch
```

The challenge is, of course, dealing with dependencies (scripts in relative folders, etc.)