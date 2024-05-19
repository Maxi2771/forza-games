const toggleImage = document.getElementById('menu-icon');
const menu = document.getElementById('menu');

toggleImage.addEventListener('click', function() {
    if (menu.classList.contains('ocultar')) {
        menu.classList.remove('ocultar');
        toggleImage.src = '../img/close.svg'; // Cambia la imagen del icono a "close_icon.png"
    } else {
        menu.classList.add('ocultar');
        toggleImage.src = '../img/menu.svg'; // Cambia la imagen del icono a "menu_icon.png"
    }
});
