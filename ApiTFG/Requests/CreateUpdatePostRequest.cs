namespace ApiTFG.Requests
{
    public class CreateUpdatePostRequest
    {
        public string Text { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
