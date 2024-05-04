

function ChangeFavouriteEmoji() {
    var element = document.getElementById("favourite");
    if (element.classList.contains("fa-star-o")) {
        element.classList.remove("fa-star-o");
        element.classList.add("fa-star");
        element.style.color = "gold"; // Set color to default

    } else {
        element.classList.remove("fa-star");
        element.classList.add("fa-star-o");
        element.style.color = ""; // Set color to default
    }
}
