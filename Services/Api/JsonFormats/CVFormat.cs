namespace Services.Api.JsonFormats
{

    public class Formation
    {
        public string? years { get; set; }
        public string? diplome { get; set; }
        public string? establishments { get; set; }
    }

    public class Competence
    {
        public string? name { get; set; }
        public string? description { get; set; }
    }

    public class ProfessionnalExperience
    {
        public string? date { get; set; }
        public string? entreprise { get; set; }
        public string? poste { get; set; }
        public string? description { get; set; }
    }

    public class CVFormat
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public int? Age { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public List<Formation>? Formations { get; set; }
        public List<Competence>? Competences { get; set; }
        public List<string>? hobbies { get; set; }
        public string? probable_profression { get; set; }
    }
}