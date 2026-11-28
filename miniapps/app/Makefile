ARGS ?= Servus Egor
FIXTURE ?= greet
RID ?= osx-arm64

run:
	cp $(FIXTURE).Makefile bin/Debug/net11.0/Makefile && dotnet run -- $(ARGS)

publish:
	dotnet publish -c Release -r $(RID) --self-contained true -p:PublishAot=true

run-on-published: publish
	cp $(FIXTURE).Makefile bin/Release/net11.0/$(RID)/publish/Makefile && bin/Release/net11.0/$(RID)/publish/tell-miniapp$(EXECUTABLE_EXTENSION) $(ARGS)

smoke:
	tell run
	tell run --args "../.. Hi Angela"
	tell run --args "--verbose"
	tell run --args "--help"
	tell run --args "farewell"