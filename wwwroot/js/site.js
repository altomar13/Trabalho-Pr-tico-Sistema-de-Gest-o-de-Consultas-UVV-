document.addEventListener('DOMContentLoaded', () => {
  document.querySelectorAll('.alert').forEach(alert => {
    setTimeout(() => alert.style.opacity = '0', 4500);
    setTimeout(() => alert.remove(), 5000);
  });
});
