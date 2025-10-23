using System.Diagnostics;
using System.IO;
using System.Reflection;

var assembly = Assembly.GetExecutingAssembly();

string? GetMetadata(string key) => assembly
    .GetCustomAttributes<AssemblyMetadataAttribute>()
    .FirstOrDefault(attr => string.Equals(attr.Key, key, StringComparison.OrdinalIgnoreCase))?
    .Value;

var contestantName = GetMetadata("ContestantName");

if (string.IsNullOrWhiteSpace(contestantName))
{
    Console.WriteLine("ContestantName property not set. Update <ContestantName> in TaskA.csproj.");
    return;
}

contestantName = contestantName.Trim();

var runTime = GetMetadata("ContestantRunTime");
runTime = string.IsNullOrWhiteSpace(runTime) ? "c#" : runTime.Trim();
var normalizedRunTime = runTime.ToLowerInvariant();

switch (normalizedRunTime)
{
    case "c#":
    case "cs":
    case "csharp":
        RunCSharpSolution(assembly, contestantName);
        break;
    case "js":
    case "javascript":
        RunJavaScriptSolution(contestantName);
        break;
    default:
        Console.WriteLine($"ContestantRunTime '{runTime}' not supported. Use 'c#' or 'js'.");
        break;
}

static void RunCSharpSolution(Assembly assembly, string contestantName)
{
    var solverType = assembly.GetType($"Contestants.{contestantName}.Solver", throwOnError: false, ignoreCase: true);
    var runMethod = solverType?.GetMethod("RunSmokeTests", BindingFlags.Public | BindingFlags.Static);

    if (runMethod is null)
    {
        Console.WriteLine($"[{contestantName}] C# solver not found. Add a static Solver.RunSmokeTests() or update Program.cs.");
        return;
    }
    runMethod.Invoke(null, null);
}

static void RunJavaScriptSolution(string contestantName)
{
    var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    var contestantFolder = Path.Combine(projectRoot, contestantName);
    var scriptPath = Path.Combine(contestantFolder, "taskA.js");

    if (!File.Exists(scriptPath))
    {
        Console.WriteLine($"[{contestantName}] taskA.js not found. Skipping JS execution.");
        return;
    }

    var startInfo = new ProcessStartInfo
    {
        FileName = "node",
        ArgumentList = { scriptPath },
        WorkingDirectory = contestantFolder,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
    };

    try
    {
        using var process = Process.Start(startInfo);
        if (process is null)
        {
            Console.WriteLine($"[{contestantName}] Failed to start Node.js process.");
            return;
        }

        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (!string.IsNullOrEmpty(stdout))
        {
            Console.Write(stdout);
        }

        if (!string.IsNullOrEmpty(stderr))
        {
            Console.Error.Write(stderr);
        }

        if (process.ExitCode != 0)
        {
            Console.WriteLine($"[{contestantName}] taskA.js exited with code {process.ExitCode}.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[{contestantName}] Unable to run taskA.js: {ex.Message}");
    }
}
