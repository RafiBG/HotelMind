// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", () => {
    const themeToggleBtn = document.getElementById("themeToggleBtn");
    const themeIcon = document.getElementById("themeIcon");
    const body = document.body;

    // 1. Check local storage for the user's saved theme preference
    const savedTheme = localStorage.getItem("hotelMindTheme");

    // 2. Apply the saved theme on page load (Dark mode is our default)
    if (savedTheme === "light") {
        body.classList.add("light-mode");
        if (themeIcon) themeIcon.textContent = "🌙"; // Show moon if in light mode
    } else {
        if (themeIcon) themeIcon.textContent = "☀️"; // Show sun if in dark mode
    }

    // 3. Handle the toggle button click
    if (themeToggleBtn) {
        themeToggleBtn.addEventListener("click", () => {
            // Toggle the 'light-mode' class on the body
            body.classList.toggle("light-mode");

            // Check if light mode is now active
            if (body.classList.contains("light-mode")) {
                localStorage.setItem("hotelMindTheme", "light");
                themeIcon.textContent = "🌙";
            } else {
                localStorage.setItem("hotelMindTheme", "dark");
                themeIcon.textContent = "☀️";
            }
        });
    }
});