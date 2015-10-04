/*
 * Sagi Weisman - 312490030
 * Yasmin Karlin - 308417138
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShaulisBlogDYS.Models
{
    public class Fan
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public int TimeInClub { get; set; }

        public Fan(string firstName, string lastName, bool gender, DateTime dateOfBirth, int timeInClub)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Gender = gender;
            this.BirthDate = dateOfBirth;
            this.TimeInClub = timeInClub;
        }

        public Fan()
        {
        }
    }
}