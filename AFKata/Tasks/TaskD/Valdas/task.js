const SAMPLE_MESSAGE = "Meet me at the old bridge at midnight.";
const secret_key = 69;



function cipher(plaintext) {
  if (plaintext == null) {
    throw new TypeError('plaintext must not be null or undefined');
  }
  // TODO: replace with your custom cipher implementation

  let encoded = "";

for (let i = 0; i < plaintext.length; i++) {
    let x = plaintext.charCodeAt(i)+secret_key;
    let y = plaintext.charCodeAt(plaintext.length - i)*secret_key;

    //console.log(x,y);
    //let encodedChar = String.fromCharCode(x*y);
    encoded += String.fromCharCode(y);
    //encoded+=plaintext[i]+secret_key;
  }
  //console.log("encoded",encoded);

  decipher(encoded);

  return encoded;
}

function showcase() {
  const encoded = cipher(SAMPLE_MESSAGE);
  console.log(`Sample input: ${SAMPLE_MESSAGE}`);
  console.log(`Your cipher output: ${encoded}`);
  console.log(`Sample output: ${SAMPLE_MESSAGE}`);
}

function decipher(encoded) {
  // TODO: replace with your custom decoder implementation

  let decoded = "";
for (let i = 0; i < encoded.length; i++) {
    let x = encoded.charCodeAt(i)-secret_key-1;
    let y = encoded.charCodeAt(encoded.length - i)/secret_key;

    console.log(x,y);
    //let encodedChar = String.fromCharCode(x*y);
    decoded += String.fromCharCode(y);
    //encoded+=plaintext[i]+secret_key;
  }
    console.log(`Sample output: ${decoded}`);
  return decoded;
}

module.exports = {
  cipher,
  showcase,
};

if (require.main === module) {
  showcase();
}
