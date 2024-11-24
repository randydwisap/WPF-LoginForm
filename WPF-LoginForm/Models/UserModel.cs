namespace WPF_LoginForm.Models
{
    public class UserModel
    {
        public int UserID { get; set; } // Ubah menjadi int
        public string Username { get; set; }
        public string Password { get; set; }
        public string NIK { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string NomorHP { get; set; }
        public string ProfilePicture { get; set; }
        public string Role { get; set; }
        public string TandaTangan { get; set; }
    }
}