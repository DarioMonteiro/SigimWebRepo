using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Presentation.WebUI.Helpers
{
    public static class CustomNotificationSummaryHelper
    {
        public static string CustomNotificationSummary(this HtmlHelper htmlHelper, MessageQueue messageQueue)
        {
            StringBuilder stringRetorno = new StringBuilder();
            if (messageQueue != null)
            {
                foreach (var message in messageQueue.GetAll())
                {
                    stringRetorno.Append("<div class=\"alert " + GetCssClassForTypeNotification(message.Type) + " fade in\">");
                    stringRetorno.Append("<button class=\"close\" data-dismiss=\"alert\">×</button>");
                    stringRetorno.Append("<i class=\"fa-fw fa " + GetCssClassForIconNotification(message.Type) + "\"></i>\n");
                    stringRetorno.Append(message.Text.Replace("\n", "<br />"));
                    stringRetorno.Append("</div>");
                }
                messageQueue.Clear();
            }
            return stringRetorno.ToString();
        }

        private static string GetCssClassForTypeNotification(TypeMessage type)
        {
            switch (type)
            {
                case TypeMessage.Error:
                    return "alert-danger";
                case TypeMessage.Info:
                    return "alert-info";
                case TypeMessage.Success:
                    return "alert-success";
                case TypeMessage.Warning:
                    return "alert-warning";
                default:
                    return string.Empty;
            }
        }

        private static string GetCssClassForIconNotification(TypeMessage type)
        {
            switch (type)
            {
                case TypeMessage.Error:
                    return "fa-times";
                case TypeMessage.Info:
                    return "fa-info";
                case TypeMessage.Success:
                    return "fa-check";
                case TypeMessage.Warning:
                    return "fa-warning";
                default:
                    return string.Empty;
            }
        }
    }
}