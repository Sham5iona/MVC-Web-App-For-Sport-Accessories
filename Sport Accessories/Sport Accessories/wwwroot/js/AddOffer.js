function CheckIsPromoValue() {
    let is_checked = document.getElementById("checkbox").checked;

    if (is_checked) {

        document.getElementById("checkbox").value = true;

    } else {

        document.getElementById("checkbox").value = false;

    }
}
