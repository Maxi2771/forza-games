function desplegar(contentId) {
    var contents = document.getElementsByClassName('desplegable');
    var arrows = document.getElementsByClassName('arrow');
    
    // Cerrar todos los contenidos y mostrar todas las flechas
    for (var i = 0; i < contents.length; i++) {
        contents[i].style.display = 'none';  // Ocultar contenido
    }

    for (var j = 0; j < arrows.length; j++) {
        arrows[j].classList.remove('hidden');  // Mostrar flecha
    }
    
    // Obtener el contenido actual y la flecha correspondiente
    var content = document.getElementById(contentId);
    var arrow = document.getElementById('downarrow' + contentId.slice(-1));

    // Alternar la visibilidad del contenido actual y la flecha correspondiente
    if (content.style.display === "flex") {
        content.style.display = "none";  // Ocultar contenido
        arrow.classList.remove('hidden');  // Mostrar flecha
    } else {
        content.style.display = "flex";  // Mostrar contenido
        arrow.classList.add('hidden');  // Ocultar flecha
    }
}

