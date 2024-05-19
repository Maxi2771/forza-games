function desplegar(contentId) {
    var contents = document.getElementsByClassName('desplegable');
    var arrows = document.getElementsByClassName('downarrow');
    
    for (var i = 0; i < contents.length; i++) {
        if (contents[i].id !== contentId) {
            contents[i].style.display = 'none';
        }
    }

    for (var j = 0; j < arrows.length; j++) {
        if (arrows[j].id !== 'downarrow' + contentId.slice(-1)) {
            arrows[j].classList.remove('hidden');
        }
    }
    
    var content = document.getElementById(contentId);
    var arrow = document.getElementById('downarrow' + contentId.slice(-1));

    if (content.style.display === "flex") {
        content.style.display = "none";
        arrow.classList.remove('hidden');
    } else {
        content.style.display = "flex";
        arrow.classList.add('hidden');
    }
}
