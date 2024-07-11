document.addEventListener('DOMContentLoaded', () => {
    const aparecer = document.querySelectorAll('.scroll');

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            } else {
                entry.target.classList.remove('visible');
            }
        });
    });

    aparecer.forEach(item => {
        observer.observe(item);
    });
});