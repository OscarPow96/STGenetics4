using System.Collections.Generic;
using System.Threading.Tasks;
using STGenetics.Shared.Models;

namespace STGenetics.Shared.Services
{
    public interface IAnimalService
    {
        Task<List<Animal>> GetAnimalsFromJson();
        Task<Animal> GetAnimalById(int animalId);
        Task<List<Animal>> GetSelectedAnimals();  
        Task SaveAnimal(Animal animal);
        Task DeleteAnimal(int animalId);
        Task UpdateAnimal(Animal animal);
    }
}
