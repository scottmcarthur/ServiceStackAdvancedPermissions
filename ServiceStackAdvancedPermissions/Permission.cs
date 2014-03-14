using System;
using System.Linq;
using ServiceStack;
using ServiceStack.Web;

namespace App
{
	public class RequirePermissionAttribute : Attribute, IHasRequestFilter
	{
		readonly int objectType;

		public RequirePermissionAttribute(int objectType)
		{
			// Set the object type
			this.objectType = objectType;
		}

		IHasRequestFilter IHasRequestFilter.Copy()
		{
			return this;
		}

		public void RequestFilter(IRequest req, IResponse res, object requestDto)
		{
			// Get the active user's session
			var session = req.GetSession() as MyServiceUserSession;
			if(session == null || session.UserAuthId == 0)
				throw HttpError.Unauthorized("You do not have a valid session");
				
			// Determine the Id of the requested object, if applicable
			int? objectId = null;
			var property = requestDto.GetType().GetPublicProperties().FirstOrDefault(p=>Attribute.IsDefined(p, typeof(ObjectIdAttribute)));
			if(property != null)
				objectId = property.GetValue(requestDto,null) as int?;

			// You will want to use your database here instead to the Mock database I'm using
			// So resolve it from the container
			// var db = HostContext.TryResolve<IDbConnectionFactory>().OpenDbConnection());
			// You will need to write the equivalent 'hasPermission' query with your provider

			// Get the most appropriate permission
			// The orderby clause ensures that priority is given to object specific permissions first, belonging to the user, then to groups having the permission
			// descending selects int value over null
			var hasPermission = session.IsAdministrator || 
								(from p in Db.Permissions
							     where p.ObjectType == objectType && ((p.ObjectId == objectId || p.ObjectId == null) && (p.UserId == session.UserAuthId || p.UserId == null) && (session.Groups.Contains(p.GroupId) || p.GroupId == null))
								 orderby p.ObjectId descending, p.UserId descending, p.Permitted, p.GroupId descending
							     select p.Permitted).FirstOrDefault();

			if(!hasPermission)
				throw new HttpError(System.Net.HttpStatusCode.Forbidden, "Forbidden", "You do not have permission to access the requested object");
		}

		public int Priority { get { return int.MinValue; } }
	}

	// Attribute for defining the ObjectId on the DTO
	public class ObjectIdAttribute : Attribute { }
}