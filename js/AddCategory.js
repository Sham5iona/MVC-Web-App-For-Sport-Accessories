function CheckIsActiveValue() {
    let element = document.getElementById("isActive").checked;

    if (element) {
        document.getElementById("isActive").value = true;
    } else {
        document.getElementById("isActive").value = false;
    }
}