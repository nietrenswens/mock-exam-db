
class Solution {
    
    public static void q1Solution(HotelContext db)
    {
        // Exercise 1: Give the booking detail of given guest booking details (for GuestID 10).  
        // The result should include booking date, room number, and number of nights. 	
        /*

            SELECT BookingDate, RoomNumber, Nights 
            FROM Bookings
            WHERE GuestID= 10;
         */

        //LINQ Method
        var guestBookings = db.bookings.Where(x => x.GuestID == 10).ToList();

        //LINQ expression
        //var guestBookings = from booking in db.bookings
        //                    where booking.GuestID == 10
        //                    select booking;

        //Output
        //guestBookings.ForEach(row => Console.WriteLine($"{row.BookingDate}, {row.RoomNumber}, {row.Nights}"));
        //OR
        foreach (var row in guestBookings)
            Console.WriteLine($"{row.BookingDate}, {row.RoomNumber}, {row.Nights}");

    }

    public static void q2Solution(HotelContext db, DateOnly date)
    {
        //Exercise: 2:  List down all the guest names, and room number, having booking on specific date(2022 - 01 - 31) 	
        /* 
         * 
         SELECT g.Name, r.Number
         FROM Bookings b JOIN Guests g on b.GuestID=g.ID
         WHERE b.BookingDate = '2022-01-31'
         
         ##### *Question 2*: Ordered according ascending guest ID
         */

        var guestRoom = from booking in db.bookings
                        join guest in db.guests
                        on booking.GuestID equals guest.Id
                        where booking.BookingDate == date
                        select new { booking, guest };
        foreach (var g in guestRoom.OrderBy(_ => _.guest.Id))
        {
            Console.WriteLine($"{g.guest.Id}, {g.guest.Name}, {g.booking.RoomNumber}");
        }

    }

    public static void q3Solution(HotelContext db)
    {
        //Exercise 3: List down number of bookings per day where there are more than 1 bookings
        /*
         Select BookingDate, count(*), array_agg(roomnumber)
         from bookings
         group by 1
         having count(*)>1
         */
        var query3 = from b in db.bookings
                     group b by b.BookingDate into bdg
                     where bdg.Count() > 1
                     select new { K = bdg.Key, 
                                  Values = (from b in bdg 
                                            select new {
                                                b.RoomNumber,
                                                b.GuestID,
                                                b.Nights,
                                                b.BookingDate
                                            }
                                          ),
                                  C = bdg.Count() };
        //output
        foreach (var x in query3) {
            Console.WriteLine($"{x.K}, {x.C}");
    /*      System.Console.Write("RoomNumber:");
            foreach(var b in x.Values) 
               System.Console.Write($" {b.RoomNumber}, ");
            System.Console.WriteLine("");  */  
        }

/*         var q3Complex = from b in db.bookings
                        group b by b.BookingDate into BookingGroup
                        where BookingGroup.Count() > 1
                        select new
                        {
                            K = BookingGroup.Key,
                            c = BookingGroup.Count(),
                            agg = string.Join(',', (from r in BookingGroup select r.RoomNumber).ToList())
                        };
        //output
        foreach (var x in q3Complex)
            Console.WriteLine($"{x.K}, {x.c}, {x.agg}"); */
    }

    public static void q4Solution(HotelContext db, DateOnly date)
    {
        //Exercise 4. List the rooms that are free on '2022-01-31'.
        /*
         SELECT Number FROM rooms  WHERE Number not in
         	(SELECT roomnumber FROM bookings WHERE bookingdate='2022-01-31')
         */
        
        //Console.WriteLine("Using subquery");
        var subq4 = from room in db.rooms
                     where (from b in db.bookings
                            where b.BookingDate == date
                            select b.RoomNumber).Contains(room.Number) == false
                     select room.Number;
        //output
        foreach (var r in subq4.OrderBy(_ => _))
            Console.WriteLine(r);

        //------Other ways--------

        //Set Operations (multiple queries):
        /*
        var allRoomNumbers = from room in db.rooms
                             select room.Number;

        var bookedRoomNumbersAtGivenDate = from booking in db.bookings
                                           where booking.BookingDate == date
                                           select booking.RoomNumber;
                                                               
        var freeRoomNumbersAtGivenDate = allRoomNumbers.Except(bookedRoomNumbersAtGivenDate);
        freeRoomNumbersAtGivenDate
                    .OrderBy(r => r)
                    .ToList()
                    .ForEach(r => Console.WriteLine(r));
        */

        /*
        //Console.WriteLine("Set Operations using Except");
        var Setq4 = db.rooms.Select(r => r.Number).ToList()
            .Except(db.bookings.Where(b => b.BookingDate == date).Select(_ => _.RoomNumber).ToList());
        
        Setq4 = (from room in db.rooms select room.Number)
                .Except(from booking in db.bookings 
                        where booking.BookingDate == date    
                        select booking.RoomNumber)      
                .OrderBy(_ => _)        
                ;
        
        //output
        foreach (var y in Setq4)
            Console.WriteLine(y);
        */
    }

    public static void q5Solution(HotelContext db) {
        //Exercise: 5:  List down top 5 valued customers, with their id and spending 
        //HINT: a valued customer is the on with max amount spent, amount = Nights * Price for each booking of a customer

        /*
         Select GuestID,  sum(nights*price), array_agg(nights||'*'||Price)
         FROM Bookings b JOIN Rooms r on b.roomnumber=r.Number
		 JOIN RoomType rt on r.RoomTypeID = rt.ID
         group by 1
         order by 2 desc
         limit 5
         */

        var q5Sub = (from b in db.bookings
                    join r in db.rooms on b.RoomNumber equals r.Number
                    join rt in db.roomType on r.RoomTypeId equals rt.Id
                    group b by b.GuestID into bookingroomgroup
                    select new
                    {
                        GuestID = bookingroomgroup.Key,
                        Spending = bookingroomgroup.Sum(_ => _.Nights * _.room.roomType.Price),
                        Values = bookingroomgroup.ToList(),            
                                    //(from bk in bookingroomgroup
                                    //select bk.Nights * bk.room.roomType.Price)
                    })
                    .ToList()
                    ;
        
        var q5 = q5Sub.OrderByDescending(_ => _.Spending).Take(5).ToList();
        q5.ForEach(_=>Console.WriteLine($"{_.GuestID}, {_.Spending}"));
/*
        System.Console.WriteLine("\n ----Other way:------\n");
        var query = (
                     from booking in db.bookings
                     join room in db.rooms
                        on booking.RoomNumber equals room.Number
                     join roomType in db.roomType
                        on room.RoomTypeId equals roomType.Id
                     group new {booking, room, roomType} by booking.GuestID into bookingData
                     select new {
                                  GuestID = bookingData.Key,
                                  Totalexpense = (from b in bookingData
                                                 select b.booking.Nights * b.roomType.Price)
                                                 .Sum(_ => _)                                                              
                         })
                        .OrderByDescending(_ => _.Totalexpense) 
                        .Take(5)                    
                        .ToList()
                        ;   
        query.ForEach(_=>Console.WriteLine($"{_.GuestID}, {_.Totalexpense}"));                          
 */       
        
    }

    public static void q6Solution(HotelContext db, DateOnly date)
    {
        //Exercise 6: 

        //Query with Left Join:
        var All = (
                    from rt in db.roomType
                    join room in db.rooms
                    on rt.Id equals room.RoomTypeId into rtrooms
                    from rtWithMayBeRooms in rtrooms.DefaultIfEmpty()
                    group new {Room = rtWithMayBeRooms, RoomType = rt } by rt.Type into roomsTypes
                    select new {
                                 RoomType = roomsTypes.Key,
                                 RoomNumbers = roomsTypes.Select(_ => _.Room == null ? 0 : _.Room.Number).Where(_ => _ != 0).ToList(), 
                                 Total = roomsTypes.Select(_ => _.Room == null ? 0 : _.Room.Number).Where(_ => _ != 0).Count(),                          
                               }        
              )
              .OrderByDescending(_ => _.Total)         
              .ToList()
              ;

        var booked = db.bookings.Where(_ => _.BookingDate == date).Select(_=>_.RoomNumber).ToList();
        
        /*
        //Overview:
        Console.WriteLine("Booking Status");
        foreach (var x in All)
        {
            Console.WriteLine($"+{x.RoomType}");
            if (x.RoomNumbers is not null)
            {
                foreach (var r in x.RoomNumbers)

                    Console.WriteLine($"\t--- {r} {(booked.Contains(r)?"Booked":"")}");
            }
        }

        Console.WriteLine("Empty");
        foreach (var x in All)
        {
            Console.WriteLine($"+{x.RoomType}");
            if (x.RoomNumbers is not null)
            {
                foreach (var r in x.RoomNumbers.Except(booked))
                    Console.WriteLine($"\t--- {r}");
            }
        }

        Console.WriteLine("Booked");
        foreach (var x in All)
        {
            Console.WriteLine($"+{x.RoomType}");
            if (x.RoomNumbers is not null)
            {
                foreach (var r in x.RoomNumbers.Intersect(booked))
                    Console.WriteLine($"\t--- {r}");
            }
        }
        */
        
        Console.WriteLine("Summary:");
        //Console.WriteLine("RoomType,  Total, Booked, Free");
        foreach (var r in All)
        {
            Console.WriteLine($"RoomType: {r.RoomType}, Total: {r.Total}, Booked: {r.RoomNumbers?.Intersect(booked).Count()}, Free:{r.RoomNumbers?.Except(booked).Count()}");
        }

        /*
        //Other way: Query with Inner Join instead of Left Join
        
        var availableRoomTypes = (
            from rt in db.roomType
            join room in db.rooms
             on rt.Id equals room.RoomTypeId 
            group new {Room = room, RoomType = rt } by rt.Type into roomsTypes
            select new {
                        RoomType = roomsTypes.Key,
                        RoomNumbers = roomsTypes.Select(_ => _.Room.Number).ToList(), 
                        Total = roomsTypes.Select(_ => _.Room.Number).Count(),                          
                    }        
            )
            .OrderByDescending(_ => _.Total)         
            .ToList()
            ; 

        var allRoomTypes = (
            from rt in db.roomType
            group new {RoomType = rt } by rt.Type into roomsTypes
            select new {
                        RoomType = roomsTypes.Key,
                        RoomNumbers = new List<int>(), //set to empty List<int> for all roomtypes
                        Total = 0,  //set to 0 for all roomtypes                       
                       }        
            )
            .ToList()
            ; 

        var notAvailableRoomTypes = allRoomTypes
                                    .Where(rt => availableRoomTypes
                                                .All(_ => _.RoomType != rt.RoomType))
                                    ;
  
        var all = availableRoomTypes.Union(notAvailableRoomTypes);

        var booked = db.bookings.Where(_ => _.BookingDate == date).Select(_=>_.RoomNumber).ToList();

        Console.WriteLine("Summary:");
        foreach (var r in all)
        {
            Console.WriteLine($"RoomType: {r.RoomType}, Total: {r.Total}, Booked: {r.RoomNumbers?.Intersect(booked).Count()}, Free:{r.RoomNumbers?.Except(booked).Count()}");
        }
    
        */

    }
   
    //     ****  Ex7 in Model.cs  **** 

    public static void q8aSolution(HotelContext db)
    {
        //Exercise 8a: 
        var AllEmps = new List<Employee>() {new Employee (1,"Alice"),
        new Employee (2,"Bob",1), new Employee (3,"Claudia",1),
        new Employee (4,"Diana",2), new Employee (5,"Faris")};
        
        try{ 
            db.AddRange(AllEmps);
            System.Console.WriteLine($"Records changed: {db.SaveChanges()}");
            System.Console.WriteLine("\n---Employees added:---\n");
           db.employees.Take(1000).ToList().ForEach(e => System.Console.WriteLine($"{e.ID}, {e.Name}, {e.BossID}"));
        }
        catch(Exception ex) {
            System.Console.WriteLine(ex);
        }
          
    }

    public static void q8bSolution(HotelContext db)
    {
        //Exercise 8b: 

        var bob = db.employees.FirstOrDefault(x => x.Name == "Bob");
        if (bob != null)
        {
            try{
                db.employees.Remove(bob);
                System.Console.WriteLine($"Removal, Records changed: {db.SaveChanges()}");
            }
            catch(Exception ex) {
                System.Console.WriteLine(ex);
            }
            finally {
                System.Console.WriteLine("\n---After removal---\n");
                db.employees.Take(1000).ToList().ForEach(e => System.Console.WriteLine($"{e.ID}, {e.Name}, {e.BossID}"));
            }

        }

    }

}