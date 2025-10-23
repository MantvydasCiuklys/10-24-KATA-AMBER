function countDivisibleSubarrays(nums, k) {
  // your code goes here
  return -1;
}

function formatCase(nums, k) {
  const values = Array.isArray(nums) ? `[${nums.join(', ')}]` : 'null';
  return `nums = ${values}, k = ${k}`;
}

function runSmokeTests() {
  const cases = [
    { nums: [4, 5, 0, -2, -3, 1], k: 5, expected: 7 },
    { nums: [5], k: 9, expected: 0 },
    { nums: [5, 10, 15], k: 5, expected: 6 },
    { nums: [1, 2, 3, 4, 5], k: 3, expected: 7 },
  ];

  for (const { nums, k, expected } of cases) {
    try {
      const result = countDivisibleSubarrays(nums, k);
      console.log(`${formatCase(nums, k)} -> ${result} (expected ${expected})`);
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error);
      console.log(`${formatCase(nums, k)} -> threw ${message}`);
    }
  }
}

module.exports = {
  countDivisibleSubarrays,
  runSmokeTests,
};

if (require.main === module) {
  runSmokeTests();
}
