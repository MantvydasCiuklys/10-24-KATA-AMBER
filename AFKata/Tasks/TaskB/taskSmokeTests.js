const cases = [
  { nums: [4, 5, 0, -2, -3, 1], k: 5, expected: 7 },
  { nums: [5], k: 9, expected: 0 },
  { nums: [5, 10, 15], k: 5, expected: 6 },
  { nums: [1, 2, 3, 4, 5], k: 3, expected: 7 },
  { nums: [0, 0, 0], k: 5, expected: 6 },
  { nums: [2, 4, 6], k: 1, expected: 6 },
  { nums: [6], k: 3, expected: 1 },
  { nums: [7, -2, 3], k: 4, expected: 1 },
  { nums: [1, 2, 3], k: 5, expected: 1 },
  { nums: [5, -5, 5, 5], k: 5, expected: 10 },
];

function formatCase(nums, k) {
  if (!Array.isArray(nums)) {
    return `nums = null, k = ${k}`;
  }

  return `nums = [${nums.join(", ")}], k = ${k}`;
}

function runTaskBSmokeTests(solver) {
  for (const { nums, k, expected } of cases) {
    try {
      const result = solver(nums, k);
      console.log(`${formatCase(nums, k)} -> ${result} (expected ${expected})`);
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error);
      console.log(`${formatCase(nums, k)} -> threw ${message}`);
    }
  }
}

module.exports = {
  runTaskBSmokeTests,
};
