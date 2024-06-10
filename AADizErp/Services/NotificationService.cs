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

            var andriodNotificationObject = new Dictionary<string, string>();
            andriodNotificationObject.Add("NavigationID", "1");

            var message = new Message
            {
                Token = managerToken,
                Notification = new Notification
                {
                    Title = remoteAttendanceDto.FullName + " Remote Attendance",
                    Body = remoteAttendanceDto.Reason
                },
                Data = andriodNotificationObject,
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

            var andriodNotificationObject = new Dictionary<string, string>();
            andriodNotificationObject.Add("NavigationID", "2");

            var message = new Message
            {
                Token = managerToken,
                Notification = new Notification
                {
                    Title = leaveRequestDto.FullName + " Leave Request",
                    Body = leaveRequestDto.Reason
                },
                Data = andriodNotificationObject,
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

            var andriodNotificationObject = new Dictionary<string, string>();
            andriodNotificationObject.Add("NavigationID", "1");

            var message = new Message
            {
                Token = userToken,
                Notification = new Notification
                {
                    Title = remoteAttendanceDto.Status,
                    Body = remoteAttendanceDto.FullName + ", Your attendance request has been " + remoteAttendanceDto.Status.ToLower()
                },
                Data = andriodNotificationObject,
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

            var andriodNotificationObject = new Dictionary<string, string>();
            andriodNotificationObject.Add("NavigationID", "3");
            var message = new Message
            {
                Token = userToken,
                Notification = new Notification
                {
                    Title = leaveRequestDto.Status,
                    Body = leaveRequestDto.FullName + ", Your leave request has been " + leaveRequestDto.Status.ToLower()
                },
                Data = andriodNotificationObject,
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

        //Legacy version of code
        //private async Task<bool> SendNotificationToUser(PushNotificationRequest request)
        //{
        //    bool isSuccess = false;
        //    string url = "https://fcm.googleapis.com/fcm/send";

        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + "AAAAy6OgnnU:APA91bFoQPy4xdLC-2Vych3DMLCk3GHQOXeDZLJD_AunaKGKLPuZOXB8aT6TTcTPtEzGbA6CvhJAHFTs3UceTSreechXqnsYydfFInS6TQRzFgQhoa6vGEKmj6xx2wXV7quqcJ-uV2mX");

        //        string serializeRequest = JsonConvert.SerializeObject(request);
        //        var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
        //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //        {
        //            isSuccess=true;
        //        }
        //    }
        //    return isSuccess;
        //}

    }
}
