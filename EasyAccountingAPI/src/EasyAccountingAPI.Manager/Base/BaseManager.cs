﻿namespace EasyAccountingAPI.Manager.Base
{
    public abstract class BaseManager<T> : IBaseManager<T> where T : class
    {
        private readonly IBaseRepository<T> _baseRepository;
        public BaseManager(IBaseRepository<T> baseRepository) => _baseRepository = baseRepository;

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _baseRepository.GetAllAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _baseRepository.GetByIdAsync(id);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            return await _baseRepository.CreateAsync(entity);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            return await _baseRepository.UpdateAsync(entity);
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            return await _baseRepository.DeleteAsync(entity);
        }
    }
}