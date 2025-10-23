function customSort(values) {
  if (!Array.isArray(values)) {
    throw new TypeError('values must be an array');
  }

  // TODO: replace with your custom sorting algorithm (no Array.prototype.sort!)
  return [...values];
}

function showcase() {
  const samples = [
    [],
    [4],
    [3, 1, 4, 1, 5, 9, 2, 6],
    [12, -4, 7, 7, -4, 0, 3, 12, 3],
    [5, 5, 5, 5, 5],
  ];

  for (const sample of samples) {
    const sorted = customSort(sample);
    console.log(`${JSON.stringify(sample)} -> ${JSON.stringify(sorted)}`);
  }
}

module.exports = {
  customSort,
  showcase,
};

if (require.main === module) {
  showcase();
}
