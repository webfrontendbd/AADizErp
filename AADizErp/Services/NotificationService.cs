using AADizErp.Models.Dtos;
using AADizErp.Models.Dtos.LeaveDtos;
using FirebaseAdmin.Messaging;

namespace AADizErp.Services
{
    public class NotificationService
    {
        private readonly DeviceTokenService _tokenService;

        public NotificationService(DeviceTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<bool> SendAttendancePushNotificationToManager(RemoteAttendanceDto remoteAttendanceDto)
        {
            bool isSuccess = false;
            var managerToken = await _tokenService.GetManagerDeviceToken(remoteAttendanceDto.ApprovedBy);

            var notificationObject = new Dictionary<string, string>();
            notificationObject.Add("NavigationID", "1");

            var message = new Message
            {
                Token = managerToken,
                Notification = new Notification
                {
                    Title = remoteAttendanceDto.FullName + " Remote Attendance",
                    Body = remoteAttendanceDto.Reason
                },
                Data = notificationObject,
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        Sound = "ding"
                    }
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Sound = "iOSding.wav"
                    }
                }
            };

            var response = SendNotificationToUser(message);
            if (!String.IsNullOrEmpty(response))
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        public async Task<bool> SendLeaveRequestPushNotificationToManager(LeaveRequestDto leaveRequestDto)
        {
            bool isSuccess = false;
            var managerToken = await _tokenService.GetManagerDeviceToken(leaveRequestDto.ApprovedBy);

            var notificationObject = new Dictionary<string, string>();
            notificationObject.Add("NavigationID", "2");

            var message = new Message
            {
                Token = managerToken,
                Notification = new Notification
                {
                    Title = leaveRequestDto.FullName + " Leave Request",
                    Body = leaveRequestDto.Reason
                },
                Data = notificationObject,
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        Sound = "ding"
                    }
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Sound = "iOSding.wav"
                    }
                }
            };

            var response = SendNotificationToUser(message);
            if (!String.IsNullOrEmpty(response))
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        public async Task<bool> SendAttendancePushNotificationBackToUser(RemoteAttendanceDto remoteAttendanceDto)
        {
            bool isSuccess = false;
            var userToken = await _tokenService.GetUserDeviceToken(remoteAttendanceDto.RequestedBy);

            var notificationObject = new Dictionary<string, string>();
            notificationObject.Add("NavigationID", "3");

            var message = new Message
            {
                Token = userToken,
                Notification = new Notification
                {
                    Title = remoteAttendanceDto.Status,
                    Body = remoteAttendanceDto.FullName + ", Your attendance request has been " + remoteAttendanceDto.Status.ToLower()
                },
                Data = notificationObject,
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        Sound = "ding"
                    }
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Sound = "iOSding.wav"
                    }
                }
            };

            var response = SendNotificationToUser(message);
            if (!String.IsNullOrEmpty(response))
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        public async Task<bool> SendLeavePushNotificationBackToUser(LeaveRequestDto leaveRequestDto)
        {
            bool isSuccess = false;
            var userToken = await _tokenService.GetUserDeviceToken(leaveRequestDto.RequestedBy);

            var notificationObject = new Dictionary<string, string>();
            notificationObject.Add("NavigationID", "4");
            var message = new Message
            {
                Token = userToken,
                Notification = new Notification
                {
                    Title = leaveRequestDto.Status,
                    Body = leaveRequestDto.FullName + ", Your leave request has been " + leaveRequestDto.Status.ToLower()
                },
                Data = notificationObject,
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        Sound = "ding"
                    }
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Sound = "iOSding.wav"
                    }
                }
            };

            var response = SendNotificationToUser(message);
            if (!String.IsNullOrEmpty(response))
            {
                isSuccess = true;
            }

            return isSuccess;
        }

        private string SendNotificationToUser(Message message)
        {
            string response = String.Empty;
            try
            {
                response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;

            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
    }
}
