const express = require('express');
const bodyParser = require('body-parser');
const app = express();

app.use(bodyParser.json());

global.actionCount = 0;

// Define a route to handle setting actionCount
app.post('/setActionCount', (req, res) => {
    // Log the received data
    //console.log('Received data:', req.body);

    // Handle the incoming data to set actionCount
    global.actionCount = req.body.count;
    res.send('Action count set: ' + global.actionCount);
});

// Define a route to handle POST requests
// Handle the incoming data from Unity here
app.post('/sendData', (req, res) => {
    const receivedData = req.body;
    // Process the data with your WebPPL algorithm and send a response
    const responseData = processWithWebPPLAlgorithm(receivedData);
    res.json(responseData);
});

// Define a route for the root path ("/")
app.get('/sendData', (req, res) => {
    // You can customize the response message.
    res.send('Server is running.'); 
});


// Start the server on a specific port (e.g., 3000)
const port = 3000;
app.listen(port, () => {
    console.log(`Server is running on port ${port}`);
});

function processWithWebPPLAlgorithm(data) 
{
    // Return the response data as an array of integers
    // Replace with actual WebPPL logic
    return { ints: generateRandomNumbers(global.actionCount) };
}

// Define a function to generate random numbers with a specified length
var generateRandomNumbers = function(length) 
{
    // Ensure that length is a non-negative integer
    if (typeof length !== 'number' || length < 0) {
        throw new Error('Invalid length ' + length);
    }

    // Base case: when length is 0, return an empty array
    if (length === 0) {
        return [];
    }
    // Recursive case: generate a random number and concatenate it with the rest of the array
    else {
        return [randomInteger(0, 2)].concat(generateRandomNumbers(length - 1));
    }
};

function randomInteger(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

