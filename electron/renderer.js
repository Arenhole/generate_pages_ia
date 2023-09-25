document
  .getElementById("toggle-dark-mode")
  .addEventListener("click", async () => {
    console.log("toggle-dark-mode clicked");
    const isDarkMode = await window.darkMode.toggle();
    document.getElementById("theme-source").innerHTML = isDarkMode
      ? "Dark"
      : "Light";
  });

document
  .getElementById("reset-to-system")
  .addEventListener("click", async () => {
    await window.darkMode.system();
    document.getElementById("theme-source").innerHTML = "System";
  });

document.getElementById("drag1").ondragstart = (event) => {
  event.preventDefault();
  window.customElec.startDrag("drag-and-drop-1.md");
};

document.getElementById("drag2").ondragstart = (event) => {
  event.preventDefault();
  window.customElec.startDrag("drag-and-drop-2.md");
};

document.getElementById("open-pdf").addEventListener("click", async () => {
  let filePath = document.getElementById("pathToPdf").value;
  if (filePath) {
    window.customElec.openPDF(filePath);
  } else {
    window.customElec.showNotification();
  }
});
