document
  .getElementById("toggle-dark-mode")
  .addEventListener("click", async () => {
    console.log("toggle-dark-mode clicked");
    await window.darkMode.toggle();
  });

document.getElementById("drag1").ondragstart = (event) => {
  event.preventDefault();
  window.customElec.startDrag("drag-and-drop-1.md");
};

document.getElementById("upload").addEventListener("click", async () => {
  console.log(document.getElementById("fileUpload").files);
  let filePath = document.getElementById("fileUpload").files[0].path;
  if (filePath) {
    window.customElec.openPDF(filePath);
  } else {
    window.customElec.showNotification();
  }
});

const progressBar = document.getElementById("progressBar");
const fileUpload = document.getElementById("fileUpload");

fileUpload.addEventListener("change", (event) => {
  const file = event.target.files[0];
  const fileSize = file.size;
  let loaded = 0;

  const reader = new FileReader();

  reader.onload = function (e) {
    // Do something with the contents of the file if needed
  };

  reader.onprogress = function (e) {
    if (e.lengthComputable) {
      loaded = (e.loaded / fileSize) * 100;
      progressBar.style.width = loaded + "%";
      progressBar.innerHTML = Math.round(loaded) + "%";
    }
  };

  reader.readAsDataURL(file);
});
