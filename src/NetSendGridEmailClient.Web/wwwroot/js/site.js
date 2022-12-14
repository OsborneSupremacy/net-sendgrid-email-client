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
async function attachmentChanged(sender, emailPayloadId, uploadedAttachmentsContainerName) {
    const attachments = sender.files;
    if (attachments === null)
        return;
    const attachment = attachments[0];
    if (attachment === undefined)
        return;
    const formData = new FormData();
    formData.append('emailPayloadId', emailPayloadId);
    formData.append('attachment', attachment);
    const response = await window.fetch('/attachment/upload', {
        method: 'POST',
        body: formData
    });
    resetFileInput(sender);
    if (!response.ok) {
        const details = await response.json();
        alert(details);
        return;
    }
    await getAndListAttachments(emailPayloadId, uploadedAttachmentsContainerName);
}
function resetFileInput(input) {
    input.files = null;
    input.value = '';
}
async function getAttachmentList(emailPayloadId) {
    const response = await window.fetch(`/attachment/getallnames?emailPayloadId=${emailPayloadId}`, {
        method: 'GET'
    });
    if (!response.ok)
        return [];
    const attachments = await response.json();
    return attachments;
}
async function getAndListAttachments(emailPayloadId, uploadedAttachmentsContainerName) {
    const attachments = await getAttachmentList(emailPayloadId);
    listAttachments(emailPayloadId, uploadedAttachmentsContainerName, attachments);
}
function listAttachments(emailPayloadId, uploadedAttachmentsContainerName, attachments) {
    const container = document.getElementById(uploadedAttachmentsContainerName);
    if (container === null)
        return;
    container.replaceChildren();
    attachments.forEach((attachment) => {
        const newDiv = document.createElement('div');
        newDiv.setAttribute('class', 'alert alert-secondary alert-dismissible fade show');
        newDiv.setAttribute('role', 'alert');
        newDiv.innerText = attachment.fileName;
        const removeButton = document.createElement('button');
        removeButton.setAttribute('type', 'button');
        removeButton.setAttribute('class', 'btn-close');
        removeButton.setAttribute('data-bs-dismiss', 'alert');
        removeButton.setAttribute('aria-label', 'Remove');
        removeButton.setAttribute('onclick', `removeAttachment("${emailPayloadId}", "${attachment.attachmentId}");`);
        newDiv.appendChild(removeButton);
        container.appendChild(newDiv);
    });
}
async function removeAttachment(emailPayloadId, attachmentId) {
    const formData = new FormData();
    formData.append('emailPayloadId', emailPayloadId);
    formData.append('attachmentId', attachmentId);
    await window.fetch('/attachment/remove', {
        method: 'POST',
        body: formData
    });
}
//# sourceMappingURL=site.js.map