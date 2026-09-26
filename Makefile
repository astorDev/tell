lib:
	dotnet new lib --output $(MODULE)/lib
	copaster-magic $(MODULE)/lib --project=Tell --module=$(MODULE)

play-lib-ref:
	dotnet add `cameled $(MODULE)/play` reference `cameled $(MODULE)/lib`

cli-play:
	dotnet new cli-play --name Tell.$(MODULE) --output `cameled $(MODULE)/play`
	copaster-magic `cameled $(MODULE)/play` --command $(COMMAND)

lib-n-play:
	tell lib $(MODULE)
	tell cli-play $(MODULE)
	dotnet add `cameled $(MODULE)/play` reference `cameled $(MODULE)/lib`

feature-branch:
	git switch --create $(BRANCH)

pr:
	test "$$(git default-branch)" != "$$(git branch --show-current)" || throw "Current branch is default ($$(git branch --show-current)). This is likely a mistake, PRs should be created from a feature branch."
	git save "$(TITLE)"
	gh pr create --title "$(TITLE)" --body "" || true
	gh pr view --web

full-pr:
	test "$$(git default-branch)" == "$$(git branch --show-current)" || throw "Current branch is NOT default ($$(git branch --show-current)). Full PR is only possible from the default branch."
	git switch --create $(BRANCH)
	git save "$(TITLE)"
	gh pr create --title "$(TITLE)" --body "" || true
	gh pr view --web

post-pr:
	git default-and-burn
	git pull