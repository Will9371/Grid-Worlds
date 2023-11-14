const express = require('express');
const bodyParser = require('body-parser');
const app = express();

app.use(bodyParser.json());

// Define a route to handle POST requests
app.post('/sendData', (req, res) => 
{
    // Handle the incoming data from Unity here
    const receivedData = req.body;
    // Process the data with your WebPPL algorithm and send a response
    const responseData = processWithWebPPLAlgorithm(receivedData);
    res.json(responseData);
});

// Define a route for the root path ("/")
app.get('/sendData', (req, res) => 
{
    // You can customize the response message.
    res.send('Server is running.'); 
});


// Start the server on a specific port (e.g., 3000)
const port = 3000;
app.listen(port, () => 
{
    console.log(`Server is running on port ${port}`);
});

function processWithWebPPLAlgorithm(data) 
{
    // Assuming data.floats is an array of floats sent from Unity
    // You can use the data as needed in your WebPPL algorithm

    // Generate two random integers between 0 and 2
    const randomInt1 = Math.floor(Math.random() * 3);  // 0, 1, or 2
    const randomInt2 = Math.floor(Math.random() * 3);  // 0, 1, or 2

    // Return the response data as an array of integers
    return { ints: [randomInt1, randomInt2] };
}
