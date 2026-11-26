GREETING ?= Hello
NAME ?= World

greet:
	echo "$(GREETING), $(NAME)!"
	echo "A little info about cwd:"
	ls
	pwd

farewell:
	echo "Goodbye, $(NAME)!"