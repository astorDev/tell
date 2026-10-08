COMMAND ?= Run

lib:
	dotnet new lib --output `cameled $(MODULE)/lib`
	copaster-magic $(MODULE)/lib --project=Tell --module=$(MODULE)

play-lib-ref:
	dotnet add `cameled $(MODULE)/play` reference `cameled $(MODULE)/lib`

cli-play:
	dotnet new cli-play --name Tell.$(MODULE) --output `cameled $(MODULE)/play`
	copaster-magic `cameled $(MODULE)/play` --command $(COMMAND)

lib-n-play:
	dotnet new sln --name Tell.Small
	tell lib $(MODULE)
	tell cli-play $(MODULE) --command $(COMMAND)
	dotnet add `cameled $(MODULE)/play` reference `cameled $(MODULE)/lib`
	cd .. && dotnet sln add `cameled small/$(MODULE)/play` --in-root
	rm Tell.Small.slnx