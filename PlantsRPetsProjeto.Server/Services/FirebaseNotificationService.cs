using Google.Apis.Auth.OAuth2;
using Google.Apis.FirebaseCloudMessaging.v1;
using Google.Apis.FirebaseCloudMessaging.v1.Data;
using Google.Apis.Services;

public class FirebaseNotificationService
{
    private readonly FirebaseCloudMessagingService _firebaseService;

    public FirebaseNotificationService()
    {
        GoogleCredential credential;
        using (var stream = new FileStream("D:\\Git\\PlantsRPets\\PlantsRPetsProjeto.Server\\plantsrpets-8828.json", FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream);
        }

        _firebaseService = new FirebaseCloudMessagingService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential
        });
    }

    public async Task SendNotificationAsync(string token, string title, string body)
    {
        var message = new Message
        {
            Token = token,
            Notification = new Notification
            {
                Title = title,
                Body = body
            }
        };

        var request = new SendMessageRequest { Message = message };
        await _firebaseService.Projects.Messages.Send(request, "projects/plantsrpets-88284").ExecuteAsync();
    }
}
