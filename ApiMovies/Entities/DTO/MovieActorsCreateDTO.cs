using System.ComponentModel.DataAnnotations;

namespace ApiMovies.Entities.DTO
{
    public class MovieActorsCreateDTO
    {
        [StringLength(maximumLength: 75)]
        public string CharacterName { get; set; }
        public int OrderInCharactersList { get; set; }
    }
}
