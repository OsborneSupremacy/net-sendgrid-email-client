interface AttachmentName {
    fileName: string;
    attachmentId: string;
}

interface DomainModel {
    domain: string;
    defaultUser: string;
}

function addRecipientField(document: Document, recipientType: string) {

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
        .querySelector(`div[data-recipientType="${recipientType}"]`)!
        .appendChild(newDiv);
}

function removeRecipientField(recipientType: string) {

    const fields = document
        .querySelectorAll(`input[data-recipientType="${recipientType}"]`);

    // don't let all fields be removed
    if (fields.length === 1) return;

    document
        .querySelector(`div[data-recipientType="${recipientType}"]`)!
        .removeChild(fields[fields.length - 1].parentNode!);
}

function tryUpdateHtmlElementInnerText(elementId: string, newText: string) {
    const element = document.getElementById(elementId);

    if (element != null)
        element.innerText = newText;
}

function tryUpdateHtmlInputValue(elementId: string, newText: string) {
    const element = document.getElementById(elementId) as HTMLInputElement;

    if (element != null)
        element.value = newText;
}

async function domainChanged(
    newDomain: string,
    domainLabelId: string,
    senderNameInputId: string
) {

    // update label
    tryUpdateHtmlElementInnerText(domainLabelId, `@${newDomain}`);

    // get default user
    const response = await window.fetch('/settings/getDomainModels', {
        method: 'GET'
    });

    if (!response.ok)
        return;

    const domains: DomainModel[] = await response.json();
    const domain = domains.filter(x => x.domain === newDomain);

    if (domain.length === 0)
        return;

    tryUpdateHtmlInputValue(senderNameInputId, domain[0].defaultUser);
}

async function attachmentChanged(
    sender: HTMLInputElement,
    emailPayloadId: string,
    uploadedAttachmentsContainerName: string)
{
    const attachments : FileList | null = sender.files;
    if (attachments === null) return;

    const attachment = attachments[0];
    if (attachment === undefined) return;

    const formData = new FormData();
    formData.append('emailPayloadId', emailPayloadId);
    formData.append('attachment', attachment);

    const response = await window.fetch('/attachment/upload', {
        method: 'POST',
        body: formData
    });

    resetFileInput(sender);

    if (!response.ok)
    {
        const details: string = await response.json();
        alert(details);
        return;
    }

    await getAndListAttachments(emailPayloadId, uploadedAttachmentsContainerName);
}

function resetFileInput(input: HTMLInputElement) {
    input.files = null;
    input.value = '';
}

async function getAttachmentList(emailPayloadId: string): Promise<AttachmentName[]> {

    const response = await window.fetch(`/attachment/getallnames?emailPayloadId=${emailPayloadId}`, {
        method: 'GET'
    });

    if (!response.ok)
        return [];

    const attachments: AttachmentName[] = await response.json();
    return attachments;
}

async function getAndListAttachments(
    emailPayloadId: string,
    uploadedAttachmentsContainerName: string)
{
    const attachments = await getAttachmentList(emailPayloadId);
    listAttachments(emailPayloadId, uploadedAttachmentsContainerName, attachments);
}

function listAttachments(
    emailPayloadId: string,
    uploadedAttachmentsContainerName: string,
    attachments: AttachmentName[])
{
    const container = document.getElementById(uploadedAttachmentsContainerName);
    if (container === null) return;

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

async function removeAttachment(emailPayloadId: string, attachmentId: string)
{
    const formData = new FormData();
    formData.append('emailPayloadId', emailPayloadId);
    formData.append('attachmentId', attachmentId);

    await window.fetch('/attachment/remove', {
        method: 'POST',
        body: formData
    });
}


