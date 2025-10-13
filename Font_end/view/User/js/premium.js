document.addEventListener("DOMContentLoaded", () => {
  const cards = document.querySelectorAll(".premium_card");

  cards.forEach((card) => {
    card.addEventListener("mousemove", (e) => {
      const rect = card.getBoundingClientRect();
      const x = e.clientX - rect.left;
      const y = e.clientY - rect.top;
      const centerX = rect.width / 2;
      const centerY = rect.height / 2;
      const rotateX = ((y - centerY) / centerY) * 10; // nghiêng dọc
      const rotateY = ((x - centerX) / centerX) * -10; // nghiêng ngang
      card.style.transform = `rotateX(${rotateX}deg) rotateY(${rotateY}deg) scale(1.05)`;
    });

    card.addEventListener("mouseleave", () => {
      card.style.transform = `rotateX(0deg) rotateY(0deg) scale(1)`;
    });
  });
});
