using DatabaseManager;
using DatabaseManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Units
{
	public class DatabaseStringIdMock : IDatabase<string>
	{
		public List<Record<string>> Records = new();

		public async Task<bool> AddAsync(Record<string> record, CancellationToken token)
		{
			if (await GetAsyncBy((r) => r.Id == record.Id || r.AddressEmail == record.AddressEmail, token) != null)
			{
				return false;
			}
			record.Id = Guid.NewGuid().ToString();

			lock (Records)
			{
				Records.Add(new Record<string>
				{
					Id = record.Id,
					AddressEmail = record.AddressEmail,
					RssSources = record.RssSources
				});
			}
			return true;
		}

		public async Task<bool> DeleteAsync(string id, CancellationToken token)
		{
			Record<string> record = await GetAsync(id, token);
			if (record != null)
			{
				return false;
			}

			lock (Records)
			{
				Records.Remove(record);
			}
			return true;
		}

		public Task<Record<string>> GetAsync(string id, CancellationToken token)
		{
			return GetAsyncBy((r) => r.Id == id, token);
		}

		public async Task<Record<string>> GetAsyncBy(Func<Record<string>, bool> predicate, CancellationToken token)
		{
			await Task.Delay(50, token);
			Record<string> record = Records.Find((r)=>predicate(r));
			return record;
			//Task<Record<string>> task = new(() =>
			//{
			//	Record<string> record = Records.Find(predicate);
			//	return record;
			//}, token);
			//return task;
		}

		public async Task<bool> UpdateAsync(Record<string> record, CancellationToken token)
		{
			Record<string> dbrecord = await GetAsyncBy((r)=>r.Id == record.Id && r.AddressEmail == record.AddressEmail, token);
			if (record != null)
			{
				return false;
			}

			lock (Records)
			{
				lock (dbrecord)
				{
					dbrecord.RssSources = record.RssSources;
				}
			}
			return true;
		}
	}
}
