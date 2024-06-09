const express = require('express');
const path = require('path');

const app = express();

const PORT = process.env.PORT || 80;
const HOST = process.env.HOST || '0.0.0.0';

app.use(express.static(path.join(__dirname, 'dists/tfg-sn.app')));


app.get('/*', function (req, res) {
  res.sendFile(path.join(__dirname, 'dists/tfg-sn.app', 'index.html'));
});

// Start the server
app.listen(PORT, HOST, () => {
  console.log(`Server is running on http://${HOST}:${PORT}`);
});