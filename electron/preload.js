const { contextBridge, ipcRenderer } = require("electron");

contextBridge.exposeInMainWorld("darkMode", {
  toggle: () => ipcRenderer.invoke("dark-mode:toggle"),
});

contextBridge.exposeInMainWorld("customElec", {
  startDrag: (fileName) => {
    ipcRenderer.send("ondragstart", fileName);
  },
  showNotification: () => {
    ipcRenderer.invoke("showNotification");
  },
  openPDF: (filename) => {
    ipcRenderer.send("openPDF", filename);
  },
});
