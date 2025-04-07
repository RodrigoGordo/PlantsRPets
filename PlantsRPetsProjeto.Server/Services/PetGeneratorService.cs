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

        /// <summary>
        /// Lista de emojis relacionados com plantas, frutas e vegetais que serão usados na composição do mascote.
        /// </summary>
        private readonly List<string> _vegetableFruitPlantEmojis = new List<string>
        {
            "🍄", "🌻", "🌺", "🌼", "🥦", "🥕",
            "🥑", "🍇", "🍉", "🍊", "🍋", "🍌",
            "🍍", "🍏", "🍒", "🍓", "🥝", "🥥"
        };

        /// <summary>
        /// Lista de emojis de expressões faciais e animais utilizados na composição do mascote.
        /// </summary>
        private readonly List<string> _funnyFaceAnimalEmojis = new List<string>
        {
            "😀", "😃", "😄", "😁", "😆", "😅", "😂", "🤣", "😊", "😇",
            "🙂", "🙃", "😉", "😌", "😍", "🥰", "😘", "😗", "😙", "😚",
            "😋", "😛", "😝", "😜", "🤪", "🤨", "🧐", "🤓", "😎", "🥳",
            "🐶", "🐱", "🦊", "🐻", "🐼", "🐯",
            "🦁", "🐮", "🐷", "🐸", "🐔", "🐧", "🦄", "💎", "☢️"
        };

        /// <summary>
        /// Construtor do serviço de geração de mascotes.
        /// </summary>
        /// <param name="emojiKitchenService">Serviço utilizado para gerar a imagem visual do mascote com base em dois emojis.</param>
        public PetGeneratorService(EmojiKitchenService emojiKitchenService)
        {
            _emojiKitchenService = emojiKitchenService;
        }

        /// <summary>
        /// Gera um novo mascote aleatório, combinando dois emojis e criando a imagem correspondente.
        /// </summary>
        /// <returns>Objeto <see cref="Pet"/> contendo os dados do mascote ou null se a imagem não puder ser gerada.</returns>
        public async Task<Pet?> GeneratePetAsync()
        {
            var vegetableFruitPlant = _vegetableFruitPlantEmojis[_random.Next(_vegetableFruitPlantEmojis.Count)];
            var funnyFaceAnimal = _funnyFaceAnimalEmojis[_random.Next(_funnyFaceAnimalEmojis.Count)];

            // Generate the pet image URL
            string imageUrl;
           
            imageUrl = await _emojiKitchenService.GeneratePetImageAsync(vegetableFruitPlant, funnyFaceAnimal);

            if (imageUrl == "")
            {
                return null;
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

        /// <summary>
        /// Gera atributos aleatórios de combate para o mascote (vida, ataque, defesa e velocidade).
        /// </summary>
        /// <returns>String formatada com os valores gerados.</returns>
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
