namespace AADizErp.Services
{
    public class BadgeManagerService
    {
        private readonly NotificationCounter _notificationCounter;
        private int _unreadCount;

        public BadgeManagerService(NotificationCounter notificationCounter)
        {
            _notificationCounter = notificationCounter;
            _unreadCount = 0;
        }

        /// <summary>
        /// Increment badge by 1 (new request submitted)
        /// </summary>
        public void Increment()
        {
            _unreadCount++;
            _notificationCounter.UpdateBadge(_unreadCount);
        }

        /// <summary>
        /// Decrement badge by 1 (request approved or cleared)
        /// </summary>
        public void Decrement()
        {
            if (_unreadCount > 0)
            {
                _unreadCount--;
                _notificationCounter.UpdateBadge(_unreadCount);
            }
        }

        /// <summary>
        /// Reset badge count (all read)
        /// </summary>
        public void Reset()
        {
            _unreadCount = 0;
            _notificationCounter.ClearBadge();
        }

        /// <summary>
        /// Set badge count directly (useful on app start)
        /// </summary>
        public void SetCount(int count)
        {
            _unreadCount = count;
            _notificationCounter.UpdateBadge(_unreadCount);
        }

        /// <summary>
        /// Get current badge count
        /// </summary>
        public int GetCount() => _unreadCount;
    }
}
