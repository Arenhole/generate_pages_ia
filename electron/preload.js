const { contextBridge, ipcRenderer } = require("electron");

contextBridge.exposeInMainWorld("darkMode", {
  toggle: () => ipcRenderer.invoke("dark-mode:toggle"),
  system: () => ipcRenderer.invoke("dark-mode:system"),
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
