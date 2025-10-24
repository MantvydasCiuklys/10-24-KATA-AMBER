const { runTaskASmokeTests } = require("../taskSmokeTests");

function firstUniqChar(s) {
  for (let i = 0; i < s.length; i++) {
    if (s.indexOf(s[i]) === i) {
      return i;
    }
  }
  return 0;
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
