using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Application.Data
{
	public class Record<TId>
	{
		[BsonId, BsonRepresentation(BsonType.ObjectId)]
		public TId Id { get; set; }
		public string AddressEmail { get; set; }
		public List<string> RssSources { get; set; } = new List<string>();
	}
}
