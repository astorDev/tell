ARGS ?= tell.recipe.parser . --name John

run:
	dotnet run -- $(ARGS)

apply-v:
	cp Versions/$(V)/Program.cs Program.cs

stash-v:
	mkdir -p Versions/$(V) && cp Program.cs Versions/$(V)/Program.cs

run-v:
	make apply-v
	make run

test:
	dotnet test --filter FullyQualifiedName~Tell.Playground.$(T) --logger "console;verbosity=detailed"