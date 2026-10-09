ARGS ?= . --file=fail.Makefile meet --verbose

run:
	dotnet run -- $(ARGS)

unsupported:
	make run ARGS=". --file=fail.Makefile meet Egor --verbose"

apply-v:
	cp Versions/$(V)/Program.cs Program.cs

stash-v:
	mkdir -p Versions/$(V) && cp Program.cs Versions/$(V)/Program.cs

run-v:
	make apply-v
	make run

test:
	dotnet test --filter FullyQualifiedName~Playground.$(TEST) --logger "console;verbosity=detailed"