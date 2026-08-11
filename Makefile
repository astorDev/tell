MODULE ?= Recipe
SUB ?= Parser

lib-0:
	dotnet new classlib --name Tell.$(MODULE) --output `cameled $(MODULE)/lib`
	dotnet sln add `cameled $(MODULE)/lib` --in-root

cli-play-0:
	dotnet new cli-play --name Tell.$(MODULE) --output `cameled $(MODULE)/play`
	dotnet sln add `cameled $(MODULE)/play` --in-root

lib-n-play-0:
	make lib-0
	make cli-play-0
	dotnet add `cameled $(MODULE)/play` reference `cameled $(MODULE)/lib`

lib:
	dotnet new classlib --name Tell.$(MODULE).$(SUB) --output `cameled $(MODULE)/$(SUB)/lib`
	dotnet sln add `cameled $(MODULE)/$(SUB)/lib`

cli-play:
	dotnet new cli-play --name Tell.$(MODULE).$(SUB) --output `cameled $(MODULE)/$(SUB)/play`
	dotnet sln add `cameled $(MODULE)/$(SUB)/play`

lib-n-play:
	make lib
	make cli-play
	dotnet add `cameled $(MODULE)/$(SUB)/play` reference `cameled $(MODULE)/$(SUB)/lib`