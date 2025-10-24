const { runTaskBSmokeTests } = require("../taskSmokeTests");

function countDivisibleSubarrays(nums, k) {

let a = 0;
let count = 0;

for (let i = 0; i < nums.length; i++) {
  //
    if (nums[i] % k === 0) {
      for (let j = i; j < nums.length; j++) {
        if (nums[j] % k !== 0) {
          break;
        }
        count++;
      }
      a+=nums[i];
    }
  }

  // your code goes here
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
