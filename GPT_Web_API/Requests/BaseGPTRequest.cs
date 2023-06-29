namespace GPT_Web_API.Requests
{
	public class BaseGPTRequest
	{
		public string? model { get; set; }
		public string? prompt { get; set; }
		public long max_tokens { get; set; }
		public double temperature { get; set; }
	}
}
