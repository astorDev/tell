case-1:
	dotnet run -- --file greet.Makefile

case-2:
	dotnet run -- --file greet.Makefile --help

case-3:
	dotnet run -- --file greet.Makefile Servus Jack

all-cases: case-1 case-2 case-3