// Действия при открытии модального окна
function openExclusionModal(button) {
    var deleteButton = document.getElementById('delete-btn');
    var notificationId = button.getAttribute('data-notification-id');
    deleteButton.setAttribute('data-notification-id', notificationId);
    document.getElementById('confirm-exclusion').showModal();
}

async function deleteNotification(button) {
    try {
        var notificationId = button.getAttribute('data-notification-id');

        const response = await fetch('/Notifications/Delete', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ id: notificationId }),
        });

        if (!response.ok) {
            const error = await response.json();
            alert(error.Message);
            return;
        }

        const notification = document.getElementById(`notification-${notificationId}`);
        notification.remove();
    } catch (error) {
        console.error('Error deleting item:', error);
    }
}