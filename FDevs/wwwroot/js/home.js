$('.owl-carousel').owlCarousel({
    loop: false,
    margin: 25,
    nav: false,
    responsive: {
        700: {
            items: 1
        },
        1000: {
            items: 2
        },
        1450: {
            items: 3
        }
    }
})
function changeBtn(clickedBtn) {
    clickedBtn.classList.remove('text-dark');
    clickedBtn.classList.add('chosen');

    let buttons = document.querySelectorAll('.btnCategory');
    buttons.forEach(btn => {
        if (btn !== clickedBtn) {
            btn.classList.remove('chosen');
            btn.classList.add('text-dark');
        }
    });
}