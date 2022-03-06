namespace ApiMovies.Entities.DTO
{
     /* Task MakeAdmin(string id) didn't work
        The JSON value could not be converted to System.String
        Few solutions from the internet didn't work, with this DTO it works. */
    public class AdminDTO
    {
        public string UserId { get; set; }
    }
}
