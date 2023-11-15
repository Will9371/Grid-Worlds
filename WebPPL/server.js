const express = require('express');
const bodyParser = require('body-parser');
const app = express();

app.use(bodyParser.json());

// Define a route to handle POST requests
// Handle the incoming data from Unity here
app.post('/sendData', (req, res) => {
    const receivedData = req.body;
    const responseData = processWithWebPPLAlgorithm(receivedData);
    res.json(responseData);
});

// Define a route for the root path ("/")
app.get('/sendData', (req, res) => {
    res.send('Server is running.'); 
});

// Start the server on a specific port (e.g., 3000)
const port = 3000;
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});

function processWithWebPPLAlgorithm(data) 
{
    // Return the response data as an array of integers (replace with actual WebPPL logic)
    return { output: generateRandomNumbers(data.output, data.output.length) };
}

// Define a function to generate random numbers with a specified length
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

