function toggleSidebar() {
    document.getElementById('sidebar').classList.toggle('active');
}

function toggleSubmenu(element) {
    const submenu = element.nextElementSibling;
    const isOpen = submenu.classList.contains('open');
    
    const parentMenu = element.closest('.submenu') || element.closest('.sidebar-menu');
    const siblings = parentMenu.querySelectorAll(':scope > .menu-item > .submenu.open');
    siblings.forEach(sibling => {
        if (sibling !== submenu) {
            sibling.classList.remove('open');
            sibling.previousElementSibling.classList.remove('open');
        }
    });

    if (isOpen) {
        submenu.classList.remove('open');
        element.classList.remove('open');
    } else {
        submenu.classList.add('open');
        element.classList.add('open');
    }
}

document.addEventListener('click', function(event) {
    const sidebar = document.getElementById('sidebar');
    const menuToggle = document.querySelector('.menu-toggle');
    
    if (window.innerWidth <= 768) {
        if (!sidebar.contains(event.target) && !menuToggle.contains(event.target)) {
            sidebar.classList.remove('active');
        }
    }
});