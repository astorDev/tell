MODULE ?= Recipe
SUB ?= Parser

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