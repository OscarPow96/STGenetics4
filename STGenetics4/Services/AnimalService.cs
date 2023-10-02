using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using STGenetics.Shared.Models;

namespace STGenetics.Shared.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly string _jsonFilePath = "F:\\Programas\\VisualStudio repos\\STGenetics4\\STGenetics4\\Data\\animal.json";        // Change route where are the archive animal.json

        public async Task<List<Animal>> GetAnimalsFromJson()
        {
            try
            {
                using (var jsonFileReader = File.OpenText(_jsonFilePath))
                {
                    var animalsJson = await jsonFileReader.ReadToEndAsync();
                    var deserializedAnimals = JsonSerializer.Deserialize<List<Animal>>(animalsJson,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return deserializedAnimals ?? new List<Animal>();  // Return Empty List if it is Null
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al cargar los animales desde el archivo JSON: {e.Message}");
                throw;
            }
        }

        public async Task<Animal> GetAnimalById(int animalId)
        {
            try
            {
                var animals = await GetAnimalsFromJson();

                if (animals != null)
                    return animals.FirstOrDefault(a => a.AnimalId == animalId);
                else
                    return null;  // Return null if animals is null
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting animal by ID: {e.Message}");
                throw;
            }
        }




        public async Task SaveAnimal(Animal animal)
        {
            try
            {
                var existingAnimals = await GetAnimalsFromJson();

                var existingAnimal = existingAnimals.FirstOrDefault(a => a.AnimalId == animal.AnimalId);
                if (existingAnimal != null)
                {
                    existingAnimal.Name = animal.Name;
                    existingAnimal.Breed = animal.Breed;
                    existingAnimal.IsSelected = animal.IsSelected;  
                }
                else
                {
                    // Agregamos el nuevo animal a la lista
                    existingAnimals.Add(animal);
                }

                var updatedAnimalsJson = JsonSerializer.Serialize(existingAnimals);
                await File.WriteAllTextAsync(_jsonFilePath, updatedAnimalsJson);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al guardar el animal: {e.Message}");
                throw;
            }
        }


        public async Task DeleteAnimal(int animalId)
        {
            try
            {
                var existingAnimals = await GetAnimalsFromJson();

                // Find the animal by AnimalId
                var animalToRemove = existingAnimals.FirstOrDefault(a => a.AnimalId == animalId);

                // Remove the animal if found
                if (animalToRemove != null)
                {
                    existingAnimals.Remove(animalToRemove);

                    // Serialize and write the updated list back to the JSON file
                    var updatedAnimalsJson = JsonSerializer.Serialize(existingAnimals);
                    await File.WriteAllTextAsync(_jsonFilePath, updatedAnimalsJson);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error deleting animal: {e.Message}");
                throw;
            }
        }

        public async Task UpdateAnimal(Animal animal)
        {
            try
            {
                var existingAnimals = await GetAnimalsFromJson();

                // Check if the animal already exists based on some unique identifier (e.g., AnimalId)
                var existingAnimal = existingAnimals.Find(a => a.AnimalId == animal.AnimalId);
                if (existingAnimal != null)
                {
                    // Update the existing animal
                    existingAnimal.Name = animal.Name;
                    existingAnimal.Breed = animal.Breed;
                    // Update other properties as needed

                    // Serialize and write the updated list back to the JSON file
                    var updatedAnimalsJson = JsonSerializer.Serialize(existingAnimals);
                    await File.WriteAllTextAsync(_jsonFilePath, updatedAnimalsJson);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error updating animal: {e.Message}");
                throw;
            }
        }

        public async Task<List<Animal>> GetSelectedAnimals()
        {
            try
            {
                var allAnimals = await GetAnimalsFromJson();
                return allAnimals.Where(a => a.IsSelected).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al obtener los animales seleccionados: {e.Message}");
                throw;
            }
        }



    }
}
