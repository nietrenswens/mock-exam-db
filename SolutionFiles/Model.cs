
class HotelContext : DbContext {

    public DbSet<RoomType> roomType { get; set; } = null!;
    public DbSet<Room> rooms { get; set; } = null!;
    public DbSet<Guest> guests { get; set; } = null!;
    public DbSet<Booking> bookings { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseNpgsql("User ID=postgres;Password=;Host=localhost;port=5432;Database=HotelExam;Pooling=true");
        //optionsBuilder.LogTo(System.Console.WriteLine, LogLevel.Critical);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) { 
        //Solution 7a (part)
        /* 
        //Optional collection navigation property "Subordinates"
         modelBuilder.Entity<Employee>()
            .HasOne<Employee>(_=>_.Boss)
            .WithMany(_ => _.Subordinates)
            .HasForeignKey(_=>_.BossID)
            .OnDelete(DeleteBehavior.Cascade); 
        */
         modelBuilder.Entity<Employee>()
            .HasOne(_=>_.Boss)
            .WithMany()
            .HasForeignKey(_=>_.BossID)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);     

        //7b
         modelBuilder.Entity<Booking>()
            .HasOne(e => e.Employee)
            .WithMany(e => e.Bookings)
            .HasForeignKey(e => e.EmployeeID)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);      
    }

    //Solution 7a (part)
    public DbSet<Employee> employees { get; set; } = null!;
}

class RoomType
{
    public int Id { get; set; }
    public string Type { get; set; } = null!;
    public int Occupancy { get; set; }
    public decimal Price { get; set; }

    //Collection navigation properties
    public virtual List<Room>? Rooms { get; set; }

}

class Room
{
    [Key]
    public int Number { get; set; }
    public int Floor { get; set; }

    //Reference navigation property
    [ForeignKey("RoomTypeId")]
    public RoomType roomType { get; set; } = null!;
    public int RoomTypeId { get; set; }

    //Collection navigation properties
    public virtual List<Booking>? Bookings { get; set; }
}

class Guest { 
    public int Id { get; set; } 
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Country { get; set; } 

    //Collection navigation properties
    public virtual List<Booking>? Bookings { get; set; }
}

class Booking { 
    public int Id { get; set; }
    [Required]
    public DateOnly BookingDate { get; set; }
    [Required]
    public int Nights { get; set; }

    //Reference navigation property
    public Guest guest { get; set; } = null!;
    public int GuestID { get; set; }

    //Reference navigation property
    [ForeignKey("RoomNumber")]
    public Room room { get; set; } = null!;
    public int RoomNumber { get; set; }

    //ToDo 7b: Modify booking table, add EmployeeID as foreign key 

    //Solution 7b:
    public Employee? Employee { get; set; } = null!;
    public int? EmployeeID { get; set; } 
}

//ToDo 7a: Create a new table named Employee

//Solution 7a
class Employee { 
    public int ID { get; set; }
    [Required, DataType("varchar(50)")]
    public string Name { get; set; } = null!;
    
    public int? BossID { get; set; }

    public Employee? Boss { get; set; } = null!;
    
    //Collection navigation properties:
    public List<Booking>? Bookings { get; set; } = null!;
    
    //optional
    public List<Employee>? Subordinates { get; set; } = null!;

    public Employee(int iD, string name) {
        ID = iD;
        Name = name;
    }
    public Employee(int id, string name, int bossID) 
        : this(id, name) => BossID = bossID;
}