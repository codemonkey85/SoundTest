# SoundTest
Just messing around.

Hosted on GitHub Pages: https://codemonkey85.github.io/SoundTest/

Development requires the .NET 11 SDK (11.0.100 or a compatible newer version, as configured in `global.json`).

```sh
dotnet workload install wasm-tools
dotnet build SoundTest.slnx
dotnet test SoundTest.slnx
dotnet run --project SoundTest --launch-profile http
```

There are currently no automated test projects in the solution. The pull request workflow builds with warnings as errors and verifies the Release publish before merge.

To produce the static site for hosting:

```sh
dotnet publish SoundTest/SoundTest.csproj -c Release -o release --nologo
```
