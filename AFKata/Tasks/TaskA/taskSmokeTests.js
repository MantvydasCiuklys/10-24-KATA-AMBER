const cases = [
  { input: "leetcode", expected: 0 },
  { input: "loveleetcode", expected: 2 },
  { input: "aabb", expected: -1 },
  { input: "aabbccd", expected: 6 },
  { input: "z", expected: 0 },
  { input: "AaBb", expected: 0 },
  { input: "abcddcbaef", expected: 8 },
  { input: "abcabcdd", expected: -1 },
];

function formatInput(value) {
  return value === undefined ? "undefined" : JSON.stringify(value);
}

function runTaskASmokeTests(solver) {
  for (const { input, expected } of cases) {
    try {
      const result = solver(input);
      console.log(`${formatInput(input)} -> ${result} (expected ${expected})`);
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error);
      console.log(`${formatInput(input)} -> threw ${message}`);
    }
  }
}

module.exports = {
  runTaskASmokeTests,
};
