namespace Portfolio.Utils
{
    public static class EmailSettings
    {
        public static string MailFromAddress = "sakibur.rahman.cse@gmail.com";
        public static bool UseSsl = true;
        public static string Username = MailFromAddress;
        public static string Password = "your app passsword from google";
        public static string ServerName = "smtp.gmail.com";
        public static int ServerPort = 587;
    }
}
