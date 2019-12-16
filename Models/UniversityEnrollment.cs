using System;
namespace AspNetIdentitySample.Models
{
    public class UniversityEnrollment
    {
        public int Id { get; set; }
        public string PESEL { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Points { get; set; }
    }
}
