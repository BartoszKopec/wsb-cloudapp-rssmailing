using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseManager.Models
{
	public class Record<TId>
	{
		[BsonId, BsonRepresentation(BsonType.ObjectId)]
		public TId Id { get; set; }
		public string AddressEmail { get; set; }
		public List<string> RssSources { get; set; } = new List<string>();
	}
}
