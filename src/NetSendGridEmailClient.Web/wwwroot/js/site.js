function addRecipientField(recipientType) {
    var newIndex = document
        .querySelectorAll("input[data-recipientType=\"".concat(recipientType, "\"]"))
        .length;
    var newDiv = document.createElement('div');
    newDiv.setAttribute('class', 'input-group mb-3');
    var newInput = document.createElement('input');
    newInput.setAttribute('type', 'email');
    newInput.setAttribute('class', 'form-control');
    newInput.setAttribute('data-recipientType', recipientType);
    newInput.setAttribute('id', recipientType + "[".concat(newIndex, "]")); // might not need this
    newInput.setAttribute('name', recipientType);
    newInput.setAttribute('placeholder', recipientType);
    newDiv.appendChild(newInput);
    document
        .querySelector("div[data-recipientType=\"".concat(recipientType, "\"]"))
        .appendChild(newDiv);
}
function removeRecipientField(recipientType) {
    var fields = document.querySelectorAll("input[data-recipientType=\"".concat(recipientType, "\"]"));
    // don't let all fields be removed
    if (fields.length === 1)
        return;
    document
        .querySelector("div[data-recipientType=\"".concat(recipientType, "\"]"))
        .removeChild(fields[fields.length - 1].parentNode);
}
//# sourceMappingURL=site.js.map