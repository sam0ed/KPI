const { app, BrowserWindow } = require('electron');
const path = require('path');
const url = require('url');
const axios = require('axios');
const { generateLayout } = require('./Crossword-Layout-Generator/layout_generator.js');
if (require('electron-squirrel-startup')) app.quit();

function createMainWindow() {
    const mainWindow = new BrowserWindow({
        title: 'Electron',
        width: 1000,
        height: 600,
        webPreferences: { webSecurity: false },
    });

    const startUrl = url.format({
        pathname: path.join(__dirname, './ui/build/index.html'),
        protocol: 'file:',
    })

    // mainWindow.loadURL(startUrl);
    mainWindow.loadURL('http://localhost:3000');

    // resp = axios.post('http://localhost:1234/v1/chat/completions', {
    //     messages: [
    //         { role: 'system', content: 'Always answer in rhymes.' },
    //         { role: 'user', content: 'Introduce yourself.' }
    //     ],
    //     temperature: 0.7,
    //     max_tokens: -1,
    //     stream: false
    // }).then(response => {
    //     response.data.choices[0].message;
    // })
    //     .catch(error => {
    //         console.error(error);
    //     });

    // input_json = [{ "clue": "that which is established as a rule or model by authority, custom, or general consent", "answer": "standard" }, { "clue": "a machine that computes", "answer": "computer" }, { "clue": "the collective designation of items for a particular purpose", "answer": "equipment" }, { "clue": "an opening or entrance to an inclosed place", "answer": "port" }, { "clue": "a point where two things can connect and interact", "answer": "interface" }]

    // var layout = generateLayout(input_json);
    // var rows = layout.rows;
    // var cols = layout.cols;
    // var table = layout.table;
    // var output_html = layout.table_string; // table as plain text (with HTML line breaks)
    // var output_json = layout.result; // words along with orientation, position, startx, and starty
    // console.log(output_json);


}

app.on('ready', createMainWindow);