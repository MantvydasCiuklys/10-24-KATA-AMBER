const SAMPLE_MESSAGE = "Meet me at the old bridge at midnight.";

function cipher(plaintext) {
  if (plaintext == null) {
    throw new TypeError('plaintext must not be null or undefined');
  }
  

  // TODO: replace with your custom cipher implementation
  return plaintext;
}

function showcase() {
  const encoded = cipher(SAMPLE_MESSAGE);
  console.log(`Sample input: ${SAMPLE_MESSAGE}`);
  console.log(`Your cipher output: ${encoded}`);
}

module.exports = {
  cipher,
  showcase,
};

if (require.main === module) {
  showcase();
}
