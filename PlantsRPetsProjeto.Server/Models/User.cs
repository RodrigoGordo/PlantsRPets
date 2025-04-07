using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PlantsRPetsProjeto.Server.Models
{
    /// <summary>
    /// Representa um utilizador da aplicação.
    /// Estende <see cref="IdentityUser"/> para incluir dados personalizados.
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// Nome público do utilizador visível na interface.
        /// </summary>
        [PersonalData]
        [Required]
        [Display(Name = "Nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// Data e hora de registo do utilizador na aplicação.
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Perfil do utilizador com dados adicionais (bio, imagem, etc.).
        /// </summary>
        public Profile? Profile { get; set; }

        /// <summary>
        /// Plantações associadas ao utilizador.
        /// </summary>
        public ICollection<Plantation> Plantations { get; set; }

        /// <summary>
        /// Pets atribuídos ao utilizador.
        /// </summary>
        public ICollection<Pet> Pets { get; set; }

        /// <summary>
        /// Dashboard personalizado do utilizador.
        /// </summary>
        public Dashboard? Dashboard { get; set; }

        /// <summary>
        /// Comunidades às quais o utilizador pertence.
        /// </summary>
        public ICollection<Community> Communities { get; set; }

        /// <summary>
        /// Notificações recebidas pelo utilizador.
        /// </summary>
        public ICollection<UserNotification> Notifications { get; set; }

        /// <summary>
        /// Frequência com que o utilizador pretende receber notificações por e-mail.
        /// </summary>
        public EmailFrequency NotificationFrequency { get; set; }

        /// <summary>
        /// Enumeração que define a frequência de envio de notificações por e-mail.
        /// </summary>
        public enum EmailFrequency
        {
            /// <summary>Nunca receber notificações.</summary>
            Never,

            /// <summary>Receber notificações diariamente.</summary>
            Daily,

            /// <summary>Receber notificações semanalmente.</summary>
            Weekly,

            /// <summary>Receber notificações mensalmente.</summary>
            Monthly
        }
    }
}