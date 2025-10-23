using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;


namespace Admin;

public static class AdminTestRunner
{
    private static readonly string ProjectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    private static readonly IReadOnlyList<string> AllTasks = new[] { "TaskA", "TaskB", "TaskC", "TaskD", "TaskE" };
    private static readonly JsonSerializerOptions JsonOptions = new() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };

    public static void Run(Assembly assembly, IReadOnlyCollection<string>? requestedTasks = null)
    {
        var taskSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (requestedTasks is { Count: > 0 })
        {
            foreach (var raw in requestedTasks)
            {
                if (string.IsNullOrWhiteSpace(raw))
                {
                    continue;
                }

                var trimmed = raw.Trim();
                if (string.Equals(trimmed, "all", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var task in AllTasks)
                    {
                        taskSet.Add(task);
                    }

                    continue;
                }

                var normalized = NormalizeTask(trimmed);
                if (normalized is not null)
                {
                    taskSet.Add(normalized);
                }
            }
        }

        if (taskSet.Count == 0)
        {
            Console.WriteLine("[admin] No tasks selected. Set ContestantTask to one of TaskA|TaskB|TaskC|TaskD|TaskE or 'all'.");
            return;
        }

        var tasks = taskSet.ToArray();
        Array.Sort(tasks, StringComparer.OrdinalIgnoreCase);


        var aggregates = new Dictionary<string, ContestantAggregate>(StringComparer.OrdinalIgnoreCase);

        foreach (var task in tasks)
        {
            var summaries = RunTask(assembly, task);
            if (summaries is null)
            {
                continue;
            }

            foreach (var summary in summaries)
            {
                var allPass = summary.Results.All(r => r.Kind == ResultKind.Pass);
                var aggregate = GetOrCreateAggregate(aggregates, summary.Contestant);
                aggregate.Record(summary.Language, summary.TaskName, allPass, summary.Results.Count(r => r.Kind == ResultKind.Pass), summary.Results.Length);
            }
        }

        PrintSummary(aggregates);
    }
    private static IReadOnlyList<TaskSummary>? RunTask(Assembly assembly, string taskName) => taskName switch
    {
        "TaskA" => RunTaskATests(assembly),
        "TaskB" => RunTaskBTests(assembly),
        "TaskC" => RunTaskCTests(assembly),
        "TaskD" => RunTaskDChecks(assembly),
        "TaskE" => RunTaskETests(assembly),
        _ => null,
    };

    private static IReadOnlyList<TaskSummary> RunTaskATests(Assembly assembly)
    {
        const string taskName = "TaskA";
        var testCases = new (string input, int expected)[]
        {
            ("abcabc", -1),
            ("aaab", 3),
            ("swiss", 1),
            ("z", 0),
            ("leetcode", 0),
        };

        var summaries = new List<TaskSummary>();
        summaries.AddRange(RunPerContestantCSharp(assembly, taskName, solverType =>
        {
            var method = solverType.GetMethod("FirstUniqChar", BindingFlags.Public | BindingFlags.Static);
            if (method is null)
            {
                return new[] { Result.Missing("FirstUniqChar") };
            }

            return testCases.Select(test =>
            {
                try
                {
                    var raw = method.Invoke(null, new object?[] { test.input });
                    int? maybe = raw as int?;
                    if (raw is int intValue)
                    {
                        maybe = intValue;
                    }

                    if (!maybe.HasValue)
                    {
                        var display = raw?.ToString() ?? "<null>";
                        return Result.Fail(test.input, display, test.expected.ToString());
                    }

                    var actual = maybe.Value;
                    return actual == test.expected
                        ? Result.Pass(test.input, actual.ToString())
                        : Result.Fail(test.input, actual.ToString(), test.expected.ToString());
                }
                catch (Exception ex)
                {
                    return Result.Error(test.input, ex);
                }
            }).ToArray();
        }));

        summaries.AddRange(RunPerContestantJavaScript(taskName, RunTaskAJavaScript));
        return summaries;
    }

    private static IReadOnlyList<TaskSummary> RunTaskBTests(Assembly assembly)
    {
        const string taskName = "TaskB";
        var testCases = new (int[] nums, int k, long expected)[]
        {
            (new[] { 4, 5, 0, -2, -3, 1 }, 5, 7),
            (new[] { 0, 0, 0 }, 1, 6),
            (new[] { 5, 0, 5 }, 5, 6),
            (new[] { -1, 2, 9 }, 2, 2),
            (new[] { 7, 4, -5, -3, 1 }, 5, 4),
        };

        var summaries = new List<TaskSummary>();
        summaries.AddRange(RunPerContestantCSharp(assembly, taskName, solverType =>
        {
            var method = solverType.GetMethod("CountDivisibleSubarrays", BindingFlags.Public | BindingFlags.Static);
            if (method is null)
            {
                return new[] { Result.Missing("CountDivisibleSubarrays") };
            }

            return testCases.Select(test =>
            {
                try
                {
                    var raw = method.Invoke(null, new object?[] { test.nums, test.k });
                    long? maybe = raw switch
                    {
                        long value => value,
                        int value => value,
                        null => null,
                        IConvertible convertible => SafeConvertToInt64(convertible),
                        _ => null,
                    };

                    if (!maybe.HasValue)
                    {
                        var display = raw?.ToString() ?? "<null>";
                        return Result.Fail(FormatArrayWithK(test.nums, test.k), display, test.expected.ToString());
                    }

                    var actual = maybe.Value;
                    return actual == test.expected
                        ? Result.Pass(FormatArrayWithK(test.nums, test.k), actual.ToString())
                        : Result.Fail(FormatArrayWithK(test.nums, test.k), actual.ToString(), test.expected.ToString());
                }
                catch (Exception ex)
                {
                    return Result.Error(FormatArrayWithK(test.nums, test.k), ex);
                }
            }).ToArray();
        }));

        summaries.AddRange(RunPerContestantJavaScript(taskName, RunTaskBJavaScript));
        return summaries;
    }
    private static IReadOnlyList<TaskSummary> RunTaskCTests(Assembly assembly)
    {
        const string taskName = "TaskC";
        var testCases = new[]
        {
            "ABEDSAGHASADBABABASDED",
            "AAAAABBBBCCCCDDDD",
            "The quick brown fox jumps over the lazy dog",
        };

        var summaries = new List<TaskSummary>();
        summaries.AddRange(RunPerContestantCSharp(assembly, taskName, solverType =>
        {
            var compress = solverType.GetMethod("Compress", BindingFlags.Public | BindingFlags.Static);
            var decompress = solverType.GetMethod("Decompress", BindingFlags.Public | BindingFlags.Static);

            if (compress is null && decompress is null)
            {
                return new[] { Result.Missing("Compress/Decompress") };
            }

            if (compress is null)
            {
                return new[] { Result.Missing("Compress") };
            }

            if (decompress is null)
            {
                return new[] { Result.Missing("Decompress") };
            }

            return testCases.Select(sample =>
            {
                try
                {
                    var compressed = compress.Invoke(null, new object?[] { sample }) as string;
                    var roundtrip = decompress.Invoke(null, new object?[] { compressed }) as string;

                    if (compressed is null)
                    {
                        return Result.Fail(sample, "<null>", "non-null compressed string");
                    }

                    var matches = string.Equals(roundtrip, sample, StringComparison.Ordinal);
                    return matches
                        ? Result.Pass(sample, $"len={compressed.Length}")
                        : Result.Fail(sample, roundtrip ?? "<null>", sample);
                }
                catch (Exception ex)
                {
                    return Result.Error(sample, ex);
                }
            }).ToArray();
        }));

        summaries.AddRange(RunPerContestantJavaScript(taskName, RunTaskCJavaScript));
        return summaries;
    }

    private static IReadOnlyList<TaskSummary> RunTaskDChecks(Assembly assembly)
    {
        const string taskName = "TaskD";
        var samples = new[]
        {
            "Meet me at the old bridge at midnight.",
            "Cipher me twice and make it nice!",
        };

        var summaries = new List<TaskSummary>();
        summaries.AddRange(RunPerContestantCSharp(assembly, taskName, solverType =>
        {
            var method = solverType.GetMethod("Cipher", BindingFlags.Public | BindingFlags.Static);
            if (method is null)
            {
                return new[] { Result.Missing("Cipher") };
            }

            return samples.Select(sample =>
            {
                try
                {
                    var first = method.Invoke(null, new object?[] { sample }) as string;
                    var second = method.Invoke(null, new object?[] { sample }) as string;

                    if (first is null)
                    {
                        return Result.Fail(sample, "<null>", "non-null output");
                    }

                    if (!string.Equals(first, second, StringComparison.Ordinal))
                    {
                        return Result.Fail(sample, first, "deterministic output");
                    }

                    if (string.Equals(first, sample, StringComparison.Ordinal))
                    {
                        return Result.Fail(sample, first, "transformed output");
                    }

                    return Result.Pass(sample, first);
                }
                catch (Exception ex)
                {
                    return Result.Error(sample, ex);
                }
            }).ToArray();
        }));

        summaries.AddRange(RunPerContestantJavaScript(taskName, RunTaskDJavaScript));
        return summaries;
    }

    private static IReadOnlyList<TaskSummary> RunTaskETests(Assembly assembly)
    {
        const string taskName = "TaskE";
        var testCases = new[]
        {
            Array.Empty<int>(),
            new[] { 4 },
            new[] { 3, 1, 4, 1, 5, 9, 2, 6 },
            new[] { 12, -4, 7, 7, -4, 0, 3, 12, 3 },
            new[] { 5, 5, 5, 5, 5 },
            Enumerable.Range(-20, 41).Reverse().ToArray(),
        };

        var summaries = new List<TaskSummary>();
        summaries.AddRange(RunPerContestantCSharp(assembly, taskName, solverType =>
        {
            var method = solverType.GetMethod("CustomSort", BindingFlags.Public | BindingFlags.Static);
            if (method is null)
            {
                return new[] { Result.Missing("CustomSort") };
            }

            return testCases.Select(sample =>
            {
                try
                {
                    var copy = sample.ToArray();
                    var raw = method.Invoke(null, new object?[] { copy });
                    if (raw is not int[] result)
                    {
                        return Result.Fail(FormatArray(sample), raw?.GetType().Name ?? "<null>", "int[]");
                    }

                    var expected = sample.ToArray();
                    Array.Sort(expected);

                    var matches = result.SequenceEqual(expected);
                    var multisetOk = MultisetEquals(result, sample);
                    var sorted = IsSorted(result);

                    if (matches && multisetOk && sorted)
                    {
                        return Result.Pass(FormatArray(sample), FormatArray(result));
                    }

                    var failureReason = !multisetOk ? "multiset mismatch" : !sorted ? "not sorted" : "order mismatch";
                    return Result.Fail(FormatArray(sample), FormatArray(result), failureReason);
                }
                catch (Exception ex)
                {
                    return Result.Error(FormatArray(sample), ex);
                }
            }).ToArray();
        }));

        summaries.AddRange(RunPerContestantJavaScript(taskName, RunTaskEJavaScript));
        return summaries;
    }
    private static IReadOnlyList<TaskSummary> RunPerContestantCSharp(Assembly assembly, string taskName, Func<Type, Result[]> testRunner)
    {
        var summaries = new List<TaskSummary>();
        foreach (var contestant in GetContestants(taskName))
        {
            var solverType = assembly.GetType($"Contestants.{contestant}.{taskName}.Solver", throwOnError: false, ignoreCase: true)
                ?? assembly.GetType($"Contestants.{contestant}.Solver", throwOnError: false, ignoreCase: true);

            Result[] results;
            if (solverType is null)
            {
                results = new[] { Result.Missing("Solver") };
            }
            else
            {
                results = testRunner(solverType);
            }

            summaries.Add(new TaskSummary(taskName, contestant, "C#", results));
        }

        return summaries;
    }

    private static IReadOnlyList<TaskSummary> RunPerContestantJavaScript(string taskName, Func<string, string, Result[]?> testRunner)
    {
        var summaries = new List<TaskSummary>();
        foreach (var contestant in GetContestants(taskName))
        {
            var folder = Path.Combine(ProjectRoot, "Tasks", taskName, contestant);
            var results = testRunner(contestant, folder);
            if (results is null)
            {
                continue;
            }

            summaries.Add(new TaskSummary(taskName, contestant, "JS", results));
        }

        return summaries;
    }
    private static Result[]? RunTaskAJavaScript(string contestant, string folder)
    {
        var modulePath = Path.Combine(folder, "task.js");
        if (!File.Exists(modulePath))
        {
            return null;
        }

        var tests = new[]
        {
            new { input = "abcabc", expected = -1 },
            new { input = "aaab", expected = 3 },
            new { input = "swiss", expected = 1 },
            new { input = "z", expected = 0 },
            new { input = "leetcode", expected = 0 },
        };

        var script = $@"const modulePath = {JsonSerializer.Serialize(modulePath, JsonOptions)};
let mod;
try {{
    mod = require(modulePath);
}} catch (error) {{
    console.log(JSON.stringify({{ error: 'require', message: error.message }}));
    process.exit(0);
}}
const fn = mod.firstUniqChar;
if (typeof fn !== 'function') {{
    console.log(JSON.stringify({{ error: 'missing', message: 'firstUniqChar not exported' }}));
    process.exit(0);
}}
const tests = {JsonSerializer.Serialize(tests, JsonOptions)};
const results = tests.map((t) => {{
    try {{
        const actual = fn(t.input);
        const value = Number(actual);
        if (!Number.isFinite(value)) {{
            return {{ kind: 'fail', input: JSON.stringify(t.input), actual: String(actual), expected: String(t.expected) }};
        }}
        if (value === t.expected) {{
            return {{ kind: 'pass', input: JSON.stringify(t.input), message: String(value) }};
        }}
        return {{ kind: 'fail', input: JSON.stringify(t.input), actual: String(value), expected: String(t.expected) }};
    }} catch (error) {{
        return {{ kind: 'error', input: JSON.stringify(t.input), message: error.message }};
    }}
}});
console.log(JSON.stringify({{ results }}));
";

        return RunJavaScriptHarness(folder, script);
    }
    private static Result[]? RunTaskBJavaScript(string contestant, string folder)
    {
        var modulePath = Path.Combine(folder, "task.js");
        if (!File.Exists(modulePath))
        {
            return null;
        }

        var tests = new[]
        {
            new { nums = new[] { 4, 5, 0, -2, -3, 1 }, k = 5, expected = 7 },
            new { nums = new[] { 0, 0, 0 }, k = 1, expected = 6 },
            new { nums = new[] { 5, 0, 5 }, k = 5, expected = 6 },
            new { nums = new[] { -1, 2, 9 }, k = 2, expected = 2 },
            new { nums = new[] { 7, 4, -5, -3, 1 }, k = 5, expected = 4 },
        };

        var script = $@"const modulePath = {JsonSerializer.Serialize(modulePath, JsonOptions)};
let mod;
try {{
    mod = require(modulePath);
}} catch (error) {{
    console.log(JSON.stringify({{ error: 'require', message: error.message }}));
    process.exit(0);
}}
const fn = mod.countDivisibleSubarrays;
if (typeof fn !== 'function') {{
    console.log(JSON.stringify({{ error: 'missing', message: 'countDivisibleSubarrays not exported' }}));
    process.exit(0);
}}
const tests = {JsonSerializer.Serialize(tests, JsonOptions)};
const results = tests.map((t) => {{
    const label = `[${{t.nums.join(', ')}}], k = ${{t.k}}`;
    try {{
        const actual = fn([...t.nums], t.k);
        const value = Number(actual);
        if (!Number.isFinite(value)) {{
            return {{ kind: 'fail', input: label, actual: String(actual), expected: String(t.expected) }};
        }}
        if (value === t.expected) {{
            return {{ kind: 'pass', input: label, message: String(value) }};
        }}
        return {{ kind: 'fail', input: label, actual: String(value), expected: String(t.expected) }};
    }} catch (error) {{
        return {{ kind: 'error', input: label, message: error.message }};
    }}
}});
console.log(JSON.stringify({{ results }}));
";

        return RunJavaScriptHarness(folder, script);
    }
    private static Result[]? RunTaskCJavaScript(string contestant, string folder)
    {
        var modulePath = Path.Combine(folder, "task.js");
        if (!File.Exists(modulePath))
        {
            return null;
        }

        var tests = new[]
        {
            "ABEDSAGHASADBABABASDED",
            "AAAAABBBBCCCCDDDD",
            "The quick brown fox jumps over the lazy dog",
        };

        var script = $@"const modulePath = {JsonSerializer.Serialize(modulePath, JsonOptions)};
let mod;
try {{
    mod = require(modulePath);
}} catch (error) {{
    console.log(JSON.stringify({{ error: 'require', message: error.message }}));
    process.exit(0);
}}
const compress = mod.compress;
const decompress = mod.decompress;
if (typeof compress !== 'function' || typeof decompress !== 'function') {{
    console.log(JSON.stringify({{ error: 'missing', message: 'compress/decompress not exported' }}));
    process.exit(0);
}}
const tests = {JsonSerializer.Serialize(tests, JsonOptions)};
const results = tests.map((sample) => {{
    try {{
        const encoded = compress(sample);
        const decoded = decompress(encoded);
        if (typeof encoded !== 'string') {{
            return {{ kind: 'fail', input: sample, actual: typeof encoded, expected: 'string' }};
        }}
        if (decoded === sample) {{
            return {{ kind: 'pass', input: sample, message: `len=${{encoded.length}}` }};
        }}
        return {{ kind: 'fail', input: sample, actual: decoded ?? '<null>', expected: sample }};
    }} catch (error) {{
        return {{ kind: 'error', input: sample, message: error.message }};
    }}
}});
console.log(JSON.stringify({{ results }}));
";

        return RunJavaScriptHarness(folder, script);
    }
    private static Result[]? RunTaskDJavaScript(string contestant, string folder)
    {
        var modulePath = Path.Combine(folder, "task.js");
        if (!File.Exists(modulePath))
        {
            return null;
        }

        var samples = new[]
        {
            "Meet me at the old bridge at midnight.",
            "Cipher me twice and make it nice!",
        };

        var script = $@"const modulePath = {JsonSerializer.Serialize(modulePath, JsonOptions)};
let mod;
try {{
    mod = require(modulePath);
}} catch (error) {{
    console.log(JSON.stringify({{ error: 'require', message: error.message }}));
    process.exit(0);
}}
const cipher = mod.cipher;
if (typeof cipher !== 'function') {{
    console.log(JSON.stringify({{ error: 'missing', message: 'cipher not exported' }}));
    process.exit(0);
}}
const tests = {JsonSerializer.Serialize(samples, JsonOptions)};
const results = tests.map((sample) => {{
    try {{
        const first = cipher(sample);
        const second = cipher(sample);
        if (first == null) {{
            return {{ kind: 'fail', input: sample, actual: '<null>', expected: 'cipher text' }};
        }}
        if (first !== second) {{
            return {{ kind: 'fail', input: sample, actual: first, expected: 'deterministic output' }};
        }}
        if (first === sample) {{
            return {{ kind: 'fail', input: sample, actual: first, expected: 'transformed output' }};
        }}
        return {{ kind: 'pass', input: sample, message: first }};
    }} catch (error) {{
        return {{ kind: 'error', input: sample, message: error.message }};
    }}
}});
console.log(JSON.stringify({{ results }}));
";

        return RunJavaScriptHarness(folder, script);
    }
    private static Result[]? RunTaskEJavaScript(string contestant, string folder)
    {
        var modulePath = Path.Combine(folder, "task.js");
        if (!File.Exists(modulePath))
        {
            return null;
        }

        var tests = new[]
        {
            Array.Empty<int>(),
            new[] { 4 },
            new[] { 3, 1, 4, 1, 5, 9, 2, 6 },
            new[] { 12, -4, 7, 7, -4, 0, 3, 12, 3 },
            new[] { 5, 5, 5, 5, 5 },
            Enumerable.Range(-20, 41).Reverse().ToArray(),
        };

        var script = $@"const modulePath = {JsonSerializer.Serialize(modulePath, JsonOptions)};
let mod;
try {{
    mod = require(modulePath);
}} catch (error) {{
    console.log(JSON.stringify({{ error: 'require', message: error.message }}));
    process.exit(0);
}}
const customSort = mod.customSort;
if (typeof customSort !== 'function') {{
    console.log(JSON.stringify({{ error: 'missing', message: 'customSort not exported' }}));
    process.exit(0);
}}
const tests = {JsonSerializer.Serialize(tests, JsonOptions)};
const results = tests.map((source) => {{
    const label = `[${{source.join(', ')}}]`;
    try {{
        const copy = [...source];
        const result = customSort(copy);
        if (!Array.isArray(result)) {{
            return {{ kind: 'fail', input: label, actual: typeof result, expected: 'array' }};
        }}
        const expected = [...source].sort((a, b) => a - b);
        if (result.length !== expected.length) {{
            return {{ kind: 'fail', input: label, actual: JSON.stringify(result), expected: JSON.stringify(expected) }};
        }}
        const mismatch = result.findIndex((value, index) => value !== expected[index]);
        if (mismatch === -1) {{
            return {{ kind: 'pass', input: label, message: JSON.stringify(result) }};
        }}
        return {{ kind: 'fail', input: label, actual: JSON.stringify(result), expected: JSON.stringify(expected) }};
    }} catch (error) {{
        return {{ kind: 'error', input: label, message: error.message }};
    }}
}});
console.log(JSON.stringify({{ results }}));
";

        return RunJavaScriptHarness(folder, script);
    }
    private static Result[] RunJavaScriptHarness(string workingDirectory, string scriptContents)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"admin_js_{Guid.NewGuid():N}.js");
        try
        {
            File.WriteAllText(tempFile, scriptContents);

            var psi = new ProcessStartInfo("node")
            {
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            psi.ArgumentList.Add(tempFile);

            using var process = Process.Start(psi);
            if (process is null)
            {
                return new[] { Result.Error("node", new InvalidOperationException("Failed to start node process.")) };
            }

            var stdout = process.StandardOutput.ReadToEnd();
            var stderr = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                var message = string.IsNullOrWhiteSpace(stderr)
                    ? $"node exited with code {process.ExitCode}"
                    : stderr.Trim();
                return new[] { Result.Error("node", new InvalidOperationException(message)) };
            }

            if (string.IsNullOrWhiteSpace(stdout))
            {
                return new[] { Result.Error("node", new InvalidOperationException("No output from JS harness.")) };
            }

            using var doc = JsonDocument.Parse(stdout);
            var root = doc.RootElement;
            if (root.TryGetProperty("error", out var errorProperty))
            {
                var message = root.TryGetProperty("message", out var msg) ? msg.GetString() : errorProperty.GetString();
                return new[] { Result.Error("js", new InvalidOperationException(message ?? "Unknown JS error")) };
            }

            if (!root.TryGetProperty("results", out var resultsElement))
            {
                return new[] { Result.Error("js", new InvalidOperationException("JS harness did not return results.")) };
            }

            var results = new List<Result>();
            foreach (var entry in resultsElement.EnumerateArray())
            {
                var kind = entry.GetProperty("kind").GetString();
                var input = entry.GetProperty("input").GetString() ?? "<input>";
                switch (kind)
                {
                    case "pass":
                        results.Add(Result.Pass(input, entry.GetProperty("message").GetString() ?? string.Empty));
                        break;
                    case "fail":
                        results.Add(Result.Fail(
                            input,
                            entry.GetProperty("actual").GetString() ?? "<null>",
                            entry.GetProperty("expected").GetString() ?? "<expected>"));
                        break;
                    case "error":
                        results.Add(Result.Error(input, new InvalidOperationException(entry.GetProperty("message").GetString() ?? "error")));
                        break;
                    default:
                        results.Add(Result.Error(input, new InvalidOperationException($"Unknown JS result kind '{kind}'")));
                        break;
                }
            }

            return results.ToArray();
        }
        catch (Exception ex)
        {
            return new[] { Result.Error("node", ex) };
        }
        finally
        {
            try
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
            catch
            {
            }
        }
    }
    private static IReadOnlyList<string> GetContestants(string taskName)
    {
        var taskFolder = Path.Combine(ProjectRoot, "Tasks", taskName);
        if (!Directory.Exists(taskFolder))
        {
            return Array.Empty<string>();
        }

        var directories = Directory.GetDirectories(taskFolder);
        if (directories.Length == 0)
        {
            return Array.Empty<string>();
        }

        var names = new List<string>(directories.Length);
        foreach (var path in directories)
        {
            var name = Path.GetFileName(path);
            if (!string.IsNullOrEmpty(name))
            {
                names.Add(name);
            }
        }

        names.Sort(StringComparer.OrdinalIgnoreCase);
        return names;
    }

    private static string? NormalizeTask(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            return null;
        }

        var cleaned = new string(raw.Where(char.IsLetterOrDigit).ToArray());
        if (string.IsNullOrEmpty(cleaned))
        {
            return null;
        }

        foreach (var task in AllTasks)
        {
            if (string.Equals(cleaned, task, StringComparison.OrdinalIgnoreCase))
            {
                return task;
            }
        }

        return null;
    }


    private static void PrintSummary(Dictionary<string, ContestantAggregate> aggregates)
    {
        if (aggregates.Count == 0)
        {
            return;
        }

        foreach (var (contestant, aggregate) in aggregates.OrderBy(kvp => kvp.Key, StringComparer.OrdinalIgnoreCase))
        {
            var snapshot = aggregate.GetBestSnapshot();
            var taskRatio = snapshot.TotalTasks == 0 ? 0d : snapshot.TaskRatio;
            var testRatio = snapshot.TotalTests == 0 ? 0d : snapshot.TestRatio;
            var status = snapshot.TotalTasks > 0 && snapshot.PassedTasks == snapshot.TotalTasks ? "PASS" : "FAIL";

            Console.Write($"- {contestant}: {status} [");
            WriteColored($"{taskRatio * 100:0.#}% of tasks", GetColor(taskRatio));
            Console.Write("] ");
            WriteColored($"{snapshot.PassedTests}/{snapshot.TotalTests} tests", GetColor(testRatio));
            Console.WriteLine();
        }
    }

    private static ContestantAggregate GetOrCreateAggregate(Dictionary<string, ContestantAggregate> aggregates, string contestant)
    {
        if (!aggregates.TryGetValue(contestant, out var aggregate))
        {
            aggregate = new ContestantAggregate();
            aggregates[contestant] = aggregate;
        }

        return aggregate;
    }

    private static ConsoleColor GetColor(double ratio)
        => ratio >= 0.999 ? ConsoleColor.Green : ratio >= 0.5 ? ConsoleColor.Yellow : ConsoleColor.Red;

    private static void WriteColored(string text, ConsoleColor color)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = previous;
    }

    private static long? SafeConvertToInt64(IConvertible convertible)
    {
        try
        {
            return convertible.ToInt64(null);
        }
        catch
        {
            return null;
        }
    }

    private static string FormatArray(int[] values) => $"[{string.Join(", ", values)}]";
    private static string FormatArrayWithK(int[] values, int k) => $"[{string.Join(", ", values)}], k = {k}";

    private static bool MultisetEquals(int[] left, int[] right)
    {
        if (left.Length != right.Length)
        {
            return false;
        }

        var counts = new Dictionary<int, int>();
        foreach (var value in right)
        {
            counts.TryGetValue(value, out var current);
            counts[value] = current + 1;
        }

        foreach (var value in left)
        {
            if (!counts.TryGetValue(value, out var current) || current == 0)
            {
                return false;
            }

            counts[value] = current - 1;
        }

        return counts.Values.All(v => v == 0);
    }

    private static bool MultisetEquals(int[] sortedResult, IEnumerable<int> source)
        => MultisetEquals(sortedResult, source.ToArray());

    private static bool IsSorted(IReadOnlyList<int> values)
    {
        for (var i = 1; i < values.Count; i++)
        {
            if (values[i] < values[i - 1])
            {
                return false;
            }
        }

        return true;
    }
    private sealed record TaskSummary(string TaskName, string Contestant, string Language, Result[] Results);

    private sealed class ContestantAggregate
    {
        private readonly Dictionary<string, LanguageAggregate> _languages = new(StringComparer.OrdinalIgnoreCase);

        public void Record(string language, string taskName, bool taskPassed, int passedTests, int totalTests)
        {
            if (!_languages.TryGetValue(language, out var aggregate))
            {
                aggregate = new LanguageAggregate();
                _languages[language] = aggregate;
            }

            aggregate.Record(taskName, taskPassed, passedTests, totalTests);
        }

        public LanguageSnapshot GetBestSnapshot()
        {
            LanguageSnapshot? best = null;

            foreach (var (language, aggregate) in _languages)
            {
                var snapshot = aggregate.ToSnapshot(language);
                if (best is null || snapshot.CompareTo(best.Value) > 0)
                {
                    best = snapshot;
                }
            }

            return best ?? LanguageSnapshot.Empty;
        }

        private sealed class LanguageAggregate
        {
            private readonly Dictionary<string, TaskAccumulator> _tasks = new(StringComparer.OrdinalIgnoreCase);
            private int _passedTests;
            private int _totalTests;

            public void Record(string taskName, bool passed, int passedTests, int totalTests)
            {
                if (!_tasks.TryGetValue(taskName, out var accumulator))
                {
                    accumulator = new TaskAccumulator();
                    _tasks[taskName] = accumulator;
                }

                accumulator.Update(passed);
                _passedTests += passedTests;
                _totalTests += totalTests;
            }

            public LanguageSnapshot ToSnapshot(string language)
            {
                var passedTasks = _tasks.Values.Count(t => t.IsSuccessful);
                return new LanguageSnapshot(language, passedTasks, _tasks.Count, _passedTests, _totalTests);
            }

            private sealed class TaskAccumulator
            {
                private bool _hasValue;
                private bool _passed = true;

                public void Update(bool passed)
                {
                    if (!_hasValue)
                    {
                        _passed = passed;
                        _hasValue = true;
                    }
                    else
                    {
                        _passed &= passed;
                    }
                }

                public bool IsSuccessful => _hasValue && _passed;
            }
        }

        public readonly struct LanguageSnapshot
        {
            public static readonly LanguageSnapshot Empty = new(string.Empty, 0, 0, 0, 0);

            public LanguageSnapshot(string language, int passedTasks, int totalTasks, int passedTests, int totalTests)
            {
                Language = language;
                PassedTasks = passedTasks;
                TotalTasks = totalTasks;
                PassedTests = passedTests;
                TotalTests = totalTests;
            }

            public string Language { get; }
            public int PassedTasks { get; }
            public int TotalTasks { get; }
            public int PassedTests { get; }
            public int TotalTests { get; }

            public double TaskRatio => TotalTasks == 0 ? 0 : (double)PassedTasks / TotalTasks;
            public double TestRatio => TotalTests == 0 ? 0 : (double)PassedTests / TotalTests;

            public int CompareTo(LanguageSnapshot other)
            {
                var taskComparison = TaskRatio.CompareTo(other.TaskRatio);
                if (taskComparison != 0)
                {
                    return taskComparison;
                }

                var testComparison = TestRatio.CompareTo(other.TestRatio);
                if (testComparison != 0)
                {
                    return testComparison;
                }

                return string.Compare(other.Language, Language, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    private readonly record struct Result(ResultKind Kind, string Input, string Message)
    {
        public static Result Pass(string input, string message) => new(ResultKind.Pass, input, message);
        public static Result Fail(string input, string actual, string expected) => new(ResultKind.Fail, input, $"actual={actual}, expected={expected}");
        public static Result Missing(string member) => new(ResultKind.Fail, member, "missing member");
        public static Result Error(string input, Exception ex) => new(ResultKind.Error, input, $"threw {ex.GetType().Name}: {ex.Message}");

        public override string ToString() => Kind switch
        {
            ResultKind.Pass => $"PASS :: {Input} :: {Message}",
            ResultKind.Fail => $"FAIL :: {Input} :: {Message}",
            ResultKind.Error => $"ERROR :: {Input} :: {Message}",
            _ => Message,
        };
    }

    private enum ResultKind
    {
        Pass,
        Fail,
        Error,
    }
}
