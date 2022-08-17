


// get string from local storage
function getTextFromLocalStorage(key) {
    return localStorage.getItem(key);
}


// save login result to local storage
function saveLoginResultToLocalStorage(key, value) {
    localStorage.setItem(key, value);
}