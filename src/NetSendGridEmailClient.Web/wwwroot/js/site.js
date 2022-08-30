function addRecipientField(recipientType) {
    const newIndex = document
        .querySelectorAll(`input[data-recipientType="${recipientType}"]`)
        .length;
    const newDiv = document.createElement('div');
    newDiv.setAttribute('class', 'input-group mb-3');
    const newInput = document.createElement('input');
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
function removeRecipientField(recipientType) {
    const fields = document
        .querySelectorAll(`input[data-recipientType="${recipientType}"]`);
    // don't let all fields be removed
    if (fields.length === 1)
        return;
    document
        .querySelector(`div[data-recipientType="${recipientType}"]`)
        .removeChild(fields[fields.length - 1].parentNode);
}
function tryUpdateHtmlElementInnerText(elementId, newText) {
    const element = document.getElementById(elementId);
    if (element != null)
        element.innerText = newText;
}
function tryUpdateHtmlInputValue(elementId, newText) {
    const element = document.getElementById(elementId);
    if (element != null)
        element.value = newText;
}
async function domainChanged(newDomain, domainLabelId, senderNameInputId) {
    // update label
    tryUpdateHtmlElementInnerText(domainLabelId, `@${newDomain}`);
    // get default user
    const response = await window.fetch('/settings/getDomainModels', {
        method: 'GET'
    });
    if (!response.ok)
        return;
    const domains = await response.json();
    const domain = domains.filter(x => x.domain === newDomain);
    if (domain.length === 0)
        return;
    tryUpdateHtmlInputValue(senderNameInputId, domain[0].defaultUser);
}
//# sourceMappingURL=site.js.map