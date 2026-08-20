NAME ?= Egor
GREETING ?= Servus

greeting:
	echo "$(GREETING), $(NAME)!"
	echo "This vars were used GREETING: $(GREETING), NAME: $(NAME)"

farewell:
	echo "Goodbye, $(NAME)!"