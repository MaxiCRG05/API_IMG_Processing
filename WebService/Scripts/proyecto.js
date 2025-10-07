document.addEventListener('DOMContentLoaded', () => {
	const dropdownEtiqueta = document.querySelector('.opciones .etiqueta');
	const selectOptionsContainer = document.querySelector('.opciones .select-options');
	const opcionesWrapper = document.querySelector('.opciones');
	const dropdownArrow = document.querySelector('.flecha');

	dropdownEtiqueta.addEventListener('click', () => {
		opcionesWrapper.classList.toggle('open');
	});

	document.addEventListener('click', (event) => {
		if (!opcionesWrapper.contains(event.target) && opcionesWrapper.classList.contains('open')) {
			opcionesWrapper.classList.remove('open');
		}
	});

	const checkboxes = document.querySelectorAll('.opcion input[type="checkbox"]');
	checkboxes.forEach(checkbox => {
		checkbox.addEventListener('change', (event) => {
			const label = event.target.closest('.opcion');
			const itemName = label.textContent.trim(); 

			if (event.target.checked) {
				console.log(`"${itemName}" ha sido seleccionado.`);
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

	submenu.addEventListener("click", (e) => {
		e.stopPropagation(); 
		submenu.classList.toggle("activo");
	});
});