using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Windows; // Tambahkan namespace ini untuk Stopwatch

namespace WPF_LoginForm.Services
{
    public class WhatsappBlast
    {
        private IWebDriver driver;

        public WhatsappBlast()
        {
            // Menentukan path profil Chrome yang sudah ada
            var options = new ChromeOptions();
            options.AddArgument("user-data-dir=C:\\Users\\dwisa\\AppData\\Local\\Google\\Chrome\\User Data"); // Ganti dengan path profil Chrome yang sesuai
            options.AddArgument("profile-directory=Default"); // Atau ganti dengan profil lain jika diperlukan

            // Konfigurasi Selenium WebDriver dengan opsi profil Chrome
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://web.whatsapp.com");

            Console.WriteLine("Silakan scan kode QR jika belum login.");

            // WebDriverWait untuk menunggu element login
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50)); // Awalnya tunggu 15 detik
            bool isLoggedIn = false;

            // Cek status login dengan mencari elemen menu di WhatsApp Web
            while (!isLoggedIn)
            {
                try
                {
                    var menuIcon = wait.Until(d => d.FindElement(By.XPath("//span[@data-icon='menu']"))); // Cari ikon menu
                    Console.WriteLine("Login berhasil.");
                    isLoggedIn = true; // Set isLoggedIn ke true jika menu ditemukan
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("Menu tidak ditemukan dalam waktu yang ditentukan. Mencoba lagi dalam 30 detik...");
                    wait.Timeout = TimeSpan.FromSeconds(30); // Coba lagi setelah 30 detik
                }
            }
        }

        public void SendMessages(List<string> contacts, string message, string filePath)
        {
            foreach (var contact in contacts)
            {
                try
                {
                    // Cari kontak
                    var searchBox = driver.FindElement(By.XPath("//div[@contenteditable='true'][@data-tab='3']"));
                    searchBox.Clear();
                    searchBox.SendKeys(contact);
                    searchBox.SendKeys(Keys.Enter);
                    Thread.Sleep(2000); // Tunggu beberapa detik untuk pencarian

                    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    {
                        // Kirim pesan teks jika filePath kosong
                        var messageBox = driver.FindElement(By.XPath("//div[@contenteditable='true'][@data-tab='10']"));
                        messageBox.Clear();

                        // Mengirim pesan dengan baris baru menggunakan Shift + Enter
                        foreach (var line in message.Split('\n'))
                        {
                            messageBox.SendKeys(line);
                            messageBox.SendKeys(Keys.Shift + Keys.Enter); // Shift + Enter untuk baris baru
                        }

                        messageBox.SendKeys(Keys.Enter); // Kirim pesan untuk chat tanpa file
                        Thread.Sleep(2000); // Tunggu beberapa detik agar pesan terkirim
                        MessageBox.Show("Chat terkirim.");
                    }
                    else
                    {
                        // Kirim pesan dengan file jika filePath ada
                        var attachIcon = driver.FindElement(By.XPath("//span[@data-icon='plus']"));
                        attachIcon.Click();
                        Thread.Sleep(1000); // Tunggu untuk memunculkan dialog file

                        // Pilih input file dan kirim file
                        var fileInput = driver.FindElement(By.XPath("//input[@type='file']"));
                        fileInput.SendKeys(filePath); // Kirimkan path file

                        Thread.Sleep(5000); // Tunggu beberapa detik agar file dapat diupload

                        // Kirim pesan teks dengan beberapa baris setelah file dikirim
                        var messageBox = driver.FindElement(By.XPath("//div[@contenteditable='true'][@data-tab='undefined']"));
                        messageBox.Clear();

                        // Mengirim pesan dengan baris baru menggunakan Shift + Enter
                        foreach (var line in message.Split('\n'))
                        {
                            messageBox.SendKeys(line);
                            messageBox.SendKeys(Keys.Shift + Keys.Enter); // Shift + Enter untuk baris baru
                        }

                        Thread.Sleep(2000); // Tunggu beberapa detik agar pesan terkirim

                        // Kirim file
                        var sendButton = driver.FindElement(By.XPath("//span[@data-icon='send']"));
                        sendButton.Click();
                        Thread.Sleep(2000); // Tunggu beberapa detik untuk pengiriman file
                        MessageBox.Show("File terkirim.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Gagal mengirim pesan atau file ke {contact}: {ex.Message}");
                }
            }
        }
        public void Close()
        {
            try
            {
                Console.WriteLine("Menutup sesi WhatsApp Web dan menutup browser...");
                driver.Quit(); // Menutup browser dan menghentikan session WebDriver
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat menutup sesi: {ex.Message}");
            }
        }

    }
}
