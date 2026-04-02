using Back_end.Objects;

namespace Back_end.Endpoints.Models;

public class UpdateExperienceRequest
{
    public Experience OldExperience { get; set; } = null!;
    public Experience NewExperience { get; set; } = null!;
}
