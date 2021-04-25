using DatabaseManager.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseManager
{
	public class DatabaseMongo : IDatabase<string>
	{
		private readonly IMongoCollection<Record<string>> _collection;

		public DatabaseMongo(DbSettings settings)
		{
			MongoClient client = new(settings.ConnectionString);
			IMongoDatabase database = client.GetDatabase(settings.DatabaseName);
			BsonClassMap.RegisterClassMap<Record<string>>((cm) =>
			{
				cm.AutoMap();
				cm.MapIdMember((c) => c.Id);
			});
			_collection = database.GetCollection<Record<string>>(settings.CollectionName);
		}

		public async Task<Record<string>> GetAsyncBy(Func<Record<string>, bool> predicate, CancellationToken token)
		{
			IAsyncCursor<Record<string>> cursor = await _collection.FindAsync(new BsonDocument(), cancellationToken: token);
			if(cursor is null)
			{
				return null;
			}
			List<Record<string>> records = await cursor.ToListAsync(token);
			Record<string> dbRecord = records.Where(predicate).FirstOrDefault();
			if (dbRecord is null)
			{
				return null;
			}
			return new Record<string>
			{
				Id = dbRecord.Id,
				AddressEmail = dbRecord.AddressEmail,
				RssSources = dbRecord.RssSources
			};
		}
		
		public Task<Record<string>> GetAsync(string id, CancellationToken token)
		{
			return GetAsyncBy((r) => r.Id == id, token);
		}

		public async Task<bool> AddAsync(Record<string> record, CancellationToken token)
		{
			if(await GetAsyncBy((r)=> r.Id == record.Id || r.AddressEmail == record.AddressEmail, token) != null)
			{
				return false;
			}

			await _collection.InsertOneAsync(
				document: new Record<string>
				{
					AddressEmail = record.AddressEmail,
					RssSources = record.RssSources
				}, 
				options: new InsertOneOptions
				{
					BypassDocumentValidation = true
				}, 
				token);
			return true;
		}

		public async Task<bool> DeleteAsync(string id, CancellationToken token)
		{
			if (await GetAsync(id, token) == null)
			{
				return false;
			}

			DeleteResult result = await _collection.DeleteOneAsync((r) => r.Id == id, token);
			if (!result.IsAcknowledged) return false;
			return result.DeletedCount == 1;
		}

		public async Task<bool> UpdateAsync(Record<string> record, CancellationToken token)
		{
			if (await GetAsync(record.Id, token) == null)
			{
				return false;
			}

			UpdateDefinition<Record<string>> update = Builders<Record<string>>.Update.Set(
				nameof(Record<string>.RssSources), record.RssSources);

			UpdateResult result = await _collection.UpdateOneAsync((r) => r.Id == record.Id, update, cancellationToken: token);
			if (!result.IsAcknowledged) return false;
			return result.ModifiedCount == 1;
		}
	}
}
