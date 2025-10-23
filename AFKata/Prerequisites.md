# Prerequisites

in .csproj file change the property <ContestantName> to your name, that matches the name of the folder in which you will have your solution.
also change property <ContestantRunTime> to either "c#" or "js" depending on which language you want to use.

## C#

.net sdk is required install it here: https://builds.dotnet.microsoft.com/dotnet/Sdk/9.0.306/dotnet-sdk-9.0.306-win-x64.exe

## JS

node.js is required install it here: https://nodejs.org/dist/v22.21.0/node-v22.21.0-x64.msi

### TASKS

If you want to debug/run your task you can use the following command: dotnet run
it will run either c# or js depending on the value of <ContestantRunTime> property in the .csproj file.
For each specific task you have to change the property <ContestantTask> to the name of the task.
for example TaskA, TaskB, etc.
