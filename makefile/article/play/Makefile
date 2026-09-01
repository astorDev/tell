run:
	dotnet run

curl:
	curl http://localhost:5154

rurl-naive:
	make run & sleep 4 && make curl && fg
	
rurl:
	bash -c -i 'make run & sleep 2 && make curl && fg'

yac:
	httpyac send .http --all

play:
	bash -c -i 'make run & sleep 2 && make yac && fg'

kill:
	kill `lsof -t -i:5154`