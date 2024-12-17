function openModalWithParameters(notificationId, modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        var deleteButton = document.getElementById('delete-notification-btn');
        deleteButton.setAttribute('data-notification-id', notificationId);
        modal.style.display = 'flex';
    }
}

function deleteNotification(button) {
    var notificationId = button.getAttribute('data-notification-id');

    fetch('/Notifications/Delete', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ id: notificationId }),
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(error => {
                    throw new Error('Failed to delete notification');
                });
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                removeNotificationFromDOM(notificationId);
                checkAndDisplayNoNotificationsMessage();
            } else {
                console.log('Failed to delete notification:', data.message);
            }

        })
        .catch(error => {
            console.error('Error:', error);
        });
    closeModal('confirm-delete-notification')
}

function removeNotificationFromDOM(notificationId) {
    const notification = document.getElementById(`notification-${notificationId}`);

    if (notification) {
        notification.remove();
    } else {
        console.warn(`Notification with ID ${notificationId} not found in DOM.`);
    }
}

function deleteReadNotifications() {
    fetch('/Notifications/DeleteReadNotifications', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(error => {
                    throw new Error(error.message || 'Error deleting notifications');
                });
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                if (data.notifications && Array.isArray(data.notifications)) {
                    updateNotificationList(data.notifications);
                }
            }

        })
        .catch(error => {
            console.error('Error:', error);
        });
    closeModal('confirm-delete-read-notification')
}

function markAllNotificationsAsRead() {
    fetch('/Notifications/MarkAllAsRead', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(error => {
                    throw new Error(error.message || 'Error mark all notifications as read');
                });
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                if (data.notifications && Array.isArray(data.notifications)) {
                    updateNotificationList(data.notifications);
                }
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

function checkAndDisplayNoNotificationsMessage() {
    const notificationList = document.getElementById('notification-list');
    if (notificationList && notificationList.children.length === 0) {
        const notificationsContainer = document.getElementById('notification-info');

        if (!document.getElementById('no-notifications-message')) {
            const noNotificationsMessage = document.createElement('h2');
            noNotificationsMessage.textContent = 'У вас нет уведомлений';
            noNotificationsMessage.id = 'no-notifications-message';
            notificationsContainer.appendChild(noNotificationsMessage);
        }
    }
}

function updateNotificationList(notifications) {
    const notificationList = document.getElementById('notification-list');
    notificationList.innerHTML = '';

    if (notifications.length === 0) {
        checkAndDisplayNoNotificationsMessage();
        return;
    }

    //<td>${notification.sendDate}</td>


    notifications.forEach(notification => {
        const notificationItem = document.createElement('tr');
        notificationItem.Id = `notification-${notification.id}`;
        if (notification.state = 3) {
            notificationItem.className = 'task-tr read-notification';
        } else {
            notificationItem.className = 'task-tr unread-notification';
        }
        notificationItem.innerHTML = `
            <td id="left-td">${notification.message}</td>
            <td>${formatDate(notification.sendDate)}</td>
            <td id="right-td">
                <button class="btn btn-danger btn-exclude-participant"
                        type="button"
                        onclick="openModalWithParameters(${notification.id}, 'confirm-delete-notification')">
                    Удалить
                </button>
            </td>
        `;
        notificationList.appendChild(notificationItem);
    });
}

function formatDate(inputDate) {
    const date = new Date(inputDate);

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${day}.${month}.${year} ${hours}:${minutes}:${seconds}`;
}

// Открыть модальное окно
function openModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = 'flex';  // Показываем модальное окно
    }
}

// Закрыть модальное окно
function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = 'none';  // Скрываем модальное окно
    }
}