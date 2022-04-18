function scrollIntoView(elementId) {
    var element = document.getElementById(elementId);
    if (!element) {
        return;
    }

    element.scrollIntoView({ behavior: 'smooth' });
}