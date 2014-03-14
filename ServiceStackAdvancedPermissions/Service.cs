using System.Linq;
using ServiceStack;

namespace App
{
	[RequirePermission(ObjectType.Document)]
	[Route("/Documents/{Id}", "GET")]
	public class DocumentRequest : IReturn<string>
	{
		[ObjectId]
		public int Id { get; set; }
	}

	[Authenticate]
	public class DocumentService : Service
	{
		public string Get(DocumentRequest request)
		{
			// We have permission to access this document
			var document = App.Db.Documents.FirstOrDefault(d=>d.Id == request.Id);
			if(document == null)
				throw HttpError.NotFound("No such document");
			return document.Name; 
		}
	}
}