const express = require('express');
const bodyParser = require('body-parser');

const { mapActionIdToName, mapActionNameToId, generateRandomNumbers } = require('./utils');
const { getParameters, setParameters } = require('./utils');
const { randomActions } = require('./placeholderAlgorithm');

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
app.post('/observe', (req, res) => { res.json(processWithWebPPLAlgorithm(req.body)); });

// Placeholder algorithm, replace with call to WebPPL logic
function processWithWebPPLAlgorithm(data) { return { output: randomActions(data) }; }