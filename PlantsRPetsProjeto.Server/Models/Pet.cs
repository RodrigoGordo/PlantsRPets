namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa um animal de estimação virtual (pet) colecionável na aplicação.
    /// Pode ser obtido como recompensa por progresso ou ações sustentáveis.
    /// </summary>
    public class Pet
    {
        /// <summary>
        /// Identificador único do pet.
        /// </summary>
        public int PetId { get; set; }

        /// <summary>
        /// Nome do pet visível ao utilizador.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Categoria ou tipo do pet (ex: "Vegetal + Animal").
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Descrição do pet com informações adicionais.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Estatísticas do pet usadas em mini-jogos ou coleções (vida, ataque, defesa...).
        /// </summary>
        public string BattleStats { get; set; }

        /// <summary>
        /// URL da imagem representativa do pet.
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
