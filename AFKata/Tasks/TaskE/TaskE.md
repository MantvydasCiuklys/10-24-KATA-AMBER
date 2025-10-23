# Task E: Build Your Own Sort

You are handed an array of integers (duplicates allowed) and must produce a sorted version without relying on the language’s built-in sort helpers. Treat this as a chance to showcase a sorting idea you understand inside-out.

## Requirements

- Implement a deterministic function that returns the input numbers in non-decreasing order.
- Do **not** call general-purpose sorting APIs such as `Array.Sort`, `List.Sort`, `sort`, or similar wrappers. You may, however, use supporting data structures you implement yourself (heaps, trees, etc.).
- Handle edge cases gracefully: empty arrays, single-element arrays, negative values, and heavily duplicated inputs.

## Deliverables

- Fill in the `CustomSort` implementation in your contestant folder.
- Run the function on the sample inputs below and note the outputs in a comment for quick verification.

### Sample Inputs

```
[]
[4]
[3, 1, 4, 1, 5, 9, 2, 6]
[12, -4, 7, 7, -4, 0, 3, 12, 3]
[5, 5, 5, 5, 5]
```

## Evaluation Criteria

- Correctness: every run returns a faithfully sorted array.
- Efficiency: algorithm choice and implementation details.
- Clarity: readable code and explanation of how to reverse/verify the process.

There are no automated tests—craft your own harnesses or prints to demonstrate the sort works.
