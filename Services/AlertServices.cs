﻿namespace InventoryWorld.Services
{
    public  class AlertServices
    {
       public enum Alerts
        {
            Success,
            Info,
            Warning,
            Danger
        }
        public static string ShowAlert(Alerts obj, string message)
        {
            string alertDiv = "";
            switch (obj)
            {
                case Alerts.Success:
                    alertDiv = "<div class='alert alert-success alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>×</button><strong> Success!</ strong > " + message + "</a>.</div>";
                    break;
                case Alerts.Danger:
                    alertDiv = "<div class='alert alert-danger alert-dismissible' id='alert'><button type='button' class='close' data-dismiss='alert'>×</button><strong> Error!</ strong > " + message + "</a>.</div>";
                    break;
                case Alerts.Info:
                    alertDiv = "<div class='alert alert-info alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>×</button><strong> Info!</ strong > " + message + "</a>.</div>";
                    break;
                case Alerts.Warning:
                    alertDiv = "<div class='alert alert-warning alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>×</button><strong> Warning!</strong> " + message + "</a>.</div>";
                    break;
            }
            return alertDiv;
        }
    }
}
