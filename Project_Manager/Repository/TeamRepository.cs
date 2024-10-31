using LR5.Data;
using LR5.Interfaces;
using LR5.Models;
using Microsoft.EntityFrameworkCore;

namespace LR5.Repository
{
	public class CarRepository : ICarRepository
	{
		private readonly ApplicationContext _context;
		public CarRepository(ApplicationContext context)
		{
			_context = context;
		}
		public bool Add(Car car)
		{
			_context.Add(car);
			return Save();
		}

		public bool Delete(Car car)
		{
			_context.Remove(car);
			return Save();
		}

		public async Task<IEnumerable<Car>> GetAll()
		{
			return await _context.Cars.ToListAsync();
		}

		public async Task<Car> GetCarByIdAsync(int id)
		{
			return await _context.Cars.FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<Car> GetCarByIdAsyncNoTracking(int id)
		{
			return await _context.Cars.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<IEnumerable<Car>> GetCarsByGroupId(int id)
		{
			var result = from car in _context.Cars
						 where car.GroupId == id
						 select car;
			return result;
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(Car car)
		{
			_context.Update(car);
			return Save();
		}
	}
}

