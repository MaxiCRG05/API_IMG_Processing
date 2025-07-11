document.addEventListener('DOMContentLoaded', () => {
    const dropdownEtiqueta = document.querySelector('.opciones .etiqueta');
    const selectOptionsContainer = document.querySelector('.opciones .select-options');
    const opcionesWrapper = document.querySelector('.opciones');
    const dropdownArrow = document.querySelector('.flecha');

    dropdownEtiqueta.addEventListener('click', () => {
        // Toggle la clase 'open' en el contenedor principal del dropdown
        opcionesWrapper.classList.toggle('open');

        // Toggle la rotación de la flecha
        // Esta línea ya está manejada por el CSS, pero si quisieras control JS directo:
        // dropdownArrow.classList.toggle('rotate');
    });

    // Opcional: Cerrar el dropdown si se hace clic fuera de él
    document.addEventListener('click', (event) => {
        // Verifica si el click no fue dentro del contenedor del dropdown
        if (!opcionesWrapper.contains(event.target) && opcionesWrapper.classList.contains('open')) {
            opcionesWrapper.classList.remove('open');
            // dropdownArrow.classList.remove('rotate'); // Si usas JS para rotar la flecha
        }
    });

    // Manejar la selección de los checkboxes (opcional, si necesitas hacer algo cuando se marcan/desmarcan)
    const checkboxes = document.querySelectorAll('.opcion input[type="checkbox"]');
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', (event) => {
            const label = event.target.closest('.opcion');
            const itemName = label.textContent.trim(); // Obtiene el texto de la opción (ej. "Pera")

            if (event.target.checked) {
                console.log(`"${itemName}" ha sido seleccionado.`);
                // Aquí podrías actualizar la etiqueta principal con las selecciones
                // o hacer cualquier otra lógica necesaria.
            } else {
                console.log(`"${itemName}" ha sido deseleccionado.`);
            }
        });
    });
});

document.addEventListener("DOMContentLoaded", () => {
    const menuIcon = document.querySelector(".retroceder");
    const menuLateral = document.getElementById("menuLateral");
    const cerrarMenu = document.getElementById("cerrarMenu");
    const submenu = document.querySelector(".submenu");

    menuIcon.addEventListener("click", () => {
        menuLateral.classList.remove("oculto");
    });

    cerrarMenu.addEventListener("click", () => {
        menuLateral.classList.add("oculto");
    });

    document.addEventListener("click", (e) => {
        if (
            !menuLateral.contains(e.target) &&
            !menuIcon.contains(e.target)
        ) {
            menuLateral.classList.add("oculto");
        }
    });

    // Toggle submenú
    submenu.addEventListener("click", (e) => {
        e.stopPropagation(); // para que no se cierre el menú
        submenu.classList.toggle("activo");
    });
});