# Tell Miniapps Cli

> 💀 Skeleton

CLI for converting `Makefile` into a global CLI command

1. Ensure Applications `<APPS_FOLDER>/miniapps/shlims` folder is present in the PATH. (Might require shell restart)

```sh
tell-miniapp init
```

2. Create a miniapp from a makefile of choice 

```sh
tell-miniapp add <PATH_TO_MAKEFILE> <MINIAPP_ALIAS>
```

- Command copies the Makefile and bundled [app template](../app/README.md) into `<APPS_FOLDER>/tell/miniapps/<MINIAPP_ALIAS>`
- Creates a shlim, named `<MINIAPP_ALIAS>` in `<APPS_FOLDER>/miniapps/shlims` and points it to the app template in `<APPS_FOLDER>/tell/miniapps/<MINIAPP_ALIAS>`.