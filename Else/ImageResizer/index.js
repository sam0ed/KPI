const { app, BrowserWindow, Menu, ipcMain, shell } = require('electron')
const path = require('path')
const os = require('os')
const fs = require('fs')
const resizeImg = require ('resize-img')

const isMac = process.platform === 'darwin';
const isDev = process.env.NODE_ENV !== 'production';

let mainWindow;

// create the main window
function createMainWindow() {
    mainWindow = new BrowserWindow({
        title: 'Image Resizer',
        width: isDev ? 1000 : 500,
        height: 600,
        webPreferences: {
            contextIsolation: true,
            nodeIntegration: true,
            preload: path.join(__dirname, 'preload.js'),
        }
    })

    if (isDev) {
        mainWindow.webContents.openDevTools();
    }
    mainWindow.loadFile(path.join(__dirname, './renderer/index.html'))
}

function createAboutWindow (){
    const aboutWindow = new BrowserWindow({
        title: 'About image resizer',
        width: 300,
        height: 300,
    })
    aboutWindow.loadFile(path.join(__dirname, './renderer/about.html'))
}

//App is ready
app.on('ready', () => {
    createMainWindow();

    const mainMenu = Menu.buildFromTemplate(menu);
    Menu.setApplicationMenu(mainMenu)


    //Remove mainWindow from memory on close
    mainWindow.on('closed', ()=> {mainWindow = null})
    app.on('activate', () => {
        if (BrowserWindow.getAllWindows().length === 0) {
            createWindow()
        }
    })
})

const menu = [
    ...(
        isMac ? [{ 
            label: app.name,
            submenu: [
                {
                    label: 'About',
                    click: createAboutWindow,
                },
            ],
        }] : []),
    {
        role: 'fileMenu',
    },
    ...(
        !isMac ? [{
            label: 'Help',
            submenu: [{
                label: 'About',
                click: createAboutWindow,  
            }],
        }] : []
    )
]

ipcMain.on('image:resize', (e, options) => {
    options.dest = path.join(os.homedir(), 'imageresizer' );
    resizeImage(options);
    
})

async function resizeImage({imgPath, width, height, dest}){
    try{
        const newPath = await resizeImg(fs.readFileSync(imgPath), {
            width: +width,
            height: +height,
        });

        const filename = path.basename(imgPath);

        //Create dest folder if it doesn`t exist
        if(!fs.existsSync(dest)){
            fs.mkdirSync(dest);
        }

        //Write file to dest folder
        fs.writeFileSync(path.join(dest, filename), newPath);

        //Send success message to render
        mainWindow.webContents.send('image:done');

        //open dest folder
        shell.openPath(dest);
        

    }
    catch(error){
        console.log(error)
    }
}
    
app.on('window-all-closed', () => {
    if (!isMac) {
        app.quit()
    }
}) 
