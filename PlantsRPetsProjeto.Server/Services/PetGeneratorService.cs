using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlantsRPetsProjeto.Server.Models;

namespace PlantsRPetsProjeto.Server.Services
{
    /// <summary>
    /// Serviço responsável por gerar mascotes únicos combinando emojis de plantas/frutas/legumes com animais ou expressões faciais.
    /// Utiliza o serviço <see cref="EmojiKitchenService"/> para criar a imagem visual do mascote.
    /// </summary>
    public class PetGeneratorService
    {
        private readonly EmojiKitchenService _emojiKitchenService;
        private readonly Random _random = new Random();

        private readonly HashSet<string> _usedCombinations = new();

        private readonly List<string> _vegetableFruitPlantEmojis = new List<string>
        {
            "🍄", "🌻", "🌺", "🌼", "🥦", "🥕",
            "🥑", "🍇", "🍉", "🍊", "🍋", "🍌",
            "🍍", "🍏", "🍒", "🍓", "🥝", "🥥"
        };

        private readonly List<string> _funnyFaceAnimalEmojis = new List<string>
        {
            "😀", "😃", "😄", "😁", "😆", "😅", "😂", "🤣", "😊", "😇",
            "🙂", "🙃", "😉", "😌", "😍", "🥰", "😘", "😗", "😙", "😚",
            "😋", "😛", "😝", "😜", "🤪", "🤨", "🧐", "🤓", "😎", "🥳",
            "🐶", "🐱", "🦊", "🐻", "🐼", "🐯",
            "🦁", "🐮", "🐷", "🐸", "🐔", "🐧", "🦄", "💎", "☢️"
        };

        public PetGeneratorService(EmojiKitchenService emojiKitchenService)
        {
            _emojiKitchenService = emojiKitchenService;
        }

        /// <summary>
        /// Gera um novo mascote aleatório, tentando evitar duplicados e combinações inválidas.
        /// </summary>
        /// <returns>Objeto <see cref="Pet"/> ou null se falhar após várias tentativas.</returns>
        public async Task<Pet?> GeneratePetAsync()
        {
            const int maxAttempts = 20;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                var emoji1 = _vegetableFruitPlantEmojis[_random.Next(_vegetableFruitPlantEmojis.Count)];
                var emoji2 = _funnyFaceAnimalEmojis[_random.Next(_funnyFaceAnimalEmojis.Count)];

                var (e1, e2) = emoji1.CompareTo(emoji2) < 0 ? (emoji1, emoji2) : (emoji2, emoji1);
                string key = $"{e1}_{e2}";

                if (_usedCombinations.Contains(key))
                    continue;

                string imageUrl = await _emojiKitchenService.GeneratePetImageAsync(e1, e2);

                if (string.IsNullOrEmpty(imageUrl))
                    continue;

                _usedCombinations.Add(key);

                string name = $"{e1}{e2} Pet";
                string type = "Plant/Fruit/Vegetable + Animal/Face";
                string details = $"A unique pet made from {e1} and {e2}.";
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

            Console.WriteLine("⚠️ Could not generate a valid unique pet after multiple attempts.");
            return null;
        }

        /// <summary>
        /// Gera atributos aleatórios de combate para o mascote.
        /// </summary>
        /// <returns>String formatada com os valores de combate.</returns>
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
