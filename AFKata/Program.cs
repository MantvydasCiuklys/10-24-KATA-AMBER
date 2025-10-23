using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

var assembly = Assembly.GetExecutingAssembly();

string? GetMetadata(string key) => assembly
    .GetCustomAttributes<AssemblyMetadataAttribute>()
    .FirstOrDefault(attr => string.Equals(attr.Key, key, StringComparison.OrdinalIgnoreCase))?
    .Value;

var contestantName = GetMetadata("ContestantName");
if (string.IsNullOrWhiteSpace(contestantName))
{
    Console.WriteLine("ContestantName property not set. Update <ContestantName> in the project file or pass -p:ContestantName=... when running.");
    return;
}
contestantName = contestantName.Trim();

var taskName = NormalizeTaskName(GetMetadata("ContestantTask"));
var runTimeRaw = GetMetadata("ContestantRunTime");
var runTime = NormalizeRunTime(runTimeRaw);

switch (runTime)
{
    case RunTime.CSharp:
        RunCSharpSolution(assembly, contestantName, taskName);
        break;
    case RunTime.JavaScript:
        RunJavaScriptSolution(contestantName, taskName);
        break;
    case RunTime.Both:
        RunCSharpSolution(assembly, contestantName, taskName);
        RunJavaScriptSolution(contestantName, taskName);
        break;
    default:
        var reported = string.IsNullOrWhiteSpace(runTimeRaw) ? "<empty>" : runTimeRaw.Trim();
        Console.WriteLine($"ContestantRunTime '{reported}' not supported. Use 'c#', 'js', or 'all'.");
        break;
}

static void RunCSharpSolution(Assembly assembly, string contestantName, string taskName)
{
    var solverType = assembly.GetType($"Contestants.{contestantName}.{taskName}.Solver", throwOnError: false, ignoreCase: true)
        ?? assembly.GetType($"Contestants.{contestantName}.Solver", throwOnError: false, ignoreCase: true);

    var runMethod = solverType?.GetMethod("RunSmokeTests", BindingFlags.Public | BindingFlags.Static);

    if (runMethod is null)
    {
        Console.WriteLine($"[{contestantName}/{taskName}] C# solver not found. Expected static Solver.RunSmokeTests().");
        return;
    }

    Console.WriteLine($"[{contestantName}/{taskName}]");
    runMethod.Invoke(null, null);
}

static void RunJavaScriptSolution(string contestantName, string taskName)
{
    var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    var taskFolder = Path.Combine(projectRoot, "Tasks", taskName);
    var contestantFolder = Path.Combine(taskFolder, contestantName);
    var scriptPath = Path.Combine(contestantFolder, "task.js");

    if (!File.Exists(scriptPath))
    {

        var legacyFolder = Path.Combine(projectRoot, contestantName);
        var legacyScript = Path.Combine(legacyFolder, $"task{taskName}.js");

        if (File.Exists(legacyScript))
        {
            contestantFolder = legacyFolder;
            scriptPath = legacyScript;
        }
    }

    if (!File.Exists(scriptPath))
    {
        Console.WriteLine($"[{contestantName}/{taskName}] task.js not found. Skipping JS execution.");
        return;
    }

    var startInfo = new ProcessStartInfo
    {
        FileName = "node",
        WorkingDirectory = contestantFolder,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,
    };
    startInfo.ArgumentList.Add(scriptPath);

    try
    {
        using var process = Process.Start(startInfo);
        if (process is null)
        {
            Console.WriteLine($"[{contestantName}/{taskName}] Failed to start Node.js process.");
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
            Console.WriteLine($"[{contestantName}/{taskName}] task.js exited with code {process.ExitCode}.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[{contestantName}/{taskName}] Unable to run task.js: {ex.Message}");
    }
}

static string NormalizeTaskName(string? raw)
{
    if (string.IsNullOrWhiteSpace(raw))
    {
        return "TaskA";
    }

    var trimmed = new string(raw.Where(char.IsLetterOrDigit).ToArray());
    return string.IsNullOrEmpty(trimmed) ? "TaskA" : trimmed;
}

static RunTime NormalizeRunTime(string? raw)
{
    if (string.IsNullOrWhiteSpace(raw))
    {
        return RunTime.CSharp;
    }

    return raw.Trim().ToLowerInvariant() switch
    {
        "c#" or "cs" or "csharp" => RunTime.CSharp,
        "js" or "javascript" or "node" => RunTime.JavaScript,
        "all" or "both" => RunTime.Both,
        _ => RunTime.Unknown,
    };
}

enum RunTime
{
    Unknown,
    CSharp,
    JavaScript,
    Both,
}
