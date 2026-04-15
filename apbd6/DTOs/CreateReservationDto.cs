using System.ComponentModel.DataAnnotations;

namespace apbd6.DTOs;

public class CreateReservationDto
{
   
   public int RoomId{get;set;}
   [Required]
   public string OrganizerName{get;set;}
   public DateTime Date{get;set;}
   public TimeSpan StartTime{get;set;}
   public TimeSpan EndTime{get;set;}
   public string Status{get;set;}
   [Required]
   public string Topic {get;set;}
   

}