ARGS ?= Servus Egor
FIXTURE ?= greet

run:
	cp $(FIXTURE).Makefile bin/Debug/net10.0/Makefile && dotnet run -- $(ARGS)

smoke:
	tell run
	tell run --args "../.. Hi Angela"
	tell run --args "--verbose"
	tell run --args "--help"
	tell run --args "farewell"