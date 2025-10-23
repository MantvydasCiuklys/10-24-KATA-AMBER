# Task A: First Non-Repeating Character

Given a non-empty string s, return the index of the first character that does not repeat. If no such character exists, return -1. The current implementation is correct but slow—make it 𝑂(n) in time while keeping space reasonable.

## Assumptions

Case-sensitive ('A' ≠ 'a')
ASCII is fine (no special Unicode handling required)
Empty string won't be passed (but feel free to harden if you like)

## Examples

s = "leetcode" → 0 ('l')
s = "loveleetcode" → 2 ('v')
s = "aabb" → -1

## Prerequisites
in .csproj file change the property <ContestantName> to your name, that matches the name of the folder in which you will have your solution.
also change property <ContestantRunTime> to either "c#" or "js" depending on which language you want to use.
### C#
.net sdk is required install it here: https://builds.dotnet.microsoft.com/dotnet/Sdk/9.0.306/dotnet-sdk-9.0.306-win-x64.exe
### JS
node.js is required install it here: https://nodejs.org/dist/v22.21.0/node-v22.21.0-x64.msi