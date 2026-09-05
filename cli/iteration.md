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