using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Services
{
    public class WhatsappBlast
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://api.fonnte.com/send"; // Endpoint API Fonnte
        private readonly string _token = "ZP2KH1ywANB2bHBb8jhr"; // Ganti dengan token API Fonnte Anda

        public WhatsappBlast()
        {
            // Membuat HttpClient hanya sekali untuk digunakan secara global
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", _token); // Menambahkan token otentikasi ke header
        }

        // Mengirim pesan ke beberapa kontak
        public async Task SendMessagesAsync(List<string> contacts, List<string> names, string message, string countryCode = "62")
        {
            for (int i = 0; i < contacts.Count; i++)
            {
                string contact = contacts[i];
                string name = names[i];
                string personalizedMessage = $"Halo *{name}*,\n{message}";

                try
                {
                    // Kirim pesan ke API WhatsApp menggunakan HTTP POST
                    var response = await SendMessageToWhatsApp(contact, personalizedMessage, countryCode);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Pesan berhasil dikirim ke {contact}");
                    }
                    else
                    {
                        Console.WriteLine($"Gagal mengirim pesan ke {contact}. Status: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Terjadi kesalahan saat mengirim pesan ke {contact}: {ex.Message}");
                }
            }
        }

        // Mengirim pesan ke WhatsApp untuk satu kontak
        private async Task<HttpResponseMessage> SendMessageToWhatsApp(string contact, string message, string countryCode)
        {
            var messageData = new Dictionary<string, string>
            {
                { "target", contact },
                { "message", message },
                { "countryCode", countryCode } // Menggunakan kode negara dari parameter
            };

            var content = new FormUrlEncodedContent(messageData);

            try
            {
                // Mengirimkan request ke API Fonnte
                var response = await _httpClient.PostAsync(_apiUrl, content);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Gagal mengirim pesan melalui API: {ex.Message}");
                throw;
            }
        }
    }
}
