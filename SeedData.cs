class SeedData
{
    public static List<RoomType> SeedRoomType()
    {
        return File.ReadAllLines("./SeedData/roomType.csv")
                   .Skip(1) //Header
                   .Where(_ => _.Length > 0)
                   .Select(_ => ParseRoomType(_)).ToList();
    }

    private static RoomType ParseRoomType(string row)
    {
        var columns = row.Split(',');
        return new RoomType()
        {
            Id = int.Parse(columns[0]),
            Type = columns[1],
            Occupancy = int.Parse(columns[2]),
            Price = Decimal.Parse(columns[3])
        };
    }

    public static List<Room> SeedRoom()
    {
        return File.ReadAllLines("./SeedData/rooms.csv")
                   .Skip(1) //Header
                   .Where(_ => _.Length > 0)
                   .Select(_ => ParseRoom(_)).ToList();
    }

    private static Room ParseRoom(string row)
    {
        var columns = row.Split(',');
        return new Room()
        {
            Number = int.Parse(columns[0]),
            Floor = int.Parse(columns[1]),
            RoomTypeId = int.Parse(columns[2])
        };
    }

    public static List<Guest> SeedGuest()
    {
        return File.ReadAllLines("./SeedData/guests.csv")
                   .Skip(1) //Header
                   .Where(_ => _.Length > 0)
                   .Select(_ => ParseGuest(_)).ToList();
    }

    private static Guest ParseGuest(string row)
    {
        var columns = row.Split(',');
        return new Guest()
        {
            Id = int.Parse(columns[0]),
            Name = columns[1],
            Phone = columns[2],
            Email = columns[3],
            Country = columns[4]
        };
    }

    public static List<Booking> SeedBookings()
    {
        return File.ReadAllLines("./SeedData/bookings.csv")
                .Skip(1) //Header
                .Where(_ => _.Length > 0)
                .Select(_ => ParseBooking(_)).ToList();        
    }

    private static Booking ParseBooking(string row)
    {
        var columns = row.Split(',');
        return new Booking()
        {
            Id = int.Parse(columns[0]),
            BookingDate = DateOnly.Parse(columns[1]),
            Nights = int.Parse(columns[2]),
            GuestID = int.Parse(columns[3]),
            RoomNumber = int.Parse(columns[4])
        };
    }
}