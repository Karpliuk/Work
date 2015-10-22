var pdfDocExtraAttachment = null,
    pageNumExtraAttachment = 1,
    pageRenderingExtraAttachment = false,
    pageNumExtraAttachmentPending = null,
    scaleExtraAttachment = 1.1,
    canvasExtraAttachment = document.getElementById('canvasExtraAttachment'),
    ctxExtraAttachment = canvasExtraAttachment.getContext('2d');

/**
* Get page info from document, resize canvas accordingly, and render page.
* @param num Page number.
*/
function renderPageExtraAttachment(num) {
    pageRenderingExtraAttachment = true;
    // Using promise to fetch the page
    pdfDocExtraAttachment.getPage(num).then(function (page) {
        var viewport = page.getViewport(scaleExtraAttachment);
        canvasExtraAttachment.height = viewport.height;
        canvasExtraAttachment.width = viewport.width;

        // Render PDF page into canvas context
        var renderContext = {
            canvasContext: ctxExtraAttachment,
            viewport: viewport
        };
        var renderTask = page.render(renderContext);

        // Wait for rendering to finish
        renderTask.promise.then(function () {
            pageRenderingExtraAttachment = false;
            if (pageNumExtraAttachmentPending !== null) {
                // New page rendering is pending
                renderPageExtraAttachment(pageNumExtraAttachmentPending);
                pageNumExtraAttachmentPending = null;
            }
        });
    });

    // Update page counters
    document.getElementById('page_numExtraAttachment').textContent = pageNumExtraAttachment + ' / ';
}

/**
* If another page rendering in progress, waits until the rendering is
* finised. Otherwise, executes rendering immediately.
*/
function queuerenderPageExtraAttachment(num) {
    if (pageRenderingExtraAttachment) {
        pageNumExtraAttachmentPending = num;
    } else {
        renderPageExtraAttachment(num);
    }
}

/**
* Displays previous page.
*/
function onPrevPageExtraAttachment() {
    if (pageNumExtraAttachment <= 1) {
        return;
    }
    pageNumExtraAttachment--;
    queuerenderPageExtraAttachment(pageNumExtraAttachment);
}
document.getElementById('ImageButtonPrevExtraAttachment').addEventListener('click', onPrevPageExtraAttachment);

/**
* Displays next page.
*/
function onNextPageExtraAttachment() {
    if (pageNumExtraAttachment >= pdfDocExtraAttachment.numPages) {
        return;
    }
    pageNumExtraAttachment++;
    queuerenderPageExtraAttachment(pageNumExtraAttachment);
}
document.getElementById('ImageButtonNextExtraAttachment').addEventListener('click', onNextPageExtraAttachment);

/**
* Displays first page.
*/
function onFirstPageExtraAttachment() {
    pageNumExtraAttachment = 1;
    queuerenderPageExtraAttachment(1);
}
document.getElementById('ImageButtonFirstPageExtraAttachment').addEventListener('click', onFirstPageExtraAttachment);

/**
* Displays first page.
*/
function onLastPageExtraAttachment() {
    pageNumExtraAttachment = pdfDocExtraAttachment.numPages;
    queuerenderPageExtraAttachment(pdfDocExtraAttachment.numPages);
}
document.getElementById('ImageButtonLastPageExtraAttachment').addEventListener('click', onLastPageExtraAttachment);


/**
    * Displays zoomIn.
    */
function onZoomInExtraAttachment() {
    scaleExtraAttachment += 0.06
    queuerenderPageExtraAttachment(pageNumExtraAttachment);
}
document.getElementById('ImageButtonZoomInExtraAttachment').addEventListener('click', onZoomInExtraAttachment);
/**
* Displays zoomOut.
*/
function onZoomOutExtraAttachment() {
    scaleExtraAttachment -= 0.06
    queuerenderPageExtraAttachment(pageNumExtraAttachment);
}
document.getElementById('ImageButtonZoomOutExtraAttachment').addEventListener('click', onZoomOutExtraAttachment);

function GetExtraAttachmentResult() {

    PDFJS.getDocument('ResultHandler?ExtraAttachment=2').then(function (pdfDocExtraAttachment_) {
        pdfDocExtraAttachment = pdfDocExtraAttachment_;
        document.getElementById('page_countExtraAttachment').textContent = pdfDocExtraAttachment.numPages;

        // Initial/first page rendering
        renderPageExtraAttachment(pageNumExtraAttachment);
    });
}

GetExtraAttachmentResult();
