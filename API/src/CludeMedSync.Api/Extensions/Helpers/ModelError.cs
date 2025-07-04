namespace CludeMedSync.Api.Extensions.Helpers
{
	public class ModelError
	{
		public string type { get; set; } 
		public string title { get; set; }
		public int status { get; set; }
		public string traceId { get; set; }
	}

}
