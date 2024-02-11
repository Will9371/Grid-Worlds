const express = require('express');
const bodyParser = require('body-parser');

const { mapActionIdToName, mapActionNameToId, generateRandomNumbers } = require('./utils');
const { getParameters, setParameters } = require('./utils');
const { randomAction, randomActions } = require('./placeholderAlgorithm');

let queryMode = false;
let queryCount = 0;

const app = express();
app.use(bodyParser.json());

// Start the server on a specific port (3000)
const port = 3000;
app.listen(port, () => { console.log(`Server is running on port ${port}`); });

app.post('/getParameters', (req, res) => { getParameters(req, res) });
app.post('/setParameters', (req, res) => { setParameters(req, res) });
app.post('/beginEpisode', (req, res) => { res.send(); });
app.post('/endEpisode', (req, res) => { res.send(); });
app.post('/agentEvent', (req, res) => { res.send(); });
app.post('/observe', (req, res) => { handleObservations(req, res); });
app.post('/query', (req, res) => { handleQuery(req, res); });

// Placeholder algorithm, replace with call to WebPPL logic
function processWithWebPPLAlgorithm(data) {
    return randomAction(data);
    //return ["Move Right"];
}

function handleObservations(req, res) {
    queryCount = 0;
    handleInput(req.body, res);
}

function handleQuery(req, res) {
    handleInput(req.body, res);
}

function handleInput(data, res) {
    if (hasQuery()) {
        processQuery(data, res);
    } else {
        processAction(data, res);
    }
}

// Placeholder function to check if there is a query
function hasQuery() {
    //return queryCount < 4;
    return false;
}

function processQuery(data, res) {
    const query = generateQuery();
    const output = { mode: "query", actions: query }
    //console.log("query", queryCount, output);
    res.json(output);
    queryCount++;
}

// Replace this with your actual logic for generating queries
function generateQuery() {
    const directions = [["Move Left"], ["Move Right"], ["Move Up"], ["Move Down"]];
    return directions[queryCount];
}

function processAction(data, res) {
    const actions = processWithWebPPLAlgorithm(data);
    const output = { mode: "action", actions: actions }
    //console.log(output.mode, actions);
    res.json(output);
}
