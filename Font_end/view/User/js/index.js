document.addEventListener("DOMContentLoaded", () => {

  /* =====================================================
     🧩 HÀM NHÚNG HTML DÙNG FETCH
  ===================================================== */
  async function includeHTML(id, filePath, callback) {
    const target = document.getElementById(id);
    if (!target) return;

    try {
      const response = await fetch(filePath);
      if (!response.ok) throw new Error(`Không thể tải ${filePath}`);
      const html = await response.text();
      target.innerHTML = html;

      if (typeof callback === "function") callback(); // gọi sau khi load xong
    } catch (err) {
      console.error(`Lỗi khi include ${filePath}:`, err);
      target.innerHTML = `<p style="color:red;">Không thể tải nội dung từ ${filePath}</p>`;
    }
  }

  /* =====================================================
     🧱 NHÚNG HEADER & FOOTER
  ===================================================== */
  includeHTML("header-placeholder", "pages/Header.html", () => {
    // Sau khi header được load => nạp script header
    addJS("js/header.js");
  });

  includeHTML("footer-placeholder", "pages/Footer.html");

  /* =====================================================
     📄 NHÚNG NỘI DUNG CHÍNH
  ===================================================== */
  const params = new URLSearchParams(window.location.search);
  let page = "Home.html"; // Mặc định

  const pageMap = {
    rent: "rent.html",
    premium: "premium.html",
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

  const cssMap = {
    rent: "css/rent.css",
    premium: "css/premium.css",
    Home: "css/home.css",
    Header: "css/header.css",
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

  const jsMap = {
    rent: "js/rent.js",
    Home: "js/home.js",
    Header: "js/header.js",
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

  // Nếu không có tham số nào => load Home mặc định
  if (!matched) {
    addCSS(cssMap.Home);
    addJS(jsMap.Home);
  }

  // Nhúng nội dung chính
  includeHTML("main-placeholder", `pages/${page}`);

  /* =====================================================
     🧩 HÀM THÊM CSS/JS AN TOÀN
  ===================================================== */
  function addCSS(href) {
    const exists = [...document.styleSheets].some(sheet => sheet.href && sheet.href.includes(href));
    if (exists) return;
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = href;
    document.head.appendChild(link);
  }

  function addJS(src) {
    const exists = [...document.scripts].some(script => script.src && script.src.includes(src));
    if (exists) return;
    const script = document.createElement("script");
    script.src = src;
    script.defer = true;
    document.body.appendChild(script);
  }

});
