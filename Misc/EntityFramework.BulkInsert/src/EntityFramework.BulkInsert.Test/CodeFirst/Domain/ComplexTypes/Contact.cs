﻿namespace EntityFramework.BulkInsert.Test.CodeFirst.Domain.ComplexTypes
{
    public class Contact
    {
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
    }
}