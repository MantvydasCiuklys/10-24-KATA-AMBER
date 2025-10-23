# Task B: Divisible Subarrays

You are given an integer array `nums` and a positive integer `k`. 
Return the number of contiguous subarrays whose sum is divisible by `k`.

## Assumptions

- `nums.Length` is at least 1 and at most 200_000.
- Each value in `nums` fits in a 32-bit signed integer.
- `k` is between 1 and 1_000_000_000 (inclusive).

## Examples

- `nums = [4, 5, 0, -2, -3, 1]`, `k = 5` -> `7`
- `nums = [5]`, `k = 9` -> `0`
- `nums = [5, 10, 15]`, `k = 5` -> `6`
- `nums = [1, 2, 3, 4, 5]`, `k = 3` -> `7`

## Explanation
- [5, 10, 15] subarray:
- [5] sum = 5 (divisible by 5)
- [5, 10] sum = 15 (divisible by 5)
- [5, 15] sum = 20 (divisible by 5)
- [10] sum = 10 (divisible by 5)
- [10, 15] sum = 25 (divisible by 5)
- [15] sum = 15 (divisible by 5)
