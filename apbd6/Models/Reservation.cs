using System.ComponentModel.DataAnnotations;

namespace apbd6.Models;

public class Reservation
{
       public int Id{get;set;}
       public int RoomId{get;set;}
       [Required]
       public string OrganizerName{get;set;}
       public DateTime Date{get;set;}
       public TimeSpan StartTime{get;set;}
       public TimeSpan EndTime{get;set;}
       public string Status{get;set;}
       [Required]
       public string topic{get;set;}
}