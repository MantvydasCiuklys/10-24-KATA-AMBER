# Task C: Custom String Compression

You are given an arbitrary ASCII string `payload`. Implement two functions:

- `string Compress(string payload)`
- `string Decompress(string encoded)`

The encoded form must be lossless: `Decompress(Compress(payload))` must always return the original `payload`.

## Rules

- The input string can contain any visible ASCII characters (including spaces and punctuation) and may be up to 10,000 characters long.
- Your encoding may use any alphabet you like (plain text, binary, base64, etc.) as long as the result is printable ASCII.
- `Compress` must handle empty strings gracefully.
- `Decompress` must validate its input and throw a clear exception (or return a sentinel) if the data is corrupt.

## Guidance

The starter kit includes a fixed sample string so you can test round-tripping:

```
ABEDSAGHASADBABABASDED
```

Suggested additional test cases:

1. `payload = ""`
2. `payload = "AAAAABBBBCCCCDDDD"`
3. `payload = "XYZXYZXYZXYZ"`

Feel free to add more test cases covering long runs, alternating characters, and edge symbols.

## Smoke Tests

The scaffold already checks that `Decompress(Compress(payload)) == payload` for the cases above.

## Winning Note

Whoever produces the smallest encoded output for the provided sample string wins this task. Document your encoded length so we can compare solutions.
