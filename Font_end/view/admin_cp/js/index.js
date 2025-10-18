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
includeHTML("sidebar-placeholder", "pages/sidebar.html");

// Xác định nội dung chính cần nhúng dựa trên tham số URL
const params = new URLSearchParams(window.location.search);
let page = "danhmuc.html"; // mặc định

// Map tham số URL với file HTML tương ứng
const pageMap = {
  Home: "Home.html",
  danhmuc: "danhmuc.html",
  account: "account.html",
  create: "create.html",
  phong: "phong.html",
};

// Map tham số URL với CSS tương ứng
const cssMap = {
  danhmuc: "css/phong.css",
  account: "css/phong.css",
  create: "css/create.css",
  phong: "css/phong.css",
  // rent: "css/rent.css",
   Home: "css/phong.css",
  // category: "css/category.css",
  // Login: "css/Login.css",
  // Account: "css/account.css",
  // Admin: "css/admin.css",
  // get_help: "css/help.css",
  // returns: "css/help.css",
  // payment_options: "css/help.css",
  // contact_us: "css/help.css",
};

// Map tham số URL với JS tương ứng
const jsMap = {
  danhmuc: "js/danhmuc.js",
  account: "js/account.js",
  create: "js/create.js",
  phong: "js/phong.js",
  // rent: "js/rent.js",
   Home: "js/home.js",
  // category: "js/category.js",
  // Login: "js/Login.js",
  // Account: "js/account.js",
  // Admin: "js/admin.js",
  // get_help: "js/help.js",
  // returns: "js/help.js",
  // payment_options: "js/help.js",
  // contact_us: "js/help.js",
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
