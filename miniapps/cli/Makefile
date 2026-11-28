ARGS ?= . --name John
RID ?= osx-arm64

run:
	dotnet run -- $(ARGS)

pack-rid:
	make -C ../app publish RID=$(RID)
	dotnet pack -c Release -r $(RID)

pack-pointer:
	dotnet pack -c Release -p:CreateRidSpecificToolPackages=false

install:
	make pack-rid RID=$(RID)
	make pack-pointer
	dotnet tool install --global --add-source ./bin/Release Tell.Miniapps.Cli --allow-downgrade

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