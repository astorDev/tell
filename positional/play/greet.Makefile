GREETING ?= Hello
NAME ?= Egor
REPEAT ?= $(GREETING), $(NAME)!$(AFTER)

greet:
	echo "$(GREETING), $(NAME)!$(AFTER)"
	echo "Again: $(REPEAT)"