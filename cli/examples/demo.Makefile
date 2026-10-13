ENV ?= Development
PORT ?= 5000

hello:
	echo "Hello from in $(ENV) environment on $(PORT)"

bye:
	echo "Goodbye from in $(ENV) environment on $(PORT)"

test: hello bye