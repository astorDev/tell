ARGS ?= --file example.Makefile Servus

run:
	dotnet run -- $(ARGS)

install:
	dotnet pack -c Release
	dotnet tool install --global --source ./bin/Release Tell.Cli --allow-downgrade

install-from-nuget:
	dotnet tool install --global Tell.Cli --prerelease

uninstall:
	dotnet tool uninstall --global Tell.Cli

reinstall:
	make uninstall || true
	make install