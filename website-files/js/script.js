$(document).ready(function() {
    // Initialisation du carrousel
    $('.carousel').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        dots: true
    });

    // Gestion du menu
    $("#fullscreen-menu").hide();
	var menu = $("#fullscreen-menu");
    var menuButton = $("#logo-link");

    // Ajoutez un gestionnaire d'événements à chaque lien du menu pour fermer le menu et défilez vers la section correspondante
    $("#fullscreen-menu a, scroll-to-download").on('click', function(e) {
        e.preventDefault(); // Empêche le comportement par défaut du lien
        var targetSection = $($(this).attr('href'));
        $("#fullscreen-menu").fadeOut();

        // Faites défiler vers la section cible avec une animation
		$('html, body').animate({
			scrollTop: targetSection.offset().top - 110 // Ajoutez ou ajustez cette valeur pour corriger la position
		}, 1000);
    });

    $("#logo-link").click(function() {
        $("#fullscreen-menu").fadeToggle();
    });
	
    $(document).on('click', function(e) {
        // Vérifiez si le clic n'est pas dans le menu, dans le contenu du menu, ni sur le bouton d'ouverture
        if (menu.is(e.target) && menu.has(e.target).length === 0 && !menuButton.is(e.target) && !menuButton.has(e.target).length) {
            menu.fadeOut();
        }
    });

    $("#close-button").click(function() {
        $("#fullscreen-menu").fadeOut();
    });

    // Gestion de l'animation des sections au défilement
    var container = $('.scroll-container');
    var sections = container.find('.scroll-section');
    var windowHeight = $(window).height();

    $(window).scroll(function() {
        var scrollPosition = $(window).scrollTop();
        var containerTop = container.offset().top;

        if (scrollPosition + windowHeight >= containerTop) {
            sections.addClass('visible');
        } else {
            sections.removeClass('visible');
        }
    });

    // Fonction pour mettre à jour le sujet du formulaire
    function updateSubject() {
        var categorySelect = document.querySelector('.select');
        var subjectInput = document.querySelector('input[name="_subject"]');

        // Mettez à jour le sujet en fonction de l'option sélectionnée
        switch (categorySelect.value) {
            case 'option1':
                subjectInput.value = '[BUG REPORT]';
                break;
            case 'option2':
                subjectInput.value = '[FEATURES ADDING]';
                break;
            default:
                subjectInput.value = '[NO SUBJECT]'; // Sujet par défaut
        }
    }
	
	    var textarea = document.getElementById('myTextarea');

    textarea.addEventListener('input', function () {
        this.style.height = 'auto';
        this.style.height = (this.scrollHeight) + 'px';
    });
	    var textarea = document.getElementById('myTextarea');

    textarea.addEventListener('input', function () {
        this.style.height = 'auto';
        this.style.height = (this.scrollHeight) + 'px';
    });
	
});
