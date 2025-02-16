In EF Core 8, whether you need the [Required] annotation depends on the nullability of your property type:
✅ When Columns Are Automatically Required

If your property is non-nullable, EF Core automatically makes the column required in the database. For example:

public class MyEntity
{
public int Id { get; set; }

    public string Name { get; set; } // Implicitly required (non-nullable string)
}

    Since string Name is not nullable, EF Core treats it as NOT NULL in the database.

⚠️ When You Need [Required]

You only need to explicitly use [Required] if your property is nullable but should be required. For example:

public class MyEntity
{
public int Id { get; set; }

    public string? Name { get; set; } // Nullable by default

    [Required] 
    public string? Description { get; set; } // Nullable in C#, but required in DB
}

    Name is nullable (string?), so EF allows it to be NULL in the database.
    [Required] on Description forces it to be NOT NULL in the database.