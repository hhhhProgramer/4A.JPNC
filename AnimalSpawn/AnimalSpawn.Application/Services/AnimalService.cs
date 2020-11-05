using AnimalSpawn.Domain.Entities;
using AnimalSpawn.Domain.Exceptions;
using AnimalSpawn.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AnimalSpawn.Application.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnimalService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void  AddAnimal(Animal animal)
        {
            var older = DateTime.Now - (animal?.CaptureDate ?? DateTime.Now);

            Expression<Func<Animal, bool>> exprAnimal = item => item.Name == animal.Name;
            var animals =  _unitOfWork.AnimalRepository.FindByCondition(exprAnimal);
            if (animal.RfidTag != null)
            {
                Expression<Func<RfidTag, bool>> exprTag = item => item.Tag == animal.RfidTag.Tag;
                var tags = _unitOfWork.RfidTagRepository.FindByCondition(exprTag);
                if (tags.Any())
                    throw new BusinessException("This animal's tag rfid already exist.");
            }
            if (animals.Any())
                throw new BusinessException("This animal name already exist.");
            if (animal?.EstimatedAge > 0 && (animal?.Weight <= 0 || animal?.Height <= 0))
                throw new BusinessException("The height and weight should be greater than zero.");
            if (older.TotalDays > 45)
                throw new BusinessException("The animal's capture date is older than 45 days");

        }

        public void DeleteAnimal(int id)
        {
             _unitOfWork.AnimalRepository.Delete(id);
             _unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<Animal> GetAnimals()
        {
            return _unitOfWork.AnimalRepository.GetAll();
        }


        public void UpdateAnimal(Animal animal)
        {
            _unitOfWork.AnimalRepository.Update(animal);
            _unitOfWork.SaveChangesAsync();
        }

    }
}
