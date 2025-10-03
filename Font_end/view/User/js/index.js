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
let page = "Home.html"; // mặc định

// Map tham số URL với file HTML tương ứng
const pageMap = {
  rent: "rent.html",
  Home: "Home.html",
  category: "category.html",
  Login: "Login.html",
  Sign_up: "Sign_up.html",
  Account: "Account.html",
  Admin: "Admin.html",
  get_help: "help/get_help.html",
  returns: "help/returns.html",
  payment_options: "help/payment_options.html",
  contact_us: "help/contact_us.html",
};

// Map tham số URL với CSS tương ứng
const cssMap = {
  rent: "css/rent.css",
  Home: "css/home.css",
  category: "css/category.css",
  Login: "css/Login.css",
  Sign_up: "css/Sign_up.css",
  Account: "css/account.css",
  Admin: "css/admin.css",
  get_help: "css/help.css",
  returns: "css/help.css",
  payment_options: "css/help.css",
  contact_us: "css/help.css",
};

// Map tham số URL với JS tương ứng
const jsMap = {
  rent: "js/rent.js",
  Home: "js/home.js",
  category: "js/category.js",
  Login: "js/Login.js",
  Sign_up: "js/Sign_up.js",
  Account: "js/account.js",
  Admin: "js/admin.js",
  get_help: "js/help.js",
  returns: "js/help.js",
  payment_options: "js/help.js",
  contact_us: "js/help.js",
};

// Kiểm tra tham số URL
let matched = false;
for (const key in pageMap) {
  if (params.has(key)) {
    matched = true;
    page = pageMap[key];

    if (cssMap[key]) addCSS(cssMap[key]);
    if (jsMap[key]) addJS(jsMap[key]);
    break;
  }
}

// Nếu không có tham số nào -> load Home mặc định + CSS/JS
if (!matched) {
  addCSS(cssMap.Home);
  addJS(jsMap.Home);
}

// Nhúng nội dung chính
includeHTML("main-placeholder", `pages/${page}`);

// Hàm thêm CSS an toàn
function addCSS(href) {
  if ([...document.styleSheets].some(sheet => sheet.href && sheet.href.includes(href))) {
    return;
  }
  const link = document.createElement("link");
  link.rel = "stylesheet";
  link.href = href;
  document.head.appendChild(link);
}

// Hàm thêm JS an toàn
function addJS(src) {
  if ([...document.scripts].some(script => script.src && script.src.includes(src))) {
    return;
  }
  const script = document.createElement("script");
  script.src = src;
  script.defer = true;
  document.body.appendChild(script);
}
