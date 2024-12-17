function goBack() {
    const lastPage = localStorage.getItem('lastVisitedPage');
    if (lastPage) {
        window.location.href = lastPage;
    } else {
        window.location.href = '/';
    }
}


