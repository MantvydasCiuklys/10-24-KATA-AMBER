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
