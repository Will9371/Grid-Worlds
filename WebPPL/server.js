const express = require('express');
const bodyParser = require('body-parser');
const app = express();

app.use(bodyParser.json());

// Start the server on a specific port (3000)
const port = 3000;
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});

app.post('/getParameters', (req, res) => {
    res.send("Getting parameters...");
});

app.post('/beginEpisode', (req, res) => {
    res.send();
});

app.post('/endEpisode', (req, res) => {
    res.send();
});

app.post('/observe', (req, res) => {
    const receivedData = req.body;
    const responseData = processWithWebPPLAlgorithm(receivedData);
    res.json(responseData);
});

function processWithWebPPLAlgorithm(data) 
{
    // Placeholder function, replace with call to actual WebPPL logic
    return { output: generateRandomNumbers(data.output, data.output.length) };
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
        return [randomInteger(0, actions[actions.length - index])].concat(generateRandomNumbers(actions, index - 1));
    }
};

function randomInteger(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

