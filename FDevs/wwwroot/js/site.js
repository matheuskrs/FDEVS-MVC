function filter(videoId, clickedButton) {
    document.querySelectorAll('.video').forEach(card => {
        card.style.display = "flex";
        if (card.getAttribute('data-video-id') !== videoId && videoId !== '') {
            card.style.display = "none";
        }
    });

    let cardCount = 0;
    document.querySelectorAll('.video').forEach(card => {
        cardCount += card.style.display == "flex" ? 1 : 0;
    });

    document.querySelectorAll('.btn-filter').forEach(button => {
        button.classList.remove('bg-gradient-success');
        button.classList.remove('active');
    });

    clickedButton.classList.add('bg-gradient-success');
    clickedButton.classList.add('active');
}
