const { runTaskBSmokeTests } = require("../taskSmokeTests");

function countDivisibleSubarrays(nums, k) {
  // your code goes here
  return -1;
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
