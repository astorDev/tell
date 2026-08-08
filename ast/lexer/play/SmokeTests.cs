namespace Tell;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Main()
    {
        var exampleMakefile = """
            # Project settings
            AUTHOR = dev@example.com
            ENV ?= dev
            ARGS += --verbose
            COMPOSE := docker-compose -f $(ENV).yml
            LOG_CMD != date --format=%Y-%m-%d
            TAG := v1::beta

            run:
            	@$(COMPOSE) up \
            		--detach

            stop: run
            	@$(COMPOSE) down

            deploy::
            	@echo $(subst dev,prod,$(ENV))
            	@printf Status:%s $(ENV)
            """;

        var tokens = MakefileLexer.Tokenizer.Tokenize(exampleMakefile).ToArray();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}