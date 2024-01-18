const { randomInteger, generateRandomNumbers } = require('./utils'); 
const { mapActionIdToName } = require('./utils'); 

function randomActions(data) {
    const randomNumbers = generateRandomNumbers(data.output, data.output.length);
    return randomNumbers.map(mapActionIdToName);
}

function randomAction(data) {
    const actionId = randomInteger(0, 4);
    const actionName = mapActionIdToName(actionId);
    //console.log(data.output.length, actionId, actionName);
    return [actionName];
}

module.exports = {
    randomAction,
    randomActions,
};
