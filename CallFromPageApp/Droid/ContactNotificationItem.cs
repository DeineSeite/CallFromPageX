using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CallFromPageApp.Droid
{
    public class ContactNotificationItem
    {
        [PrimaryKey]
        public Guid Guid { get; set; }  // notification id unique key

        public int Id { get; set; } // listview adapter position

        public string Title { get; set; }

        public string Message { get; set; }

        public Boolean IsPhone { get; set; }

        public String PhoneOrEmail { get; set; }

        public String LandingPage { get; set; }

        public DateTime DateOfContact {get;set;}

        public DateTime DateCreated { get; set; }

        public bool Shown { get; set; }

        public ContactNotificationItem()
        {
            DateCreated = DateTime.Now;
            DateOfContact = DateTime.Now;
            IsPhone = true;
            Shown = true;
        }

    }
}