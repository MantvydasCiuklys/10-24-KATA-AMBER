const { runTaskASmokeTests } = require("../taskSmokeTests");

function firstUniqChar(s) {
  // your code goes here
  if (s.length === 1) return 0;

  for (let i = 0; i < s.length; i++) {
    const unique = s[i];

    for (let j = i + 1; j < s.length; j++) {
      if (s[j] == unique) {
        return i;
      }
    }
  }

  return -1;
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
