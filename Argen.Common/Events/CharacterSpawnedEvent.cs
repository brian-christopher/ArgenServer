namespace Argen.Common.Events
{
    public class CharacterSpawnedEvent
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
    }
}