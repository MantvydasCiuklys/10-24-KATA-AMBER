const cases = [
  { input: [], expected: [] },
  { input: [4], expected: [4] },
  { input: [3, 1, 4, 1, 5, 9, 2, 6], expected: [1, 1, 2, 3, 4, 5, 6, 9] },
  { input: [12, -4, 7, 7, -4, 0, 3, 12, 3], expected: [-4, -4, 0, 3, 3, 7, 7, 12, 12] },
  { input: [5, 5, 5, 5, 5], expected: [5, 5, 5, 5, 5] },
  { input: [1, 2, 3, 4], expected: [1, 2, 3, 4] },
  { input: [9, 7, 5, 3, 1], expected: [1, 3, 5, 7, 9] },
  { input: [-3, -1, -2, -4], expected: [-4, -3, -2, -1] },
  { input: [0, -1, 1, 0, -1, 1], expected: [-1, -1, 0, 0, 1, 1] },
  {
    input: [2147483647, 0, -2147483648, 42, -42],
    expected: [-2147483648, -42, 0, 42, 2147483647],
  },
];

function clone(values) {
  return Array.isArray(values) ? values.slice() : values;
}

function format(values) {
  if (!Array.isArray(values)) {
    return "null";
  }

  return `[${values.join(", ")}]`;
}

function runTaskESmokeTests(solver) {
  if (typeof solver !== "function") {
    throw new TypeError("solver must be a function");
  }

  for (const { input, expected } of cases) {
    const copy = clone(input);

    try {
      const result = solver(copy);
      console.log(`${format(input)} -> ${format(result)} (expected ${format(expected)})`);
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error);
      console.log(`${format(input)} -> threw ${message}`);
    }
  }
}

module.exports = {
  runTaskESmokeTests,
};
