ARGS ?= . --name John

run:
	dotnet run -- $(ARGS)

install:
	dotnet pack -c Release
	dotnet tool install --global --source ./bin/Release Tell.Miniapps.Cli --allow-downgrade

install-from-nuget:
	dotnet tool install --global Tell.Miniapps.Cli

uninstall:
	dotnet tool uninstall --global Tell.Miniapps.Cli

reinstall:
	make uninstall || true
	make install

smoke:
	make run ARGS="init"
	make run ARGS="install ../app/greet.Makefile greeter"
	greeter
	greeter ../../../ Servus Egor

path:
	echo $$PATH | tr ':' '\n'

search-path:
	echo $$PATH | tr ':' '\n' | grep 'miniapps'