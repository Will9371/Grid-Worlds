const express = require('express');
const bodyParser = require('body-parser');
const app = express();

app.use(bodyParser.json());

// Start the server on a specific port (3000)
const port = 3000;
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});

const fs = require('fs');
const path = require('path');

app.post('/getParameters', (req, res) => {
    // Assuming the JSON file is named 'Parameters.json' in the same directory
    const filePath = path.join(__dirname, 'Parameters.json');

    // Read the contents of the JSON file synchronously
    fs.readFile(filePath, 'utf8', (err, data) => {
        if (err) {
            console.error("Error reading JSON file:", err);
            res.status(500).send("Internal Server Error");
            return;
        }

        // Parse the JSON data
        const jsonData = JSON.parse(data);

        // Send the JSON data as the response
        res.json(jsonData);
    });
});

app.post('/setParameters', (req, res) => {
    // Assuming the JSON file is named 'Parameters.json' in the same directory
    const filePath = path.join(__dirname, 'Parameters.json');

    // Convert the new data to JSON format
    const updatedJson = JSON.stringify(req.body, null, 2);

    // Write the updated JSON data to the file, overwriting its contents
    fs.writeFile(filePath, updatedJson, 'utf8', (err) => {
        if (err) {
            console.error("Error writing to JSON file:", err);
            res.status(500).send("Internal Server Error");
            return;
        }

        res.send("Parameters updated successfully");
    });
});

app.post('/beginEpisode', (req, res) => {
    res.send();
});

app.post('/endEpisode', (req, res) => {
    res.send();
});

app.post('/agentEvent', (req, res) => {
    console.log(req.body);
    res.send();
});

app.post('/observe', (req, res) => {
    const receivedData = req.body;
    const responseData = processWithWebPPLAlgorithm(receivedData);
    res.json(responseData);
});

/// Placeholder function, replace with call to actual WebPPL logic
function processWithWebPPLAlgorithm(data) {

    var randomNumbers = generateRandomNumbers(data.output, data.output.length);
    var mappedStrings = randomNumbers.map(mapActionIdToName);
    return { output: mappedStrings };
}

function mapActionIdToName(number) {
    //console.log(number);
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
    //console.log(name);
    switch (name) {
        case "Move Stay": return 0;
        case "Move Down": return 1;
        case "Move Up": return 2;
        case "Move Left": return 3;
        case "Move Right": return 4;
        default: return -1;
    }
}

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

