function firstUniqChar(s) {
  // your code goes here
}

function runSmokeTests() {
  const cases = [
    { input: "leetcode", expected: 0 },
    { input: "aabb", expected: -1 },
  ];

  console.log(`[Tadas] running smoke tests`);
  for (const { input, expected } of cases) {
    try {
      const result = firstUniqChar(input);
      console.log(`${JSON.stringify(input)} -> ${result} (expected ${expected})`);
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error);
      console.log(`${JSON.stringify(input)} -> threw ${message}`);
    }
  }
}

module.exports = {
  firstUniqChar,
  runSmokeTests,
};

if (require.main === module) {
  runSmokeTests();
}
