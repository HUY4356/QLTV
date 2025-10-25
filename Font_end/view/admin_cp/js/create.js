fetch("pages/create.html")
  .then(res => res.text())
  .then(data => {
    document.getElementById("content").innerHTML = data;
  })
  .catch(err => {
    console.error("Không thể tải nội dung từ pages/create.html");
  });
