// Action name/id mapping
function mapActionIdToName(number) {
    switch (number) {
        case 0: return "Move Stay";
        case 1: return "Move Down";
        case 2: return "Move Up";
        case 3: return "Move Left";
        case 4: return "Move Right";
        default: return "Move Unknown";
    }
}

function mapActionNameToId(name) {
    switch (name) {
        case "Move Stay": return 0;
        case "Move Down": return 1;
        case "Move Up": return 2;
        case "Move Left": return 3;
        case "Move Right": return 4;
        default: return -1;
    }
}

// Get/Set parameters
const fs = require('fs');
const path = require('path');

function getParameters(req, res) {
    const filePath = path.join(__dirname, 'Parameters.json');

    fs.readFile(filePath, 'utf8', (err, data) => {
        if (err) {
            console.error("Error reading JSON file:", err);
            res.status(500).send("Internal Server Error");
            return;
        }

        const jsonData = JSON.parse(data);
        res.json(jsonData);
    });
}

function setParameters(req, res) {
    const filePath = path.join(__dirname, 'Parameters.json');
    const updatedJson = JSON.stringify(req.body, null, 2);

    fs.writeFile(filePath, updatedJson, 'utf8', (err) => {
        if (err) {
            console.error("Error writing to JSON file:", err);
            res.status(500).send("Internal Server Error");
            return;
        }

        res.send("Parameters updated successfully");
    });
}

// Generate random numbers
var generateRandomNumbers = function(actions, index)
{
    // Ensure that length is a non-negative integer
    if (typeof index !== 'number' || index < 0) {
        throw new Error('Invalid index ' + index);
    }

    // Base case: when length is 0, return an empty array
    if (index === 0) {
        return [];
    }
    // Recursive case: generate a random number and concatenate it with the rest of the array
    else {
        const actionName = actions[actions.length - index];
        const actionId = mapActionNameToId(actionName);
        const randomNumber = randomInteger(0, actionId);
        //console.log('randomNumber:', randomNumber, 'action:', actionId);
        return [randomNumber].concat(generateRandomNumbers(actions, index - 1));
    }
};

function randomInteger(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

// Exports
module.exports = {
    mapActionIdToName,
    mapActionNameToId,
    getParameters,
    setParameters,
    generateRandomNumbers
};

