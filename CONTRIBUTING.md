# Contributing

Thank you for your interest in contributing to EP Finder!

## Development Setup

EP Finder targets .NET 8. Install the .NET 8 SDK and clone the repository:

```bash
git clone git@github.com:dotorg-projects/data-finder.git
cd data-finder
dotnet restore
```

Before opening a pull request, run:

```bash
dotnet build "EP Finder/EP Finder.csproj" --configuration Release
dotnet test --project "EP Finder Test/EP Finder Test.csproj" --configuration Release --coverage
dotnet format "EP Finder/EP Finder.csproj" --verify-no-changes
dotnet format "EP Finder Test/EP Finder Test.csproj" --verify-no-changes
```

## Reporting Issues

If you find a bug or have a suggestion, please [open an issue](https://github.com/dotorg-projects/data-finder/issues) on GitHub.

Do not report security vulnerabilities in public issues. Follow [SECURITY.md](SECURITY.md).

## Pull Requests

1. Fork the repository
2. Create a branch from `main`
3. Make your changes in small, focused commits
4. Ensure build, tests, coverage, and formatting checks pass
5. Submit a pull request using the repository template

Pull requests should preserve the public API unless the change is explicitly discussed first. EP Finder is published as a NuGet package, so compatibility matters for downstream users.

## Code Quality

- Keep nullable reference types enabled.
- Prefer clear, minimal changes over broad refactors.
- Add or update tests when changing behavior.
- Keep documentation and samples aligned with public API changes.
- Do not introduce generated files or build artifacts into source control.

## DevSecOps

Pull requests are expected to pass GitHub Actions checks for build, tests, formatting, dependency review, CodeQL, Codecov, and SonarQube Cloud when configured.

Snyk scans run on a schedule or manually when `SNYK_TOKEN` is configured by the repository owner.

Repository maintainers should configure:

- `SONAR_TOKEN` for SonarQube Cloud analysis.
- `SNYK_TOKEN` for scheduled or manual Snyk dependency scans.
- `CODECOV_TOKEN` only if tokenless Codecov uploads are not enabled for the public repository.
- Branch protection rules requiring build, test, dependency review, and CodeQL checks before merge.

## License Compatibility

EP Finder is licensed under the [Microsoft Public License (Ms-PL)](LICENSE.txt). By contributing, you agree that your contributions will be licensed under the same license.

Contributions that include code licensed under GPL, LGPL, AGPL, or any other copyleft license are not accepted, as they are incompatible with Ms-PL. Pull requests containing such code will be automatically rejected.

## License

By contributing, you agree that your contributions will be licensed under the [Ms-PL](LICENSE.txt).
