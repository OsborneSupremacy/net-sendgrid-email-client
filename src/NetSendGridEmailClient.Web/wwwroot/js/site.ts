
function addRecipientField(recipientType: string) {

    let newIndex = document
        .querySelectorAll(`input[data-recipientType="${recipientType}"]`)
        .length;

    let newDiv = document.createElement('div');
    newDiv.setAttribute('class', 'input-group mb-3');

    let newInput = document.createElement('input');
    newInput.setAttribute('type', 'email');
    newInput.setAttribute('class', 'form-control');
    newInput.setAttribute('data-recipientType', recipientType);
    newInput.setAttribute('id', recipientType + `[${newIndex}]`); // might not need this
    newInput.setAttribute('name', recipientType);
    newInput.setAttribute('placeholder', recipientType);

    newDiv.appendChild(newInput);

    document
        .querySelector(`div[data-recipientType="${recipientType}"]`)
        .appendChild(newDiv);
}

function removeRecipientField(recipientType: string) {

    let fields = document
        .querySelectorAll(`input[data-recipientType="${recipientType}"]`);

    // don't let all fields be removed
    if (fields.length === 1) return;

    document
        .querySelector(`div[data-recipientType="${recipientType}"]`)
        .removeChild(fields[fields.length - 1].parentNode);
}

