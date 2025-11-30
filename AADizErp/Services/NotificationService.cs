using AADizErp.Models.Dtos;
using AADizErp.Models.Dtos.LeaveDtos;
using FirebaseAdmin.Messaging;

namespace AADizErp.Services
{
    public class NotificationService
    {
        private readonly DeviceTokenService _tokenService;
        private readonly NotificationCounter _counter;

        public NotificationService(DeviceTokenService tokenService, NotificationCounter counter)
        {
            _tokenService = tokenService;
            _counter = counter;
        }

        public async Task<bool> SendAttendancePushNotificationToManager(RemoteAttendanceDto remoteAttendanceDto)
        {
            var managerToken = await _tokenService.GetManagerDeviceToken(remoteAttendanceDto.ApprovedBy);
            var notificationObject = new Dictionary<string, string> { { "NavigationID", "1" } };

            var message = BuildMessage(managerToken,
                                       $"{remoteAttendanceDto.FullName} Remote Attendance",
                                       remoteAttendanceDto.Reason,
                                       notificationObject);

            bool result = await SendAndUpdateBadgeAsync(message);
            return result;
        }

        public async Task<bool> SendLeaveRequestPushNotificationToManager(LeaveRequestDto leaveRequestDto)
        {
            var managerToken = await _tokenService.GetManagerDeviceToken(leaveRequestDto.ApprovedBy);
            var notificationObject = new Dictionary<string, string> { { "NavigationID", "2" } };

            var message = BuildMessage(managerToken,
                                       $"{leaveRequestDto.FullName} Leave Request",
                                       leaveRequestDto.Reason,
                                       notificationObject);

            bool result = await SendAndUpdateBadgeAsync(message);
            return result;
        }

        public async Task<bool> SendAttendancePushNotificationBackToUser(RemoteAttendanceDto remoteAttendanceDto)
        {
            var userToken = await _tokenService.GetUserDeviceToken(remoteAttendanceDto.RequestedBy);
            var notificationObject = new Dictionary<string, string> { { "NavigationID", "3" } };

            var message = BuildMessage(userToken,
                                       remoteAttendanceDto.Status,
                                       $"{remoteAttendanceDto.FullName}, Your attendance request has been {remoteAttendanceDto.Status.ToLower()}",
                                       notificationObject);

            bool result = await SendAndUpdateBadgeAsync(message);
            return result;
        }

        public async Task<bool> SendLeavePushNotificationBackToUser(LeaveRequestDto leaveRequestDto)
        {
            var userToken = await _tokenService.GetUserDeviceToken(leaveRequestDto.RequestedBy);
            var notificationObject = new Dictionary<string, string> { { "NavigationID", "4" } };

            var message = BuildMessage(userToken,
                                       leaveRequestDto.Status,
                                       $"{leaveRequestDto.FullName}, Your leave request has been {leaveRequestDto.Status.ToLower()}",
                                       notificationObject);

            bool result = await SendAndUpdateBadgeAsync(message);
            return result;
        }

        private Message BuildMessage(string token, string title, string body, Dictionary<string, string> data)
        {
            return new Message
            {
                Token = token,
                Notification = new Notification { Title = title, Body = body },
                Data = data,
                Android = new AndroidConfig { Notification = new AndroidNotification { Sound = "ding" } },
                Apns = new ApnsConfig { Aps = new Aps { Sound = "iOSding.wav" } }
            };
        }

        private async Task<bool> SendAndUpdateBadgeAsync(Message message)
        {
            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);

                // Increment badge count locally
                _counter.UpdateBadge(1); // You can replace 1 with a dynamic unread count
                return !string.IsNullOrEmpty(response);
            }
            catch
            {
                return false;
            }
        }
    }
}

//using AADizErp.Models.Dtos;
//using AADizErp.Models.Dtos.LeaveDtos;
//using FirebaseAdmin.Messaging;

//namespace AADizErp.Services
//{
//    public class NotificationService
//    {
//        private readonly DeviceTokenService _tokenService;

//        public NotificationService(DeviceTokenService tokenService)
//        {
//            _tokenService = tokenService;
//        }

//        public async Task<bool> SendAttendancePushNotificationToManager(RemoteAttendanceDto remoteAttendanceDto)
//        {
//            bool isSuccess = false;
//            var managerToken = await _tokenService.GetManagerDeviceToken(remoteAttendanceDto.ApprovedBy);

//            var notificationObject = new Dictionary<string, string>();
//            notificationObject.Add("NavigationID", "1");

//            var message = new Message
//            {
//                Token = managerToken,
//                Notification = new Notification
//                {
//                    Title = remoteAttendanceDto.FullName + " Remote Attendance",
//                    Body = remoteAttendanceDto.Reason
//                },
//                Data = notificationObject,
//                Android = new AndroidConfig
//                {
//                    Notification = new AndroidNotification
//                    {
//                        Sound = "ding"
//                    }
//                },
//                Apns = new ApnsConfig
//                {
//                    Aps = new Aps
//                    {
//                        Sound = "iOSding.wav"
//                    }
//                }
//            };

//            var response = SendNotificationToUser(message);
//            if (!String.IsNullOrEmpty(response))
//            {
//                isSuccess = true;
//            }
//            return isSuccess;
//        }

//        public async Task<bool> SendLeaveRequestPushNotificationToManager(LeaveRequestDto leaveRequestDto)
//        {
//            bool isSuccess = false;
//            var managerToken = await _tokenService.GetManagerDeviceToken(leaveRequestDto.ApprovedBy);

//            var notificationObject = new Dictionary<string, string>();
//            notificationObject.Add("NavigationID", "2");

//            var message = new Message
//            {
//                Token = managerToken,
//                Notification = new Notification
//                {
//                    Title = leaveRequestDto.FullName + " Leave Request",
//                    Body = leaveRequestDto.Reason
//                },
//                Data = notificationObject,
//                Android = new AndroidConfig
//                {
//                    Notification = new AndroidNotification
//                    {
//                        Sound = "ding"
//                    }
//                },
//                Apns = new ApnsConfig
//                {
//                    Aps = new Aps
//                    {
//                        Sound = "iOSding.wav"
//                    }
//                }
//            };

//            var response = SendNotificationToUser(message);
//            if (!String.IsNullOrEmpty(response))
//            {
//                isSuccess = true;
//            }
//            return isSuccess;
//        }

//        public async Task<bool> SendAttendancePushNotificationBackToUser(RemoteAttendanceDto remoteAttendanceDto)
//        {
//            bool isSuccess = false;
//            var userToken = await _tokenService.GetUserDeviceToken(remoteAttendanceDto.RequestedBy);

//            var notificationObject = new Dictionary<string, string>();
//            notificationObject.Add("NavigationID", "3");

//            var message = new Message
//            {
//                Token = userToken,
//                Notification = new Notification
//                {
//                    Title = remoteAttendanceDto.Status,
//                    Body = remoteAttendanceDto.FullName + ", Your attendance request has been " + remoteAttendanceDto.Status.ToLower()
//                },
//                Data = notificationObject,
//                Android = new AndroidConfig
//                {
//                    Notification = new AndroidNotification
//                    {
//                        Sound = "ding"
//                    }
//                },
//                Apns = new ApnsConfig
//                {
//                    Aps = new Aps
//                    {
//                        Sound = "iOSding.wav"
//                    }
//                }
//            };

//            var response = SendNotificationToUser(message);
//            if (!String.IsNullOrEmpty(response))
//            {
//                isSuccess = true;
//            }
//            return isSuccess;
//        }

//        public async Task<bool> SendLeavePushNotificationBackToUser(LeaveRequestDto leaveRequestDto)
//        {
//            bool isSuccess = false;
//            var userToken = await _tokenService.GetUserDeviceToken(leaveRequestDto.RequestedBy);

//            var notificationObject = new Dictionary<string, string>();
//            notificationObject.Add("NavigationID", "4");
//            var message = new Message
//            {
//                Token = userToken,
//                Notification = new Notification
//                {
//                    Title = leaveRequestDto.Status,
//                    Body = leaveRequestDto.FullName + ", Your leave request has been " + leaveRequestDto.Status.ToLower()
//                },
//                Data = notificationObject,
//                Android = new AndroidConfig
//                {
//                    Notification = new AndroidNotification
//                    {
//                        Sound = "ding"
//                    }
//                },
//                Apns = new ApnsConfig
//                {
//                    Aps = new Aps
//                    {
//                        Sound = "iOSding.wav"
//                    }
//                }
//            };

//            var response = SendNotificationToUser(message);
//            if (!String.IsNullOrEmpty(response))
//            {
//                isSuccess = true;
//            }

//            return isSuccess;
//        }

//        private string SendNotificationToUser(Message message)
//        {
//            string response = String.Empty;
//            try
//            {
//                response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;

//            }
//            catch (Exception ex)
//            {
//                response = ex.Message;
//            }
//            return response;
//        }
//    }
//}
