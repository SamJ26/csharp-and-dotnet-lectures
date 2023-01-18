---
marp: true
paginate: true
theme: default
---

# What is .NET?

- Free, **cross-platform**, **open source** developer platform
- Runtime
- Maintained by Microsoft
- Created because of [litigation](https://www.cnet.com/tech/tech-industry/sun-microsoft-settle-java-suit/) about Java “tweaking” on Windows

---

# Why to choose .NET?

- Productivity
- Almost every platform
- Performance
- Security
- Open Source
- Large ecosystem

---

# Productivity

TODO - patrikov slide

---

# Almost every platform

TODO - patrikov slide

\+ novy obrazok z https://devblogs.microsoft.com/dotnet/announcing-dotnet-7/

---

# Performance

TODO - patrikov slide

\+ aktualne performance testy
\+ mozno porovnanie rychlosti .NET 6 a .NET 7

---

# Security

TODO - patrikov slide

---

# Open Source

TODO - patrikov slide

---

# Large ecosystem

TODO - patrikov slide

---

# .NET Foundation

TODO - patrikov slide

---
<!-- IMPLEMENTATIONS AND RELATED STUFF -->

# .NET implementations

- **.NET 5 and later versions** (current version is **.NET 7**)
- .NET Core
    - Implements .NET Standard
    - Cross-platform
- .NET Framework
    - Original implementation of .NET
    - Windows OS only
    - Versions 4.5 and later implement .NET Standard
- Mono/Xamarin
- UWP

---

TODO: pridat obrazok od Patrika

---

# .NET Standard

- Formal specification of .NET APIs that are available on multiple .NET implementations
- **Enables code sharing between different implementations**
- No new versions of .NET Standard will be released
- Recommended version to use is **.NET Standard 2.0**
- **Enables migration from .NET Framework**
- Evolution of Portable Class libraries (PCL)

---

TODO: pridat obrazok od Patrika s .NET standartom

---

# .NET Schedule

**STS** = Short Term Support
**LTS** = Long Term Support - min. 3 years

TODO: pridat obrazok s planom releasov

---
<!-- TOOLING AND RELATED STUFF -->

# .NET SDK

- Set of libraries and tools that allow developers to create .NET apps and libs
- **The thing you need to install to use .NET**
- It contains the following components:
    - .NET CLI
    - .NET runtime and libraries
    - The `dotnet` driver
- Multiple versions can be installed (version selection via `Global.json`)

---

# Global.json

- Defines which .NET SDK version is used when you run .NET CLI commands
- Based on folder structure
- `rollForward` feature to specify an acceptable range of versions

```json
{
  "sdk": {
    "version": "6.0.300",               // Doesn't support version ranges (wildcards)
    "allowPrerelease": true,            // Default value
    "rollForward": "latestFeature"      // See the doc for all possible values
  }
}
```

`dotnet new globaljson --sdk-version 6.0.100`

---

# .NET CLI

- Cross-platform toolchain for developing, building, running, and publishing
- Part of .NET SDK
- Invokes `dotnet` driver - resposible for app or command execution

```bash
dotnet --version    # Get current SDK version
dotnet --list-sdks  # Lists all installed SDKs
dotnet new          # Creates a new project, configuration file, or solution
dotnet restore      # Restores the dependencies and tools of a project
dotnet build        # Builds a project and all of its dependencies
dotnet publish      # Publishes the app and its dependencies to a folder for deployment
dotnet run          # Runs source code
dotnet test         # Run available unit tests
dotnet pack         # Packs the code into a NuGet package
dotnet clean        # Cleans the output of a project
dotnet sln          # Lists or modifies the projects in a .NET solution file
dotnet format       # Formats code to match editorconfig settings
```

---

# Dotnet Tools

- Special NuGet package that contains a **console application**
- The tool binaries are installed in a default directory (`~/.dotnet/tools`)

- **Gloal Tool**
    - Invoke the tool from any directory
    - Same version everywhere

- **Local Tool**
    - Invoke the tool from the installation directory or any of its subdirectories
    - Different directories can use different versions of the tool
    - `manifest` file keeps track of tools that are installed as local to a directory

---

# Roslyn

TODO: continue

---

# MSBuild

- Platform for building applications
- Calls Roslyn for C# code
- **Used in Visual Studio**, but MSBuild doesn't depend on Visual Studio!
- You can orchestrate the build via `.csproj`
- Similar to _maven_ (Java) or _make_ (CLang)

---

# Nuget

- Package manager for .NET platform
- Nuget package
    - Shareable unit of code
    - `.nupkg` file with DLLs and assets
- Responsible for managing dependency tree in projects
- .NET Standard vs Multi-targeting
- [nuget.org](https://www.nuget.org/)

---

![](./images/nuget.png)

---

# Useful learning resources

- **[Microsoft documentation](https://learn.microsoft.com/en-us/docs/)**
- Books:
    - [C# in Nutshell](https://www.amazon.com/gp/product/1098121953?ie=UTF8&tag=cinanu-20&linkCode=as2&camp=1789&creative=9325&creativeASIN=1098121953)
- Youtube:
    - [Rainer Stropek](https://www.youtube.com/@rstropek)
    - [IAmTimCorey](https://www.youtube.com/@IAmTimCorey)
    - [Nick Chapsas](https://www.youtube.com/@nickchapsas)
- Blogs:
    - [Andrew Lock](https://andrewlock.net/)
    - [Jeremy Likness](https://blog.jeremylikness.com/blog)
