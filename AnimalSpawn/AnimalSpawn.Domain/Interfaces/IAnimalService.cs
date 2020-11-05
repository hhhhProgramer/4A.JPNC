using AnimalSpawn.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnimalSpawn.Domain.Interfaces
{
    public interface IAnimalService
    {
         void AddAnimal(Animal animal);
        IEnumerable<Animal> GetAnimals();
        void DeleteAnimal(int id);
        void UpdateAnimal(Animal animal);

    }

}
