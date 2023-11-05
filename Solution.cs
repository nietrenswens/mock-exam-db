
class Solution {
    
    public static void q1Solution(HotelContext db)
    {
        // Exercise 1: Give the booking detail of given guest booking details (for GuestID 10).  
        // The result should include booking date, room number, and number of nights. 	
        var result = from b in db.bookings
                     where b.GuestID == 10
                     select new { b.BookingDate, b.RoomNumber, b.Nights };
        foreach(var r in result)
        {
            System.Console.WriteLine($"{r.BookingDate}, {r.RoomNumber}, {r.Nights}");
        }
    }

    public static void q2Solution(HotelContext db, DateOnly date)
    {
        // Exercise: 2:  List down all the guest names, and room number, 
        // having booking on specific date (2022 - 01 - 31) 	
        var results = from b in db.bookings
                      join g in db.guests on b.GuestID equals g.Id
                      where b.BookingDate == date
                      orderby g.Id
                      select new { g.Id, g.Name, b.RoomNumber };
        foreach(var r in results)
        {
            System.Console.WriteLine($"{r.Id}, {r.Name}, {r.RoomNumber}");
        }
    }

    public static void q3Solution(HotelContext db)
    {
        // Exercise 3: List down number of bookings per day where there are more than 1 bookings
        var bookings = from b in db.bookings
                       group b by b.BookingDate into grp
                       orderby grp.Key
                       select new { Date = grp.Key, Count = grp.ToList().Count() };
        var result = bookings.Where(_ => _.Count > 1);
        foreach(var booking in result)
        {
            System.Console.WriteLine($"{booking.Date}, {booking.Count}");
        }
    }

    public static void q4Solution(HotelContext db, DateOnly date)
    {
        // Exercise 4. List the rooms that are free on '2022-01-13'.
        var allRoomNumbers = db.rooms.Select(_ => _.Number);
        var occupiedRoomNumbers = db.bookings.Where(_ => _.BookingDate == date).Select(_ => _.RoomNumber);
        var result = allRoomNumbers.Except(occupiedRoomNumbers).OrderBy(_ => _);
        foreach(var r in result)
        {
            System.Console.WriteLine(r);
        }
    }

    public static void q5Solution(HotelContext db) 
    {
        // Exercise: 5:  List down top 5 valued customers, with their id and spending 
        // HINT: a valued customer is the on with max amount spent, 
        // amount = Nights * Price for each booking of a customer
        var guests = from b in db.bookings
                     join r in db.rooms on b.RoomNumber equals r.Number
                     join rt in db.roomType on r.RoomTypeId equals rt.Id
                     group b by b.GuestID into grp
                     orderby grp.Key
                     select new { Id = grp.Key, Value = grp.Sum(_ => _.room.roomType.Price * _.Nights) };
        var result = guests.OrderByDescending(_ => _.Value).Take(5).ToList();
        foreach(var r in result)
        {
            System.Console.WriteLine($"{r.Id}, {r.Value}");
        }
    }

    public static void q6Solution(HotelContext db, DateOnly date)
    {
     // Exercise 6: 

    }
   
    //     ****  Ex7 in Model.cs  **** 

    public static void q8aSolution(HotelContext db)
    {
     // Exercise 8a: 
    
    }

    public static void q8bSolution(HotelContext db)
    {
     // Exercise 8b: 

    }

}