using DatabaseManager.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseManager
{
	public interface IDatabase<TId>
	{
		Task<bool> AddAsync(Record<TId> record, CancellationToken token);
		Task<Record<TId>> GetAsync(TId id, CancellationToken token);
		Task<Record<TId>> GetAsyncBy(Func<Record<string>, bool> predicate, CancellationToken token);
		Task<bool> UpdateAsync(Record<TId> record, CancellationToken token);
		Task<bool> DeleteAsync(TId id, CancellationToken token);
	}
}
