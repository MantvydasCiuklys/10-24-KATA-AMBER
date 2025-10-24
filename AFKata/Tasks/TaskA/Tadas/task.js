const { runTaskASmokeTests } = require("../taskSmokeTests");

function firstUniqChar(s) {
  // your code goes here
  if (s.length === 1) return 0;

  let foundedChards = [];
  for (let i = 0; i < s.length; i++) {
    let foundDublicate = false;
    let unique = s[i];

    for (let j = i + 1; j < s.length; j++) {
      if (s[j] == unique) {
        foundDublicate = true;
      }
    }

    if (!foundDublicate && foundedChards.indexOf(unique) === -1) {
      return i;
    }

    foundedChards.push(unique);
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
