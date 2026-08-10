MODULE ?= Recipe
SUB ?= Parser
SUB-2 ?= Lexer
COMMAND ?= Tokens

lib:
	dotnet new classlib --name Tell.$(MODULE).$(SUB) --output `cameled $(MODULE)/$(SUB)/lib`
	dotnet sln add `cameled $(MODULE)/$(SUB)/lib` --in-root

lib-2:
	dotnet new classlib --name Tell.$(MODULE).$(SUB).$(SUB-2) --output `cameled $(MODULE)/$(SUB)/$(SUB-2)/lib`
	dotnet sln add `cameled $(MODULE)/$(SUB)/$(SUB-2)/lib` --in-root

cli-play:
	dotnet new cli-play --name Tell.$(MODULE).$(SUB) --output `cameled $(MODULE)/$(SUB)/play`
	make -C `cameled $(MODULE)/$(SUB)/play` -f copaster.Makefile COMMAND=$(COMMAND)
	dotnet sln add `cameled $(MODULE)/$(SUB)/play` --in-root

cli-play-2:
	dotnet new cli-play --name Tell.$(MODULE).$(SUB).$(SUB-2) --output `cameled $(MODULE)/$(SUB)/$(SUB-2)/play`
	make -C `cameled $(MODULE)/$(SUB)/$(SUB-2)/play` -f copaster.Makefile COMMAND=$(COMMAND)
	dotnet sln add `cameled $(MODULE)/$(SUB)/$(SUB-2)/play` --in-root

lib-n-play:
	make lib
	make cli-play
	dotnet add `cameled $(MODULE)/$(SUB)/play` reference `cameled $(MODULE)/$(SUB)/lib`

lib-n-play-2:
	make lib-2
	make cli-play-2
	dotnet add `cameled $(MODULE)/$(SUB)/$(SUB-2)/play` reference `cameled $(MODULE)/$(SUB)/$(SUB-2)/lib`