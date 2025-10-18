
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

      // ✅ Kích hoạt tất cả <script> trong file được nhúng
      const scripts = target.querySelectorAll("script");
      scripts.forEach(oldScript => {
        const newScript = document.createElement("script");
        if (oldScript.src) {
          newScript.src = oldScript.src;
          newScript.defer = true;
        } else {
          newScript.textContent = oldScript.textContent;
        }
        document.body.appendChild(newScript);
        oldScript.remove();
      });
    })
    .catch(error => {
      console.error("Lỗi khi include:", error);
      target.innerHTML = `<p style="color:red;">Không thể tải nội dung từ ${filePath}</p>`;
    });
}

// --- Nhúng sidebar vào khung chứa ---
includeHTML("sidebar-placeholder", "pages/sidebar.html");

// --- Xác định nội dung chính cần nhúng dựa trên tham số URL ---
const params = new URLSearchParams(window.location.search);
let page = "danhmuc.html"; // Trang mặc định

// --- Map tham số URL với file HTML tương ứng ---
const pageMap = {
  Home: "Home.html",
  danhmuc: "danhmuc.html",
  account: "account.html",
  phong: "phong.html",
  product_renting: "product_renting.html",
  product: "product.html",
  create: "create.html",
};

// --- Map CSS tương ứng ---
const cssMap = {
  Home: "css/main.css",
  danhmuc: "css/main.css",
  account: "css/main.css",
  phong: "css/main.css",
  product_renting: "css/main.css",
  product: "css/main.css",
  create: "css/create.css",
};

// --- Map JS tương ứng ---
const jsMap = {
  Home: "js/home.js",
  danhmuc: "js/danhmuc.js",
  account: "js/account.js",
  phong: "js/phong.js",
  product_renting: "js/product_renting.js",
  product: "js/product.js",
  create: "js/create.js",
};

// --- Kiểm tra URL có tham số nào trùng không ---
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

// --- Nếu không có tham số => load Home mặc định ---
if (!matched) {
  addCSS(cssMap.Home);
  addJS(jsMap.Home);
}

// --- Nhúng nội dung chính ---
includeHTML("main-placeholder", `pages/${page}`);

// --- Hàm thêm CSS ---
function addCSS(href) {
  if ([...document.styleSheets].some(sheet => sheet.href && sheet.href.includes(href))) return;
  const link = document.createElement("link");
  link.rel = "stylesheet";
  link.href = href;
  document.head.appendChild(link);
}

// --- Hàm thêm JS ---
function addJS(src) {
  if ([...document.scripts].some(script => script.src && script.src.includes(src))) return;
  const script = document.createElement("script");
  script.src = src;
  script.defer = true;
  document.body.appendChild(script);
}
