function compress(payload) {
  if (payload == null) {
    throw new TypeError("payload must not be null or undefined");
  }

  // Baseline implementation performs no compression
  return payload;
}

function decompress(encoded) {
  if (encoded == null) {
    throw new TypeError("encoded must not be null or undefined");
  }

  // Baseline implementation assumes identity encoding
  return encoded;
}

function runSmokeTests() {
  const cases = [
    "",
    "AAAAABBBBCCCCDDDD",
    "XYZXYZXYZXYZ",
    "ABEDSAGHASADBABABASDED",
  ];

  for (const payload of cases) {
    try {
      const compressed = compress(payload);
      const roundtrip = decompress(compressed);
      console.log(
        `${format(payload)} -> compressed ${format(
          compressed
        )} -> roundtrip ${format(roundtrip)}`
      );
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error);
      console.log(`${format(payload)} -> threw ${message}`);
    }
  }
}

function format(value) {
  return value == null ? "null" : JSON.stringify(value);
}

module.exports = {
  compress,
  decompress,
  runSmokeTests,
};

if (require.main === module) {
  runSmokeTests();
}
