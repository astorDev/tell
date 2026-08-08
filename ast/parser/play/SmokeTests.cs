using System.Text.Json;
using Superpower;
using Superpower.Model;

namespace Tell.Parser.Playground;

[TestClass]
public class SmokeTests
{
    [TestMethod]
    public void Basic()
    {
        var makefile = 
"""
ENV ?= dev
REGION := us-east

run:
	dotnet run

deploy:
	$(MAKE) run
	./deploy.sh $(REGION)

""";

        var tokens = MakefileLexer.Tokenizer.Tokenize(makefile);
        var doc = MakefileParser.DocumentParser.Parse(tokens);

        var docJson = JsonSerializer.Serialize(doc, new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new TokenConverter<MakefileTokenKind>() }
        });

        Console.WriteLine(docJson);

        doc.Assignments.Length.ShouldBe(2);
        doc.Assignments[0].Name.ToStringValue().ShouldBe("ENV");
        doc.Assignments[0].Operator.Kind.ShouldBe(MakefileTokenKind.QuestionEquals);
        doc.Assignments[0].Value.Select(v => v.ToStringValue()).ShouldBe(["dev"]);
        doc.Assignments[1].Name.ToStringValue().ShouldBe("REGION");
        doc.Assignments[1].Operator.Kind.ShouldBe(MakefileTokenKind.ColonEquals);
        doc.Assignments[1].Value.Select(v => v.ToStringValue()).ShouldBe(["us-east"]);

        doc.Rules.Length.ShouldBe(2);
        doc.Rules[0].Target.ToStringValue().ShouldBe("run");
        doc.Rules[0].Prerequisites.ShouldBeEmpty();
        doc.Rules[0].Recipes.Length.ShouldBe(1);
        doc.Rules[0].Recipes[0].Tokens.Select(t => t.ToStringValue()).ShouldBe(["dotnet", "run"]);
        doc.Rules[1].Target.ToStringValue().ShouldBe("deploy");
        doc.Rules[1].Prerequisites.ShouldBeEmpty();
        doc.Rules[1].Recipes.Length.ShouldBe(2);
        doc.Rules[1].Recipes[0].Tokens.Select(t => t.ToStringValue()).ShouldBe(["$(", "MAKE", ")", "run"]);
        doc.Rules[1].Recipes[1].Tokens.Select(t => t.ToStringValue()).ShouldBe(["./deploy.sh", "$(", "REGION", ")"]);
    }

    [TestMethod]
    public void SimpleRule()
    {
        var makefile = 
"""
run:
    dotnet run

""";

        var tokens = MakefileLexer.Tokenizer.Tokenize(makefile);
        var doc = MakefileParser.DocumentParser.Parse(tokens);

        doc.Assignments.ShouldBeEmpty();
        doc.Rules.Length.ShouldBe(1);

        ShouldReflectSimpleRule(doc.Rules[0]);
    }

    [TestMethod]
    public void RuleWithVariable()
    {
        var makefile =
"""
deploy:
    k8s deploy $(REGION)

""";

        var tokens = MakefileLexer.Tokenizer.Tokenize(makefile);
        var doc = MakefileParser.DocumentParser.Parse(tokens);

        doc.Assignments.ShouldBeEmpty();
        doc.Rules.Length.ShouldBe(1);

        var rule = doc.Rules[0];
        rule.Target.ToStringValue().ShouldBe("deploy");
        rule.Prerequisites.ShouldBeEmpty();
        rule.Recipes.Length.ShouldBe(1);
        rule.Recipes[0].Tokens.Select(t => t.ToStringValue()).ShouldBe(["k8s", "deploy", "$(", "REGION", ")"]);
    }

    private static void ShouldReflectSimpleRule(MakefileRule rule)
    {
        rule.Target.ToStringValue().ShouldBe("run");
        rule.Prerequisites.ShouldBeEmpty();
        rule.Recipes.Length.ShouldBe(1);
        rule.Recipes[0].Tokens.Select(t => t.ToStringValue()).ShouldBe(["dotnet", "run"]);
    }

    [TestMethod]
    public void TwoRules()
    {
        var makefile = 
"""
run:
    dotnet run

deploy: run
    ./deploy.sh $(REGION)

""";

        var tokens = MakefileLexer.Tokenizer.Tokenize(makefile);
        var doc = MakefileParser.DocumentParser.Parse(tokens);

        doc.Assignments.ShouldBeEmpty();
        doc.Rules.Length.ShouldBe(2);

        ShouldReflectSimpleRule(doc.Rules[0]);
        AssertDeployRule(doc.Rules[1]);
    }

    private static void AssertDeployRule(MakefileRule rule)
    {
        rule.Target.ToStringValue().ShouldBe("deploy");
        rule.Prerequisites.Select(p => p.ToStringValue()).ShouldBe(["run"]);
        rule.Recipes.Length.ShouldBe(1);
        rule.Recipes[0].Tokens.Select(t => t.ToStringValue()).ShouldBe(["./deploy.sh", "$(", "REGION", ")"]);
    }

    [TestMethod]
    public void RecipeLine()
    {
        var fragment = 
"""
    dotnet run

""";

        var tokens = MakefileLexer.Tokenizer.Tokenize(fragment);
        var recipe = MakefileParser.RecipeLine.Parse(tokens);

        recipe.Tokens.Select(t => t.ToStringValue()).ShouldBe(["dotnet", "run"]);
    }
}

public class TokenConverter<T> : System.Text.Json.Serialization.JsonConverter<Token<T>> where T : struct, Enum
{
    public override Token<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Token<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Kind", value.Kind.ToString());
        writer.WriteString("Value", value.ToStringValue());
        writer.WriteEndObject();
    }
}