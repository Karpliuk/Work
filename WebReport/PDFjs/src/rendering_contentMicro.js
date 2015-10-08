var pdfDocMicro = null,
    pageNumMicro = 1,
    pageRenderingMicro = false,
    pageNumMicroPendingMicro = null,
    scaleMicro = 1.1,
    canvasMicro = document.getElementById('canvasMicro'),
    ctxMicro = canvasMicro.getContext('2d');

function renderPageMicro(num) {
    pageRenderingMicro = true;
    // Using promise to fetch the page
    pdfDocMicro.getPage(num).then(function (page) {
        var viewport = page.getViewport(scaleMicro);
        canvasMicro.height = viewport.height;
        canvasMicro.width = viewport.width;

        // Render PDF page into canvas context
        var renderContext = {
            canvasContext: ctxMicro,
            viewport: viewport
        };
        var renderTask = page.render(renderContext);

        // Wait for rendering to finish
        renderTask.promise.then(function () {
            pageRenderingMicro = false;
            if (pageNumMicroPendingMicro !== null) {
                // New page rendering is pending
                renderPageMicro(pageNumMicroPendingMicro);
                pageNumMicroPendingMicro = null;
            }
        });
    });

    // Update page counters
    document.getElementById('page_numMicro').textContent = pageNumMicro + ' / ';
}

/**
* If another page rendering in progress, waits until the rendering is
* finised. Otherwise, executes rendering immediately.
*/
function queuerenderPageMicro(num) {
    if (pageRenderingMicro) {
        pageNumMicroPendingMicro = num;
    } else {
        renderPageMicro(num);
    }
}

/**
* Displays previous page.
*/
function onPrevPageMicro() {
    if (pageNumMicro <= 1) {
        return;
    }
    pageNumMicro--;
    queuerenderPageMicro(pageNumMicro);
}
document.getElementById('ImageButtonPrevMicro').addEventListener('click', onPrevPageMicro);

/**
* Displays next page.
*/
function onNextPageMicro() {
    if (pageNumMicro >= pdfDocMicro.numPages) {
        return;
    }
    pageNumMicro++;
    queuerenderPageMicro(pageNumMicro);
}
document.getElementById('ImageButtonNextMicro').addEventListener('click', onNextPageMicro);

/**
* Displays first page.
*/
function onFirstPageMicro() {
    pageNumMicro = 1;
    queuerenderPageMicro(1);
}
document.getElementById('ImageButtonFirstPageMicro').addEventListener('click', onFirstPageMicro);

/**
* Displays first page.
*/
function onLastPageMicro() {
    pageNumMicro = pdfDocMicro.numPages;
    queuerenderPageMicro(pdfDocMicro.numPages);
}
document.getElementById('ImageButtonLastPageMicro').addEventListener('click', onLastPageMicro);


/**
    * Displays zoomIn.
    */
function onZoomInMicro() {
    scaleMicro += 0.02
    queuerenderPageMicro(pageNumMicro);
}
document.getElementById('ImageButtonZoomInMicro').addEventListener('click', onZoomInMicro);
/**
* Displays zoomOut.
*/
function onZoomOutMicro() {
    scaleMicro -= 0.02
    queuerenderPageMicro(pageNumMicro);
}
document.getElementById('ImageButtonZoomOutMicro').addEventListener('click', onZoomOutMicro);

function GetMicroResult() {

        PDFJS.getDocument('ResultHandler?Micro=1').then(function (pdfDocMicro_) {
            pdfDocMicro = pdfDocMicro_;
            document.getElementById('page_countMicro').textContent = pdfDocMicro.numPages;

            // Initial/first page rendering
            renderPageMicro(pageNumMicro);
        });
}

GetMicroResult();
