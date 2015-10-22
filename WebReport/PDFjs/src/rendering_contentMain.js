var pdfDoc = null,
    pageNum = 1,
    pageRendering = false,
    pageNumPending = null,
    scale = 1.1,
    canvas = document.getElementById('canvasMain'),
    ctx = canvas.getContext('2d');

/**
* Get page info from document, resize canvas accordingly, and render page.
* @param num Page number.
*/
function renderPage(num) {
pageRendering = true;
// Using promise to fetch the page
pdfDoc.getPage(num).then(function(page) {
    var viewport = page.getViewport(scale);
    canvas.height = viewport.height;
    canvas.width = viewport.width;

    // Render PDF page into canvas context
    var renderContext = {
    canvasContext: ctx,
    viewport: viewport
    };
    var renderTask = page.render(renderContext);

    // Wait for rendering to finish
    renderTask.promise.then(function () {
    pageRendering = false;
    if (pageNumPending !== null) {
        // New page rendering is pending
        renderPage(pageNumPending);
        pageNumPending = null;
    }
    });
});

// Update page counters
document.getElementById('page_num').textContent = pageNum + ' / ';
}
    
/**
* If another page rendering in progress, waits until the rendering is
* finised. Otherwise, executes rendering immediately.
*/
function queueRenderPage(num) {
if (pageRendering) {
    pageNumPending = num;
} else {
    renderPage(num);
}
}

/**
* Displays previous page.
*/
function onPrevPage() {
if (pageNum <= 1) {
    return;
}
pageNum--;
queueRenderPage(pageNum);
}
document.getElementById('ImageButtonPrev').addEventListener('click', onPrevPage);

/**
* Displays next page.
*/
function onNextPage() {
if (pageNum >= pdfDoc.numPages) {
    return;
}
pageNum++;
queueRenderPage(pageNum);
}
document.getElementById('ImageButtonNext').addEventListener('click', onNextPage);

/**
* Displays first page.
*/
function onFirstPage() {
pageNum =1;
queueRenderPage(1);
}
document.getElementById('ImageButtonFirstPage').addEventListener('click', onFirstPage);
  
/**
* Displays first page.
*/
function onLastPage() {
pageNum =pdfDoc.numPages;
queueRenderPage(pdfDoc.numPages);
}
document.getElementById('ImageButtonLastPage').addEventListener('click', onLastPage);
  

/**
    * Displays zoomIn.
    */
function onZoomIn() {
    scale += 0.06
    queueRenderPage(pageNum);
}
document.getElementById('ImageButtonZoomIn').addEventListener('click', onZoomIn);
/**
* Displays zoomOut.
*/
function onZoomOut() {
    scale -= 0.06
    queueRenderPage(pageNum);
}
document.getElementById('ImageButtonZoomOut').addEventListener('click', onZoomOut);

function base64ToUint8Array(base64) {//base64 is an byte Array sent from server-side
    var raw = atob(base64); //This is a native function that decodes a base64-encoded string.
    var uint8Array = new Uint8Array(new ArrayBuffer(raw.length));
    for (var i = 0; i < raw.length; i++)
    {
        uint8Array[i] = raw.charCodeAt(i);
    }
    return uint8Array;
}

function GetMainResult() {

            PDFJS.getDocument('ResultHandler?Main=0').then(function (pdfDoc_) {
                pdfDoc = pdfDoc_;
                document.getElementById('page_count').textContent = pdfDoc.numPages;

                // Initial/first page rendering
                renderPage(pageNum);
            });           
}

GetMainResult();
