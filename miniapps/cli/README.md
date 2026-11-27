# Tell Miniapps Cli

> 💀 Skeleton

CLI for converting `Makefile` into a global CLI command

1. Ensure applications `<APPS_FOLDER>/miniapps/shims` folder is present in the PATH. (Might require shell restart):

```sh
tell-miniapp init
```

2. Create or update a miniapp from a makefile of choice

```sh
tell-miniapp install <PATH_TO_MAKEFILE> <MINIAPP_ALIAS>
```

- Command copies the Makefile and bundled [app template](../app/README.md) into `<APPS_FOLDER>/tell/miniapps/bundles/<MINIAPP_ALIAS>`
- Creates a shim named `<MINIAPP_ALIAS>` in `<APPS_FOLDER>/miniapps/shims` and points it to the app template in `<APPS_FOLDER>/tell/miniapps/bundles/<MINIAPP_ALIAS>`.

## APPS_FOLDER

`APPS_FOLDER` is the per-user root for installed application files, including executables and their supporting files.

| OS      | Path                      | Reasoning                                          |
| ------- | ------------------------- | -------------------------------------------------- |
| macOS   | `~/Applications`          | User-specific location for installed applications. |
| Windows | `%LOCALAPPDATA%\Programs` | User-specific location for installed programs.     |
| Linux   | `~/.local`                | User-specific application prefix.                  |