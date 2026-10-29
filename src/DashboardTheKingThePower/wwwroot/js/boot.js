// Boot: redirect de SPA, tema/acentos e handlers globais.
// Ficou fora do index.html por causa da CSP estrita (sem 'unsafe-inline').
(function () {
    var redirect = sessionStorage.getItem('redirect');
    if (redirect) {
        sessionStorage.removeItem('redirect');
        history.replaceState(null, null, redirect);
    }
})();

initTheme();
initAccent();

window.addEventListener('error', function (e) {
    console.error('[GLOBAL ERROR]', e.message, e.filename, e.lineno, e.colno, e.error);
});
window.addEventListener('unhandledrejection', function (e) {
    console.error('[UNHANDLED PROMISE]', e.reason);
});
document.addEventListener('DOMContentLoaded', function () {
    var dismiss = document.querySelector('#blazor-error-ui .dismiss');
    if (dismiss) {
        dismiss.addEventListener('click', function () {
            document.getElementById('blazor-error-ui').style.display = 'none';
        });
    }
});
