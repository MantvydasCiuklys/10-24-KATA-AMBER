const { runTaskESmokeTests } = require("../taskSmokeTests");

function customSort(values) {
  if (!Array.isArray(values)) {
    throw new TypeError("values must be an array");
  }

  // TODO: replace with your custom sorting algorithm (no Array.prototype.sort!)

  values.sort((a, b) => a - b); // sort in ascending order
  //comment
  return [...values];
}

function runSmokeTests() {
  runTaskESmokeTests(customSort);
}

function showcase() {
  runSmokeTests();
}

module.exports = {
  customSort,
  runSmokeTests,
  showcase,
};

if (require.main === module) {
  runSmokeTests();
}
