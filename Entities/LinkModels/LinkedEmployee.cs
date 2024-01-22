namespace Entities.LinkModels
{
    public record LinkedEmployee
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public int Age { get; init; }
        public string? Position { get; init; }
        public List<Link>? Links { get; init; }
    }
}
