const {
  app,
  BrowserWindow,
  ipcMain,
  nativeTheme,
  Notification,
} = require("electron");
const url = require("url");
const path = require("path");
const fs = require("fs");
const https = require("https");

const iconName = path.join(__dirname, "iconForDragAndDrop.png");
const icon = fs.createWriteStream(iconName);
let win;
// Create a new file to copy - you can also copy existing files.
function createWindow() {
  win = new BrowserWindow({
    width: 1200,
    height: 600,
    webPreferences: {
      preload: path.join(__dirname, "preload.js"),
      nodeIntegration: true,
    },
  });

  win.loadFile("index.html");
  win.setMenuBarVisibility(false);
}

https.get("https://img.icons8.com/ios/452/drag-and-drop.png", (response) => {
  response.pipe(icon);
});

const NOTIFICATION_TITLE = "Erreur";
const NOTIFICATION_BODY = "You must provide a valid path";

function showNotification() {
  new Notification({
    title: NOTIFICATION_TITLE,
    body: NOTIFICATION_BODY,
  }).show();
}

// HANDLE THEME
ipcMain.handle("dark-mode:toggle", () => {
  if (nativeTheme.shouldUseDarkColors) {
    nativeTheme.themeSource = "light";
  } else {
    nativeTheme.themeSource = "dark";
  }
  return nativeTheme.shouldUseDarkColors;
});

// HANDLE DRAG AND DROP
ipcMain.on("ondragstart", (event, filePath) => {
  event.sender.startDrag({
    file: path.join(__dirname, filePath),
    icon: iconName,
  });
});

ipcMain.handle("showNotification", () => {
  showNotification();
});

ipcMain.on("openPDF", (event, filePath) => openPDF(filePath));

function openPDF(filePath) {
  console.log("openPDF called");
  let pdfWindow = new BrowserWindow({
    icon: "./build/icon.png",
    width: 1200,
    height: 800,
    webPreferences: {
      plugins: true,
    },
  });

  pdfWindow.loadURL(
    url.format({
      pathname: filePath,
      protocol: "file:",
      slashes: true,
    })
  );

  pdfWindow.setMenu(null);

  pdfWindow.on("closed", function () {
    pdfWindow = null;
  });
}

app.whenReady().then(() => {
  createWindow();

  app.on("activate", () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createWindow();
    }
  });
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") {
    app.quit();
  }
});
