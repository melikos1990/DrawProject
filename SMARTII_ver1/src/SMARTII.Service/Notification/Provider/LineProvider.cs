﻿using System;
using SMARTII.Domain.Notification;

namespace SMARTII.Service.Notification.Provider
{
    public class LineProvider : INotificationProvider
    {
        public void Send(ISenderPayload payload, Action<object> beforeSend = null, Action<object> afterSend = null)
        {
            throw new NotImplementedException();
        }
    }
}