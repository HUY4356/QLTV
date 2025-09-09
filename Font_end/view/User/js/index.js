// Hàm dùng để nhúng nội dung HTML vào phần tử có id tương ứng
function includeHTML(id, filePath) {
  const target = document.getElementById(id);
  if (!target) return;

  fetch(filePath)
    .then(response => {
      if (!response.ok) throw new Error(`Không thể tải ${filePath}`);
      return response.text();
    })
    .then(data => {
      target.innerHTML = data;
    })
    .catch(error => {
      console.error("Lỗi khi include:", error);
      target.innerHTML = `<p style="color:red;">Không thể tải nội dung từ ${filePath}</p>`;
    });
}

// Nhúng header và footer vào index.html
includeHTML("header-placeholder", "pages/Header.html");
includeHTML("footer-placeholder", "pages/Footer.html");

// Xác định nội dung chính cần nhúng dựa trên tham số URL
const params = new URLSearchParams(window.location.search);
let page = "Home.html"; // mặc định nếu không có tham số

// Map tham số URL với file HTML tương ứng
const pageMap = {
  feature: "feature.html",
  category: "category.html",
  Login: "Login.html",
  Account: "Account.html",
  Admin: "Admin.html",
  get_help: "help/get_help.html",
  returns: "help/returns.html",
  payment_options: "help/payment_options.html",
  contact_us: "help/contact_us.html"
};

// Duyệt qua các key trong pageMap để kiểm tra tham số
for (const key in pageMap) {
  if (params.has(key)) {
    page = pageMap[key];
    break;
  }
}

// Nhúng nội dung chính vào phần tử có id="main-placeholder"
includeHTML("main-placeholder", `pages/${page}`);





