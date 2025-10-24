const { runTaskASmokeTests } = require("../taskSmokeTests");

function firstUniqChar(s) {
  // your code goes here
}

function runSmokeTests() {
  runTaskASmokeTests(firstUniqChar);
}

module.exports = {
  firstUniqChar,
  runSmokeTests,
};

if (require.main === module) {
  runSmokeTests();
}
