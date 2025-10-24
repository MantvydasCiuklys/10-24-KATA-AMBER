function compress(payload) {
  if (payload == null) {
    throw new TypeError("payload must not be null or undefined");
  }
  // Baseline implementation performs no compression

  
  let compressed = "";
  let X = [[],[]];
  let count = 1;

  for (let i = 0; i < payload.length; i++) {
    //console.log(payload[i]);
    //compressed += payload[i];
    if(payload[i]!==payload[i+1]){
      
      compressed+=payload[i];
      //if(count>1){
        compressed+=count;
    //}
      //compressed+=",";
      count=1;
      //i++;
    }
    else{ 
      count++;
      //compressed+=payload[i];
      }

  }

  console.log("compressed",compressed);
  return compressed;
}

function decompress(encoded) {
  if (encoded == null) {
    throw new TypeError("encoded must not be null or undefined");
  }

  let decompressed = "";
  for (let i = 0; i < encoded.length; i++) {
    for (let j = 0; j < encoded[i+1]; j++) {
      decompressed+=encoded[i];
    }

    
  }
  // Baseline implementation assumes identity encoding
  return decompressed;
}

function runSmokeTests() {
  const cases = [
    "",
    "AAAAABBBBCCCCDDDD",
    "XYZXYZXYZXYZ",
    "ABEDSAGHASADBABABASDED",
    "1233334455AABBCDE"
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
