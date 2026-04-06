using Back_end.Objects;

public class GameUserServiceData
{
    public static List<User> Empty = new List<User>();
    public static List<User> OneJob = new List<User>
    {
            new User(
            username: "John Doe",
            email: "john.doe@example.com",
            about: "Software Developer",
            firstName: "John",
            lastName: "Doe",
            password: "password",
            userId: 1,
            experiences: []
        ),
    };

    public static List<User> UserList = new List<User>
    {
        new User(
            username: "John Doe",
            email: "john.doe@example.com",
            about: "Software Developer",
            firstName: "John",
            lastName: "Doe",
            password: "password",
            userId: 1,
            experiences: []
        ),
        new User(
            username: "Jane Smith",
            email: "jane.smith@example.com",
            about: "Software Engineer",
            firstName: "Jane",
            lastName: "Smith",
            password: "password",
            userId: 2,
            experiences: []
        ),
        new User(
            username: "Bob Johnson",
            email: "bob.johnson@example.com",
            about: "Product Manager",
            firstName: "Bob",
            lastName: "Johnson",
            password: "password",
            userId: 3,
            experiences: []
        ),
    };
}