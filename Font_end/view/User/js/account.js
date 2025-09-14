document.querySelectorAll('.tag').forEach(tag => {
  const hue = Math.floor(Math.random() * 360);
  tag.style.setProperty('--tag-bg', `hsl(${hue}, 80%, 70%)`);
});