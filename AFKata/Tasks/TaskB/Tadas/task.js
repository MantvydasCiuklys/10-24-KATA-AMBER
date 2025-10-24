const { runTaskBSmokeTests } = require("../taskSmokeTests");

function countDivisibleSubarrays(nums, k) {
  if (nums.length === 0) return 0;

  if (nums.length === 1) {
    return nums[0] % k === 0 ? 1 : 0;
  }

  let count = 0;
  for (let i = 0; i < nums.length; i++) {
    let sum = 0;

    for (let j = i; j < nums.length; j++) {
      let sumBy2 = nums[i] + nums[j];
      if (sumBy2 % k === 0) {
        count++;
      }
    }
  }

  return count;
}

function runSmokeTests() {
  runTaskBSmokeTests(countDivisibleSubarrays);
}

module.exports = {
  countDivisibleSubarrays,
  runSmokeTests,
};

if (require.main === module) {
  runSmokeTests();
}
