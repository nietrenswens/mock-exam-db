
class Program {

    public static void Main(string[] args)
    {
        // BulkDataSeeding();    
     
        using (var context = new HotelContext())
        {         
            Console.WriteLine("\n Q1 Solution: \n");
            Solution.q1Solution(context);
            Console.WriteLine("\n Q2 Solution: \n");
            Solution.q2Solution(context, new DateOnly(2022,01,31));
            Console.WriteLine("\n Q3 Solution: \n");
            Solution.q3Solution(context);
            Console.WriteLine("\n Q4 Solution: \n");
            Solution.q4Solution(context, new DateOnly(2022,01,31));
            Console.WriteLine("\n Q5 Solution: \n");
            Solution.q5Solution(context);
            Console.WriteLine("\n Q6 Solution: \n");
            Solution.q6Solution(context, new DateOnly(2022,01,13));

            Console.WriteLine("\n Q8 Solution: \n");
            Solution.q8aSolution(context);
            Solution.q8bSolution(context);
        }
    }
        
    static void BulkDataSeeding(){
        using (var dbContext = new HotelContext()) {
            if (dbContext == null)
            {
                Console.WriteLine("No DbContext avaialable");
                return;
            }                     
            
            dbContext.roomType.AddRange(SeedData.SeedRoomType());
            dbContext.rooms.AddRange(SeedData.SeedRoom());
            dbContext.guests.AddRange(SeedData.SeedGuest());
            dbContext.bookings.AddRange(SeedData.SeedBookings());
            Console.WriteLine("{0} Records Created.", dbContext.SaveChanges());
        };
    }
}