GREETING ?= Servus
NAME ?= World

greet:
	echo "$(GREETING), $(NAME)!"

farewell:
	echo "Goodbye, $(NAME)!"