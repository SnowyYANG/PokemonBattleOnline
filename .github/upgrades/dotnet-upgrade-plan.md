# .NET8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET8.0 upgrade.
3. Upgrade `PokemonBattleOnline\PokemonBattleOnline.csproj`
4. Upgrade `PBO.Game\PBO.Game.csproj`
5. Upgrade `PBO.Network\PBO.Network.csproj`
6. Upgrade `PBO.Game.Host\PBO.Game.Host.csproj`
7. Upgrade `PBO.Network.Server\PBO.Network.Server.csproj`
8. Upgrade `PBO.Test\PBO.Test.csproj`
9. Upgrade `PBOServer\PBOServer.csproj`
10. Upgrade `PBO\PBO.csproj`

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name | Description |
|:-----------------------------------------------|:---------------------------:|
| (none) | No projects explicitly excluded |

### Aggregate NuGet packages modifications across all projects

No NuGet package version changes were discovered by the automatic analysis; package updates may be required after code compilation on .NET8.0 and will be determined during the upgrade execution.

### Project upgrade details

This section contains details about each project upgrade and modifications that need to be done in the project.

#### PokemonBattleOnline modifications

Project properties changes:
 - Convert project file to SDK-style project file format.
 - Target framework should be changed from `net48` to `net8.0`.

NuGet packages changes:
 - Review packages after migration and update any packages that are incompatible with .NET8.0.

Feature upgrades / code notes:
 - Replace any usage of .NET Framework-only APIs (for example `System.Web`, `AppDomain` specific behavior, or other Windows-only APIs) with cross-platform alternatives or guard them behind runtime checks.
 - Validate `ClientWebSocket` usage and any custom networking code for compatibility with `System.Net.WebSockets` in .NET8.0.

Other changes:
 - Update project file references and remove legacy `packages.config` if present. Prefer PackageReference.

#### PBO.Game modifications

Project properties changes:
 - Convert project file to SDK-style.
 - Change Target Framework from `net48` to `net8.0`.

NuGet packages changes:
 - Update packages as needed after build.

Feature upgrades / code notes:
 - Review any binary-serialized formats, reflection usage, or APIs that changed across .NET versions.

#### PBO.Network modifications

Project properties changes:
 - Convert to SDK-style.
 - Target framework: `net48` -> `net8.0`.

Code notes:
 - Check custom networking helpers and extension methods for compatibility with `System.Net.Sockets` and `System.Net.WebSockets` in .NET8.0.

#### PBO.Game.Host modifications

Project properties changes:
 - Convert to SDK-style.
 - Target framework: `net48` -> `net8.0`.

Code notes:
 - If this project hosts UI or Windows-specific features, verify if `net8.0-windows` is required. If so, adjust TFM accordingly and ensure runtime-specific APIs are supported.

#### PBO.Network.Server modifications

Project properties changes:
 - Convert to SDK-style.
 - Target framework: `net48` -> `net8.0`.

Code notes:
 - Validate server hosting code, thread/async patterns, and any native interop.

#### PBO.Test modifications

Project properties changes:
 - Convert to SDK-style.
 - Target framework: `net48` -> `net8.0` (or `net8.0` test runner target).

Notes:
 - Update test framework packages (NUnit/xUnit/MSTest) to versions compatible with .NET8.0.

#### PBOServer modifications

Project properties changes:
 - Convert to SDK-style.
 - Target framework: `net48` -> `net8.0-windows` (analysis suggested `net8.0-windows` for this project; if you prefer cross-platform, review Windows API usages and switch to `net8.0` if possible).

Notes:
 - If the project depends on Windows-specific APIs (WinForms/WPF/Registry/COM), keep `net8.0-windows`.

#### PBO modifications

Project properties changes:
 - Convert to SDK-style.
 - Target framework: `net48` -> `net8.0-windows` (analysis suggested `net8.0-windows` for this project).

Notes:
 - Review UI or Windows-specific dependencies and update accordingly.



