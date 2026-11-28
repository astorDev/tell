# Tell Miniapps Template App

The application is a template for all miniapps created with tell. It repeats behaviour of tell, but with static Makefile (found in the location relative to the executable itself).

A miniapp will only be created with valid makefile paired, so fallback logic is not needed for this app.

## Native AOT publishing

Pass one of the supported runtime identifiers to publish a self-contained executable:

```sh
make publish RID=osx-arm64
make publish RID=osx-x64
make publish RID=win-x64
make publish RID=linux-x64
make publish RID=linux-arm64
```

Run the published executable with the default fixture:

```sh
make run-on-published RID=osx-arm64
```

The [CLI](../cli/README.md) packages the matching app-template executable inside its RID-specific .NET tool package. A local `make install` in the CLI directory builds the default `osx-arm64` package without requiring CI.