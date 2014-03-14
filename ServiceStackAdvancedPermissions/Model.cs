using System.Collections.Generic;

namespace App
{
	// Define the object types here
	public static class ObjectType
	{
		public const int Document = 1;
	}

	public static class Model
	{
		public class Permission
		{
			public int Id { get; set; }
			public int ObjectType { get; set; }
			public int? ObjectId { get; set; }
			public int? UserId { get; set; }
			public int? GroupId { get; set; }
			public bool Permitted { get; set; }
		}

		public class User
		{
			public int Id { get; set; }
			public string Username { get; set; }
			public string Password { get; set; }
		}

		public class Group
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}

		public class GroupMember
		{
			public int Id { get; set; }
			public int GroupId { get; set; }
			public int UserId { get; set; }
		}

		public class Document
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
	}

	// A simple data source to mock a database
	public static class Db
	{
		// Users
		public static List<Model.User> Users = new List<Model.User>() {
			new Model.User { Id = 1, Username = "jack.bauer", Password = "ct-you" },
			new Model.User { Id = 2, Username = "sheldon.cooper", Password = "baz1nga" },
			new Model.User { Id = 3, Username = "gregory.house", Password = "vicod1n" },
			new Model.User { Id = 4, Username = "dexter.morgan", Password = "slice0flife" },
			new Model.User { Id = 5, Username = "leroy.gibbs", Password = "iloveNC1S" },
		};

		// Groups
		public static List<Model.Group> Groups = new List<Model.Group>() {
			new Model.Group { Id = 1, Name = "Administrators" },
			new Model.Group { Id = 2, Name = "Executives" },
			new Model.Group { Id = 3, Name = "Managers" },
			new Model.Group { Id = 4, Name = "ClericalStaff" },
			new Model.Group { Id = 5, Name = "Contractors" },
		};

		// Group Memberships
		public static List<Model.GroupMember> GroupMembers = new List<Model.GroupMember>() {
			new Model.GroupMember { Id = 1, GroupId = 1, UserId = 1 }, // Jack Bauer - Administrator
			new Model.GroupMember { Id = 2, GroupId = 1, UserId = 5 }, // Leroy Gibbs - Administrator
			new Model.GroupMember { Id = 3, GroupId = 2, UserId = 3 }, // Gregory House - Executive
			new Model.GroupMember { Id = 4, GroupId = 3, UserId = 2 }, // Sheldon Cooper - Manager
			new Model.GroupMember { Id = 5, GroupId = 4, UserId = 4 }, // Dexter Morgan - Clerical Staff
			new Model.GroupMember { Id = 6, GroupId = 5, UserId = 3 }, // Gregory House - Contractor
			new Model.GroupMember { Id = 7, GroupId = 5, UserId = 2 }, // Sheldon Cooper - Contractor
		};

		// Documents
		public static List<Model.Document> Documents = new List<Model.Document>() {
			new Model.Document { Id = 1, Name = "Secret Terrorist Plans" },
			new Model.Document { Id = 2, Name = "Where I hid the bodies" },
			new Model.Document { Id = 3, Name = "Naval command tactics" },
			new Model.Document { Id = 4, Name = "Patient History" },
			new Model.Document { Id = 5, Name = "ServiceStack Documentation" },
		};

		// Permissions
		public static List<Model.Permission> Permissions = new List<Model.Permission>() {
			new Model.Permission { Id = 1, ObjectType = ObjectType.Document, ObjectId = 5, UserId = null, 	GroupId = null,	Permitted = true }, // Everybody permitted to access - ServiceStack Documentation
			new Model.Permission { Id = 2, ObjectType = ObjectType.Document, ObjectId = 1, UserId = 1, 		GroupId = null,	Permitted = true }, // Jack Bauer permitted to access - Secret Terrorist Plans
			new Model.Permission { Id = 3, ObjectType = ObjectType.Document, ObjectId = 2, UserId = null, 	GroupId = 4, 	Permitted = true }, // Clerical Staff group members permitted to access - Where I hid the bodies
			new Model.Permission { Id = 4, ObjectType = ObjectType.Document, ObjectId = 4, UserId = 3, 		GroupId = null,	Permitted = true }, // Gregory House permitted to access - Patient History
			new Model.Permission { Id = 5, ObjectType = ObjectType.Document, ObjectId = 3, UserId = null, 	GroupId = 5, 	Permitted = true }, // Contractor group members permitted to access - Naval command tactics
			new Model.Permission { Id = 6, ObjectType = ObjectType.Document, ObjectId = 3, UserId = 2, 		GroupId = null,	Permitted = false }, // Sheldon Cooper not permitted to access - Naval command tactics (Explicit deny)
		};
	}
}

