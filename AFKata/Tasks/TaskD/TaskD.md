# Task D: Invent a Cipher

For this task there are no automated tests or sample cases. Your mission is to design and implement your own text cipher.

## Requirements

- Implement a deterministic function that takes a plaintext string and returns an encoded string.
- Your cipher must be reversible in theory (i.e., a teammate could decode it if they knew your algorithm), but you do **not** need to provide the decoder implementation here.
- Input strings may contain uppercase letters, lowercase letters, digits, spaces, and common punctuation.
- Produce only printable ASCII characters in the encoded output.

## Deliverables

- Update the provided `Cipher` function in your language of choice.
- Document your approach (briefly) in code comments or a README inside your contestant folder so judges understand how to decode it.
- Include the encoded result for the sample message below in a comment so we can compare entries:

```
Meet me at the old bridge at midnight.
```

## Evaluation

- Creativity and strength of the cipher.
- Clarity of documentation for how it works.
- Consistency (running the function twice on the same input must give the same output).

Remember: there are no unit tests—run whatever manual checks you need to convince yourself your cipher works.
