const { generateRandomNumbers } = require('./utils'); 
const { mapActionIdToName } = require('./utils'); 

function randomActions(data) {
    const randomNumbers = generateRandomNumbers(data.output, data.output.length);
    return randomNumbers.map(mapActionIdToName);
}

module.exports = {
    randomActions,
};
