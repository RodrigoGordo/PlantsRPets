using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Services
{
    public class PetGeneratorService
    {
        private readonly EmojiKitchenService _emojiKitchenService;
        private readonly Random _random = new Random();

        private readonly List<string> _vegetableFruitPlantEmojis = new List<string>
        {
            "🍄", "🌻", "🌺", "🌼", "🥦", "🥕",
            "🌽", "🍆", "🍅", "🥑", "🍇", "🍈", "🍉", "🍊", "🍋", "🍌",
            "🍍", "🥭", "🍎", "🍏", "🍐", "🍑", "🍒", "🍓", "🥝", "🥥"
        };

        private readonly List<string> _funnyFaceAnimalEmojis = new List<string>
        {
            "😀", "😃", "😄", "😁", "😆", "😅", "😂", "🤣", "😊", "😇",
            "🙂", "🙃", "😉", "😌", "😍", "🥰", "😘", "😗", "😙", "😚",
            "😋", "😛", "😝", "😜", "🤪", "🤨", "🧐", "🤓", "😎", "🥳",
            "🐶", "🐱", "🐭", "🐹", "🐰", "🦊", "🐻", "🐼", "🐨", "🐯",
            "🦁", "🐮", "🐷", "🐸", "🐵", "🐔", "🐧", "🐦", "🐤", "🦄"
        };

        public PetGeneratorService(EmojiKitchenService emojiKitchenService)
        {
            _emojiKitchenService = emojiKitchenService;
        }

        public async Task<Pet> GeneratePetAsync()
        {
            var vegetableFruitPlant = _vegetableFruitPlantEmojis[_random.Next(_vegetableFruitPlantEmojis.Count)];
            var funnyFaceAnimal = _funnyFaceAnimalEmojis[_random.Next(_funnyFaceAnimalEmojis.Count)];

            // Generate the pet image URL
            string imageUrl;
            try
            {
                imageUrl = await _emojiKitchenService.GeneratePetImageAsync(vegetableFruitPlant, funnyFaceAnimal);
            }
            catch
            {
                // If the combination fails, use a fallback image
                imageUrl = "https://via.placeholder.com/128";
            }

            string name = $"{vegetableFruitPlant}{funnyFaceAnimal} Pet";
            string type = "Plant/Fruit/Vegetable + Animal/Face";
            string details = $"A unique pet made from {vegetableFruitPlant} and {funnyFaceAnimal}.";
            string battleStats = GenerateBattleStats();

            return new Pet
            {
                Name = name,
                Type = type,
                Details = details,
                BattleStats = battleStats,
                ImageUrl = imageUrl
            };
        }

        private string GenerateBattleStats()
        {
            int health = _random.Next(50, 100);
            int attack = _random.Next(10, 30);
            int defense = _random.Next(10, 30);
            int speed = _random.Next(10, 30);

            return $"Health: {health}, Attack: {attack}, Defense: {defense}, Speed: {speed}";
        }

    }
}
