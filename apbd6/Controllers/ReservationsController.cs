using System.Collections;
using apbd6.DTOs;
using apbd6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apbd6.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {

        public static List<Reservation> reservations = new List<Reservation>()
        {
            new Reservation()
            {
                Id = 1,
                RoomId = 1,
                Date = new DateTime(2026, 4, 25),
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(12, 0, 0),
                Status = "planned",
                topic = "Sprint planning",
                OrganizerName = "Jan Kowalski"
            },
            new Reservation()
            {
                Id = 2,
                RoomId = 2,
                Date = new DateTime(2026, 4, 26),
                StartTime = new TimeSpan(14, 0, 0),
                EndTime = new TimeSpan(15, 30, 0),
                Status = "planned",
                topic = "Lab z baz danych",
                OrganizerName = "Anna Nowak"
            },
            new Reservation()
            {
                Id = 3,
                RoomId = 3,
                Date = new DateTime(2026, 4, 27),
                StartTime = new TimeSpan(9, 30, 0),
                EndTime = new TimeSpan(11, 0, 0),
                Status = "confirmed",
                topic = "Code review",
                OrganizerName = "Piotr Wiśniewski"
            },
            new Reservation()
            {
                Id = 4,
                RoomId = 1,
                Date = new DateTime(2026, 4, 28),
                StartTime = new TimeSpan(13, 0, 0),
                EndTime = new TimeSpan(14, 0, 0),
                Status = "cancelled",
                topic = "Daily standup",
                OrganizerName = "Maria Zielińska"
            },
            new Reservation()
            {
                Id = 5,
                RoomId = 2,
                Date = new DateTime(2026, 4, 29),
                StartTime = new TimeSpan(11, 0, 0),
                EndTime = new TimeSpan(13, 0, 0),
                Status = "canceled",
                topic = "Warsztaty z AI",
                OrganizerName = "Tomasz Lewandowski"
            }
        };
    

        
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Reservation>> Get([FromRoute] int id)
        {
            var reservation = reservations.FirstOrDefault(x=>  x.Id == id);
            if (reservation == null)
                return NotFound();
            
            return Ok(reservation);
        }


        [HttpGet]
        public IActionResult GetRooms(
            [FromQuery] DateTime? date,
            [FromQuery] string? status,
            [FromQuery] int? roomId)
        {
            var query = reservations.AsEnumerable();

            if (date.HasValue)
                query = query.Where(r => r.Date.Equals(date));

            if (status != null || status.Length == 0)
                query = query.Where(r => r.Status.Equals(status));

            if (roomId.HasValue)
                query = query.Where(r => r.RoomId.Equals(roomId));

            var result = query.ToList();
            if (result.Count == 0)
            {
                return NotFound();
            }
            return Ok(result);
            
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateReservationDto createReservationDto)
        {
            
            if (createReservationDto.StartTime >= createReservationDto.EndTime || RoomsController.rooms.All(x => x.Id != createReservationDto.RoomId) )
            {
                return BadRequest("Wrong  start time or end time");
            }
           

            if (reservations.Any(r =>
                    r.RoomId == createReservationDto.RoomId &&
                    r.Date == createReservationDto.Date &&
                    r.StartTime < createReservationDto.EndTime &&
                    r.EndTime > createReservationDto.StartTime))
                return Conflict("Time slot overlaps with existing reservation");
            var res = new Reservation()
            {
                Date = createReservationDto.Date,
                EndTime = createReservationDto.EndTime,
                StartTime =  createReservationDto.StartTime,
                Status =  createReservationDto.Status,
                RoomId = createReservationDto.RoomId,
                Id = reservations.Count +1,
                topic =  createReservationDto.Topic,
            };
            reservations.Add(res);
            return Created();
        }

        [HttpPut("{id:int}")]
        
        
        public IActionResult Put([FromRoute] int id, [FromBody] UpdateReservationDto updateReservationDto)
        {
            var reservation = reservations.FirstOrDefault(x=> x.Id == id);
            if (reservation == null)
                return NotFound();
            
            
            if(updateReservationDto.StartTime <= reservation.EndTime)
                return BadRequest();
            reservation.topic = updateReservationDto.Topic;
            reservation.RoomId =  updateReservationDto.RoomId;
            reservation.StartTime = updateReservationDto.StartTime;
            reservation.EndTime = updateReservationDto.EndTime;
            reservation.Status = updateReservationDto.Status;
            return Ok();

        }

        [HttpDelete(("{id:int}"))]
        public IActionResult Delete([FromRoute] int id)
        {
            if (reservations.FirstOrDefault(x => x.Id == id) == null)
            {
                return NotFound();
            }
            reservations.Remove(reservations.First(x => x.Id == id));
            return NoContent();
        }




}
}