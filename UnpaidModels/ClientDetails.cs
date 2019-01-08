using System;
using System.Collections.Generic;
using System.Text;

namespace UnpaidModels
{
    public enum Gender
    {
        Male,
        Female
    }

    public class ClientDetails
    {
        public Decimal Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmployerBusiness { get; set; }
        public string EmployerGroup { get; set; }
        public string FirstName { get; set; }
        public Gender Gender { get; set; }
        public string IdNumber { get; set; }
        public string IdNumberType { get; set; }
        public string Initials { get; set; }
        public string LastNCBaffectingclaimdate { get; set; }
        public string LicenceEndorsements { get; set; }
        public string MaritalStatus { get; set; }
        public string MaritalStatusDescription { get; set; }
        public string MobileTelNo { get; set; }
        public string NoClaimsDiscount { get; set; }
        public string OccupationStatus { get; set; }
        public string OccupationStatusDescription { get; set; }
        public string PersonDescription { get; set; }
        public int PersonNumber { get; set; }
        public string PreviousClaims { get; set; }
        public string PreviousInsuranceCancelled { get; set; }
        public string PreviousLosses { get; set; }
        public string ReferenceNumber { get; set; }
        public string Relationship { get; set; }
        public string RelationshipCode { get; set; }
        public string RelationshipGroup { get; set; }
        public int SpousePartnerSeqno { get; set; }
        public string Status { get; set; }
        public string Surname { get; set; }
        public string TitleCode { get; set; }
        public DateTime? LastClaimLossDate { get; set; }
        public int NoOfClaims { get; set; }
        public int NoOfLosses { get; set; }
    }
}
