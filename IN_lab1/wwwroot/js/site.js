function IncreaseReloadCounter() {
    localStorage.setItem('ReloadIteration', Number(localStorage.getItem('ReloadIteration')) + 1)
    sessionStorage.setItem('ReloadIteration', Number(sessionStorage.getItem('ReloadIteration')) + 1)
}

let reload_iteration = localStorage.getItem('ReloadIteration')

if (reload_iteration === null) {
    localStorage.setItem('ReloadIteration', 1)
}

let sesion_reload_iteration = sessionStorage.getItem('ReloadIteration')

if (sesion_reload_iteration === null) {
    sessionStorage.setItem('ReloadIteration', localStorage.getItem('ReloadIteration'))
}

function checkReload() {
    if (sessionStorage.getItem('ReloadIteration') < localStorage.getItem('ReloadIteration')) {
        sessionStorage.setItem('ReloadIteration', localStorage.getItem('ReloadIteration'))
        location.reload()
    }
}

setInterval(checkReload, 1000);
