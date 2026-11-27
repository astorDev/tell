# Tell Miniapps Cli

> 💀 Skeleton

CLI for converting a `Makefile` into a global CLI command.

1. Ensure `<APPS_FOLDER>/tell/miniapps/shims` is present in `PATH`:

```sh
tell-miniapp init
```

`init` adds the shims directory to the detected user-level `PATH` configuration only if it is not already present. If it adds the directory, restart the shell to apply the change.

2. Create or update a miniapp from a `Makefile`:

```sh
tell-miniapp install <PATH_TO_MAKEFILE> <MINIAPP_ALIAS>
```

- Copies the `Makefile` and the self-contained Native AOT [app template](../app/README.md) executable into `<APPS_FOLDER>/tell/miniapps/bundles/<MINIAPP_ALIAS>`.
- Creates a shim named `<MINIAPP_ALIAS>` in `<APPS_FOLDER>/tell/miniapps/shims` that invokes the executable in that bundle and forwards its arguments. On Windows, the shim is named `<MINIAPP_ALIAS>.cmd`.

## Distribution packaging

The CLI distribution includes a prebuilt, self-contained app template executable for each supported runtime identifier: `osx-arm64`, `osx-x64`, `win-x64`, `linux-x64`, and `linux-arm64`. The `install` command uses the executable matching the current platform.

## APPS_FOLDER

`APPS_FOLDER` is the per-user root for installed application files, including executables and their supporting files.

| OS      | Path                      | Reasoning                                          |
| ------- | ------------------------- | -------------------------------------------------- |
| macOS   | `~/Applications`          | User-specific location for installed applications. |
| Windows | `%LOCALAPPDATA%\Programs` | User-specific location for installed programs.     |
| Linux   | `~/.local`                | User-specific application prefix.                  |