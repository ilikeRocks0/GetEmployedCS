using Back_end.Endpoints.Models;
using Back_end.Persistence.Objects;

public class JobAddServiceData
{
    public static User jobSeeker = new(101, "email@gmail.com", "user", "pass", "I am cool guy!", "Test", "User", []);
    public static User employer = new(102, "employer@gmail.com", "testEmployer", "testPassword", "This is a test employer", "TestEmployer");
    public static NewJob baseJob = new(
        "Software Developer",
        "2026-12-31", 
        "www.test.org", 
        false, 
        true, 
        "Full stack", 
        "Co-op", 
        ["Winnipeg, MB, Canada"], 
        ["Java"],
        "As a Software Development Intern, you will work closely with the team to support the development and modernization of our backend systems.", 
        false);

    public static NewJob badDateFormatJob = new(
        "Software Developer",
        "31/12/2026", 
        "www.test.org", 
        false, 
        true, 
        "Full stack", 
        "Co-op", 
        ["Winnipeg, MB, Canada"], 
        ["Java"],
        "As a Software Development Intern, you will work closely with the team to support the development and modernization of our backend systems.", 
        false);

    public static NewJob badLinkFormatJob = new(
        "Software Developer",
        "2026-12-31", 
        "this is not a link", 
        false, 
        true, 
        "Full stack", 
        "Co-op", 
        ["Winnipeg, MB, Canada"], 
        ["Java"],
        "As a Software Development Intern, you will work closely with the team to support the development and modernization of our backend systems.", 
        false);
}

