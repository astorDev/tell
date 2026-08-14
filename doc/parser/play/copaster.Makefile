COMMAND ?= MyCommandName

main:
	replace --all-cases CommandName $(COMMAND)
	rm -rf copaster.Makefile
