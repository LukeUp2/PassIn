namespace PassIn.Infrastructure.Entitties
{
    public class Event
    {
        //Estamos criando esse id na entidade pois o SQLite não da suporte para esse tipo de id
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int Maximum_Attendees { get; set; }
    }
}
