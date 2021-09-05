using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Core.Issues
{
    [FirestoreData]
    public class Issue
    {
        [FirestoreProperty]
        public string Title { get; set; }
        [FirestoreProperty]
        public string Created
        {
            get; set;
        }
        [FirestoreProperty]
        public string LastUpdated { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string Tag { get; set; }   
        [FirestoreProperty]
        public string Solution { get; set; }
        [FirestoreProperty]
        public bool Solved { get; set; }
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty]
        public string Status { get; set; }
        [FirestoreProperty]
        public string Email { get; set; }
        [FirestoreProperty]
        public bool IsPublic { get; set; }
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;
        public string Ago
        {
            get
            {
                var ts = new TimeSpan(DateTime.UtcNow.Ticks - DateTime.ParseExact(this.Created, "dd-MM-yyyy HH-mm-ss", System.Globalization.CultureInfo.InvariantCulture).Ticks);
                double delta = Math.Abs(ts.TotalSeconds);

                if (delta < 1 * MINUTE)
                    return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

                if (delta < 2 * MINUTE)
                    return "a minute ago";

                if (delta < 45 * MINUTE)
                    return ts.Minutes + " minutes ago";

                if (delta < 90 * MINUTE)
                    return "an hour ago";

                if (delta < 24 * HOUR)
                    return ts.Hours + " hours ago";

                if (delta < 48 * HOUR)
                    return "yesterday";

                if (delta < 30 * DAY)
                    return ts.Days + " days ago";

                if (delta < 12 * MONTH)
                {
                    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    return months <= 1 ? "one month ago" : months + " months ago";
                }
                else
                {
                    int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                    return years <= 1 ? "one year ago" : years + " years ago";
                }
            }
        }
        public List<string> Comments { get; set; } = new List<string>();
    }
}