# Tell Miniapps Template App

This is an app, serving as executable for tell miniapps. It acts just like main `tell` cli, but always takes a predefined `Makefile` (the one in the same folder as the executable). 

`Makefile` is assumed to be validated before creating a miniapp, therefore it does not have fallback support, like the main util does.