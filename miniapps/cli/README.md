# Tell Miniapps Cli

> 💀 Skeleton - the README describes target state, not current. Source code serves as a template, not implementation.

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

The CLI is a .NET 11 RID-specific Native AOT tool. Its package for each supported RID includes the matching self-contained app template executable under `app-template/`. The .NET CLI selects the matching RID-specific tool package automatically when installing the top-level package.

The supported RIDs are `osx-arm64`, `osx-x64`, `win-x64`, `linux-x64`, and `linux-arm64`. Native AOT requires publishing on the target OS; cross-architecture publishing requires the target toolchain.

Build and install the local macOS ARM64 package with one command:

```sh
make install
```

This publishes the app template for `osx-arm64`, packs the RID-specific CLI package and the top-level pointer package, then installs from the local package directory. Pass `RID=<RID>` to build and install a different RID package on a compatible host.

For distribution, run `make pack-rid RID=<RID>` on a compatible host for each RID, then run `make pack-pointer` once. All RID-specific packages must use the same version and be available before publishing the pointer package. A local macOS install only needs the `osx-arm64` package; CI or developers on other operating systems can build the remaining packages.

## APPS_FOLDER

`APPS_FOLDER` is the per-user root for installed application files, including executables and their supporting files.

| OS      | Path                      | Reasoning                                          |
| ------- | ------------------------- | -------------------------------------------------- |
| macOS   | `~/Applications`          | User-specific location for installed applications. |
| Windows | `%LOCALAPPDATA%\Programs` | User-specific location for installed programs.     |
| Linux   | `~/.local`                | User-specific application prefix.                  |