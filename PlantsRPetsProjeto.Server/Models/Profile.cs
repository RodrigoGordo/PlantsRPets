namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa o perfil público de um utilizador.
    /// Contém informações adicionais como biografia, imagem de perfil e preferências pessoais.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Identificador único do perfil.
        /// </summary>
        public int ProfileId { get; set; }

        /// <summary>
        /// Identificador do utilizador a que este perfil pertence.
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// Biografia ou descrição pessoal do utilizador.
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// Caminho relativo para a imagem de perfil do utilizador.
        /// </summary>
        public string? ProfilePicture { get; set; }

        /// <summary>
        /// Lista de identificadores dos pets favoritos do utilizador.
        /// </summary>
        public ICollection<int>? FavoritePets { get; set; }

        /// <summary>
        /// Lista de identificadores das plantações destacadas pelo utilizador.
        /// </summary>
        public ICollection<int>? HighlightedPlantations { get; set; }
    }
}
